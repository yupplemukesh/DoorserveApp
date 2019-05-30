using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Repository.ServiceCenters;
using TogoFogo.Models.ServiceCenter;
using TogoFogo.Models.ClientData;
using TogoFogo.Permission;
using TogoFogo.Repository;
using TogoFogo.Repository.ImportFiles;

namespace TogoFogo.Controllers
{
    public class ClientDataController : Controller
    {
        private readonly IUploadFiles _RepoUploadFile;
        private readonly ICallLog _RepoCallLog;
        private readonly ICenter _centerRepo;
        private readonly IContactPerson _contactPerson;
        private readonly DropdownBindController _dropdown;
        
        public ClientDataController()
        {
            _RepoUploadFile = new UploadFiles();
            _dropdown = new DropdownBindController();
            _RepoCallLog = new CallLog();
            _centerRepo = new Center();
            _contactPerson = new ContactPerson();

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> Index()
        {
            var session = Session["User"] as SessionModel;
            ViewBag.PageNumber = (Request.QueryString["grid-page"] == null) ? "1" : Request.QueryString["grid-page"];
            bool IsClient = false;
            var filter = new FilterModel { CompId = session.CompanyId };
            if (session.UserTypeName.ToLower().Contains("client"))
            {
                filter.ClientId = session.RefKey;
                IsClient = true;
            }
            var clientData = _RepoUploadFile.GetUploadedList(filter);

            var serviceType = await CommonModel.GetServiceType(session.CompanyId);
            var deliveryType = await CommonModel.GetDeliveryServiceType(session.CompanyId);
            clientData.Client = new ClientDataModel();
            clientData.Client.IsClient = IsClient;
            clientData.Client.ClientId = filter.ClientId;
            clientData.Client.ClientList = new SelectList(await CommonModel.GetClientData(session.CompanyId), "Name", "Text");
            clientData.Client.ServiceTypeList = new SelectList(serviceType, "Value", "Text");
            clientData.Client.DeliveryTypeList = new SelectList(deliveryType, "Value", "Text");

            // new call Log
            clientData.NewCallLog = new CallDetailsModel
            {
                ClientList = clientData.Client.ClientList,
                ServiceTypeList = clientData.Client.ServiceTypeList,
                DeliveryTypeList = clientData.Client.DeliveryTypeList,
                BrandList = new SelectList(_dropdown.BindBrand(session.CompanyId), "Value", "Text"),
                CategoryList = new SelectList(_dropdown.BindCategory(session.CompanyId), "Value", "Text"),
                ProductList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text"),
                ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text"),
                IsClient = IsClient,
                // address=new AddressDetail
                //{
                AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text"),
                CityList = new SelectList(Enumerable.Empty<SelectListItem>()),
                StateList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text"),
                // }

            };
            return View(clientData);
        }

        public async Task<ActionResult> GetAssignedCalls()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId };
            if (session.UserRole.ToLower().Contains("client"))
                filter.ClientId = session.RefKey;
            var calls = await _RepoUploadFile.GetAssingedCalls(filter);
            return PartialView("_TotalCallsList", calls);
        }




        private string SaveFile(HttpPostedFileBase file, string folderName)
        {
            try
            {
                string path = Server.MapPath("~/Files/" + folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return path + "\\" + savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> Create()
        {
            var session = Session["User"] as SessionModel;
            var clientDate = new ClientDataModel();
            clientDate.ClientList = new SelectList(await CommonModel.GetClientData(session.CompanyId), "Name", "Text");
            clientDate.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(session.CompanyId), "Value", "Text");
            return View(clientDate);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Assign_Calls)]
        [HttpPost]        
        public async Task<ActionResult> Upload(ClientDataModel clientDataModel)
        {
            var session = Session["User"] as SessionModel;
            clientDataModel.CompanyId = session.CompanyId;
            clientDataModel.UserId = session.UserId;
            if (clientDataModel.IsClient)
                clientDataModel.ClientId = session.RefKey;
            if (clientDataModel.DataFile != null)
            {
                string excelPath = SaveFile(clientDataModel.DataFile, "ClientData");
                string conString = string.Empty;
                string extension = Path.GetExtension(clientDataModel.DataFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;

                }

                conString = string.Format(conString, excelPath);
                DataTable dtExcelData = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    dtExcelData.Columns.AddRange(new DataColumn[20] {
                new DataColumn("CUSTOMER TYPE", typeof(string)),
                new DataColumn("CUSTOMER NAME", typeof(string)),
                new DataColumn("Customer Contact Number", typeof(string)),
                new DataColumn("Customer Email", typeof(string)),
                new DataColumn("CUSTOMER ADDRESS TYPE", typeof(string)),
                new DataColumn("CUSTOMER ADDRESS", typeof(string)),
                new DataColumn("CUSTOMER COUNTRY", typeof(string)),
                new DataColumn("CUSTOMER STATE", typeof(string)),
                new DataColumn("CUSTOMER CITY", typeof(string)),
                new DataColumn("CUSTOMER PINCODE", typeof(string)),
                new DataColumn("DEVICE CATEGORY", typeof(string)),
                new DataColumn("DEVICE BRAND", typeof(string)),
                new DataColumn("DEVICE NAME", typeof(string)),
                new DataColumn("DEVICE MODEL", typeof(string)),
                new DataColumn("DEVICE IMEI FIRST", typeof(string)),
                new DataColumn("DEVICE IMEI SECOND", typeof(string)),
                new DataColumn("DEVICE SN", typeof(string)),
                new DataColumn("DOP", typeof(DateTime)),
                new DataColumn("PURCHASE FROM", typeof(string)),
                new DataColumn("DEVICE CONDITION", typeof(string))
                });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [CUSTOMER TYPE], [CUSTOMER NAME],[CUSTOMER Contact Number],[CUSTOMER EMAIL]," +
                        "[CUSTOMER ADDRESS TYPE],[CUSTOMER ADDRESS],[CUSTOMER COUNTRY],[CUSTOMER STATE],[CUSTOMER CITY]," +
                        "[CUSTOMER PINCODE],[DEVICE CATEGORY],[DEVICE BRAND],[DEVICE MODEL],[DEVICE IMEI FIRST]," +
                        "[DEVICE IMEI SECOND],[DEVICE SN],[DOP],[PURCHASE FROM],[DEVICE CONDITION]  FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                }
                try
                {

                    var response = await _RepoUploadFile.UploadClientData(clientDataModel, dtExcelData);
                    if (!response.IsSuccess)
                        System.IO.File.Delete(excelPath);
                    TempData["response"] = response;
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    if (System.IO.File.Exists(Server.MapPath(excelPath)))
                        System.IO.File.Delete(Server.MapPath(excelPath));
                    return RedirectToAction("index");

                }
            }
            return RedirectToAction("index");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Assign_Calls)]
        [HttpPost]
        public async Task<ActionResult> NewCallLog(UploadedExcelModel uploads)
        {
            var session = Session["User"] as SessionModel;

            uploads.UserId = session.UserId;
            uploads.CompanyId = session.CompanyId;
            if (session.UserRole.ToLower().Contains("client"))
                uploads.ClientId = session.RefKey;
            uploads.EventAction = 'I';
            var response = await _RepoCallLog.AddOrEditCallLog(uploads);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Assign_Calls)]
        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, tabIndex = tabIndex };
            if (session.UserRole.ToLower().Contains("Client"))
                filter.ClientId = session.RefKey;
            var response = await _RepoUploadFile.GetExportAssingedCalls(filter);
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'O')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail","CustomerCity",
                "ServiceTypeName","DeliveryTypeName","DeviceCategory","DeviceBrand","DeviceModel","DeviceSn"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.OpenCalls, "", true, columns);
            }
            else if (tabIndex == 'C')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail","CustomerCity",
                "ServiceTypeName","DeliveryTypeName","DeviceCategory","DeviceBrand","DeviceModel","DeviceSn"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.CloseCalls, "", true, columns);
            }
            else if (tabIndex == 'D')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail","CustomerCity",
                "ServiceTypeName","DeliveryTypeName","DeviceCategory","DeviceBrand","DeviceModel","DeviceSn"};
                filecontent = ExcelExportHelper.ExportExcel(response.UploadedData, "", true, columns);
            }
            else if(tabIndex=='F')
            {               
                    columns = new string[]{"UploadedDate","UserName","UploadedFileName","ServiceType","ServiceDeliveryType",
                    "TotalRecords","UploadedRecords","FailedRecords"};
                    filecontent = ExcelExportHelper.ExportExcel(response.UploadedFiles, "", true, columns);
               
            }
            else

            {
                columns = new string[]{"CustomerType","CustomerName","CustomerContactNumber","CustomerEmail","CustomerAddressType",
"CustomerAddress","CustomerCountry","CustomerState","CustomerCity","CustomerPincode","DeviceCategory","DeviceBrand", "DeviceModel","DeviceSn",
                    "DOP","PurchaseFrom","DeviceIMEIOne","DeviceIMEISecond","DeviceCondition"};
                var devices = new List<UploadedExcelModel> { new UploadedExcelModel
                {
                    CustomerType = "CORPORATE",
                    CustomerName = "Rahul Singh",
                    CustomerContactNumber = "9993344444",
                    CustomerEmail = "rahul@gmail.com",
                    CustomerAddressType = "Home",
                    CustomerAddress = "F-451 Okhala Phase-1",
                    CustomerCountry = "India",
                    CustomerState = "Delhi",
                    CustomerCity = "New Delhi",
                    CustomerPincode = "110020",
                    DeviceCategory = "Mobiles",
                    DeviceBrand = "Samsung",
                    DeviceModel = "Samsung A5-16",
                    DeviceIMEIOne = "2456675644433",
                    DeviceIMEISecond = "8756454444445",
                    DeviceSn = "#45444",
                    DOP = Convert.ToDateTime("10/12/2019"),
                    PurchaseFrom = "Delhi",
                    DeviceCondition = "Brand New",
                    

                }};
                filecontent = ExcelExportHelper.ExportExcel(devices, "", false, columns);
            }

            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> Edit(string Crn)
        {
            var SessionModel = Session["User"] as SessionModel;
            var CallDetailsModel = await _centerRepo.GetCallsDetailsById(Crn);
            CallDetailsModel.ClientList = new SelectList(await CommonModel.GetClientData(SessionModel.CompanyId), "Name", "Text");
            CallDetailsModel.BrandList = new SelectList(_dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CategoryList = new SelectList(_dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.ProductList = new SelectList(_dropdown.BindProduct(CallDetailsModel.DeviceBrandId), "Value", "Text");
            CallDetailsModel.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CallDetailsModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CallDetailsModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "Value", "Text");
            CallDetailsModel.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CallDetailsModel.StateList = new SelectList(_dropdown.BindState(CallDetailsModel.CountryId), "Value", "Text");
            CallDetailsModel.CityList = new SelectList(_dropdown.BindLocation(CallDetailsModel.StateId), "Value", "Text");    
          
            return View("_EditForm", CallDetailsModel);

        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Assign_Calls)]
        [HttpPost]
        public async Task<ActionResult> Edit(CallDetailsModel CallDetailsModel)
        {
            try
            {
                var SessionModel = Session["User"] as SessionModel;
                CallDetailsModel.UserId = SessionModel.UserId;
                CallDetailsModel.CompanyId = SessionModel.CompanyId;
                CallDetailsModel.EventAction = 'U';
                var response = await _RepoCallLog.AddOrEditCallLog(CallDetailsModel);
                TempData["response"] = response;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var response = new ResponseModel { Response = ex.Message, IsSuccess = false };
                TempData["response"] = response;
                TempData.Keep("response");
                //return Json("ex", JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }

        }

        
    


    }

}
