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
using doorserve.Filters;
using doorserve.Models;
using doorserve.Repository.ServiceCenters;
using doorserve.Models.ServiceCenter;
using doorserve.Models.ClientData;
using doorserve.Permission;
using doorserve.Repository;
using doorserve.Repository.ImportFiles;

namespace doorserve.Controllers
{
    [ValidateModel]
    public class ClientDataController : Controller
    {
        private readonly IUploadFiles _RepoUploadFile;
        private readonly ICallLog _RepoCallLog;
        private readonly ICenter _centerRepo;
        private readonly IContactPerson _contactPerson;
        private readonly DropdownBindController _dropdown;
        private string FilePath = "~/Files/";
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
            var clientData = new MainClientDataModel();
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
                DataSourceId=101,
             IsAssingedCall = true,
            ClientList = clientData.Client.ClientList,
                ServiceTypeList = clientData.Client.ServiceTypeList,
                DeliveryTypeList = clientData.Client.DeliveryTypeList,
                BrandList = new SelectList(_dropdown.BindBrand(session.CompanyId), "Value", "Text"),
                CategoryList = new SelectList(_dropdown.BindCategory(session.CompanyId), "Value", "Text"),
                SubCategoryList =new SelectList(Enumerable.Empty<SelectListItem>()),
                ProductList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text"),
                ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text"),
                IsClient = IsClient,
                StatusList=new SelectList(await CommonModel.GetStatusTypes("Client"), "Value", "Text"),
                // address=new AddressDetail
                //{
                AddressTypelist = new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text"),
                LocationList = new SelectList(Enumerable.Empty<SelectListItem>()),
              
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
                string path = Server.MapPath(FilePath + folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return  savedFileName;
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
            string excelPath = Server.MapPath(FilePath + "ClientData");
            if (clientDataModel.DataFile != null)
            {
                string FileName = SaveFile(clientDataModel.DataFile, "ClientData");
                clientDataModel.FileName = FileName;
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
                conString = string.Format(conString, excelPath+"/"+FileName);
                DataTable dtExcelData = new DataTable();
                var cols = new List<DataColumn> {
                new DataColumn("Customer Type", typeof(string)),
                new DataColumn("Customer Name", typeof(string)),
                new DataColumn("Customer Contact Number", typeof(string)),
                new DataColumn("Customer Alt Con Number", typeof(string)),
                new DataColumn("Customer Email", typeof(string)),
                new DataColumn("Customer Address Type", typeof(string)),
                new DataColumn("Customer Address", typeof(string)),
                new DataColumn("Customer Country", typeof(string)),
                new DataColumn("Customer State", typeof(string)),
                new DataColumn("Customer City", typeof(string)),
                new DataColumn("Customer Pincode", typeof(string)),
                new DataColumn("Device Category", typeof(string)),
                new DataColumn("Device Sub Category", typeof(string)),
                new DataColumn("Device Brand", typeof(string)),
                new DataColumn("Device Model", typeof(string)),
                new DataColumn("Device Model No", typeof(string)),
                new DataColumn("Device Sn", typeof(string)),
                new DataColumn("DOP", typeof(string)),
                new DataColumn("Purchase From", typeof(string)),
                new DataColumn("Device IMEI First", typeof(string)),
                new DataColumn("Device IMEI Second", typeof(string)),
                new DataColumn("Device Condition", typeof(string)),
                new DataColumn("PROBLEM DESCRIPTION", typeof(string)),
                new DataColumn("ISSUE OCURRING SINCE DATE", typeof(string))
                };
               
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();
                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    dtExcelData.Columns.AddRange(cols.ToArray());
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "] where  [Customer Name] is not null", excel_con))
                    {
                            oda.Fill(dtExcelData);
                    }
                    excel_con.Close();
                }                                                   
                try
                {
                    var response = await _RepoUploadFile.UploadClientData(clientDataModel, dtExcelData);
                    if (!response.IsSuccess)
                        System.IO.File.Delete(excelPath+'/' + clientDataModel.FileName);
                    TempData["response"] = response;
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {

                        
                    if (System.IO.File.Exists(excelPath+'/'+ clientDataModel.FileName))
                        System.IO.File.Delete(excelPath+'/'+ clientDataModel.FileName);
                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("index");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Assign_Calls)]
        [HttpPost]
        public async Task<ActionResult> NewCallLog(CallDetailsModel uploads)
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
                "ServiceTypeName","DeliveryTypeName","DeviceCategory","DeviceSubcategory", "DeviceBrand","DeviceModel", "ModelNumber","DeviceSn"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.OpenCalls, "", true, columns);
            }
            else if (tabIndex == 'C')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail","CustomerCity",
                "ServiceTypeName","DeliveryTypeName","DeviceCategory", "DeviceSubcategory","DeviceBrand","DeviceModel", "ModelNumber", "DeviceSn"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.CloseCalls, "", true, columns);
            }
            else if (tabIndex == 'D')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail","CustomerCity",
                "ServiceTypeName","DeliveryTypeName","DeviceCategory","DeviceSubcategory","DeviceBrand","DeviceModel", "DeviceModel","DeviceSn"};
                filecontent = ExcelExportHelper.ExportExcel(response.UploadedData, "", true, columns);
            }
            else if (tabIndex == 'F')
            {
                columns = new string[]{"UploadedDate","UserName","UploadedFileName","ServiceType","ServiceDeliveryType",
                    "TotalRecords","UploadedRecords","FailedRecords"};
                filecontent = ExcelExportHelper.ExportExcel(response.UploadedFiles, "", true, columns);

            }



            else if(tabIndex=='T')
            {
                columns = new string[]{"CustomerType","CustomerName","CustomerContactNumber","CustomerAltConNumber","CustomerEmail","CustomerAddressType",
"CustomerAddress","CustomerCountry","CustomerState","CustomerCity","CustomerPincode","DeviceCategory","DeviceSubCategory","DeviceBrand","DeviceModel","DeviceModelNo","DeviceSn",
                    "DOP","PurchaseFrom","DeviceIMEIOne","DeviceIMEISecond","DeviceCondition","ProblemDescription","IssueOcurringSinceDate"};
                var devices = new List<ReportedProblemModel> { new ReportedProblemModel
                {
                    CustomerType = "CORPORATE",
                    CustomerName = "Rahul Singh",
                    CustomerContactNumber = "9993344444",
                    CustomerAltConNumber="5556667778",
                    CustomerEmail = "rahul@gmail.com",
                    CustomerAddressType = "Home",
                    CustomerAddress = "F-451 Okhala Phase-1",
                    CustomerCountry = "India",
                    CustomerState = "Delhi",
                    CustomerCity = "New Delhi",
                    CustomerPincode = "110020",
                    DeviceCategory = "Mobiles",
                    DeviceSubCategory = "Smart Phones",
                    DeviceBrand = "Samsung",
                    DeviceModel = "Samsung A5-16",
                    DeviceModelNo="SMG000000",
                    DeviceIMEIOne = "2456675644433",
                    DeviceIMEISecond = "8756454444445",
                    DeviceSn = "#45444",
                    DOP = "10/12/2019",
                    PurchaseFrom = "Delhi",
                    DeviceCondition = "Brand New",
                    ProblemDescription="No Cooling",
                    IssueOcurringSinceDate="25/05/2019"

                }
                };

                filecontent = ExcelExportHelper.ExportExcel(devices, "", false, columns);

            }



            else

            {               
                
                    columns = new string[]{"CustomerType","CustomerName","CustomerContactNumber","CustomerAltConNumber","CustomerEmail","CustomerAddressType",
"CustomerAddress","CustomerCountry","CustomerState","CustomerCity","CustomerPincode","DeviceCategory","DeviceSubCategory","DeviceBrand","DeviceModel","DeviceModelNo","DeviceSn",
                    "DOP","PurchaseFrom","DeviceIMEIOne","DeviceIMEISecond","DeviceCondition"};
                    var devices = new List<UploadedExcelModel> { new UploadedExcelModel
                {
                    CustomerType = "CORPORATE",
                    CustomerName = "Rahul Singh",
                    CustomerContactNumber = "9993344444",
                    CustomerAltConNumber="5556667778",
                    CustomerEmail = "rahul@gmail.com",
                    CustomerAddressType = "Home",
                    CustomerAddress = "F-451 Okhala Phase-1",
                    CustomerCountry = "India",
                    CustomerState = "Delhi",
                    CustomerCity = "New Delhi",
                    CustomerPincode = "110020",
                    DeviceCategory = "Mobiles",
                    DeviceSubCategory = "Smart Phones",
                    DeviceBrand = "Samsung",                    
                    DeviceModel = "Samsung A5-16",
                    DeviceModelNo="SMG000000",                    
                    DeviceSn = "#45444",
                    DOP = "10/12/2019",
                    PurchaseFrom = "Delhi",
                    DeviceIMEIOne = "2456675644433",
                    DeviceIMEISecond = "8756454444445",
                    DeviceCondition = "Brand New"


                }
                };

                    filecontent = ExcelExportHelper.ExportExcel(devices, "", false, columns);
                }
            
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> Edit(string Crn)
        {
            var SessionModel = Session["User"] as SessionModel;
            var CallDetailsModel = await _centerRepo.GetCallsDetailsById(Crn);
            CallDetailsModel.IsAssingedCall = true;
            CallDetailsModel.ClientList = new SelectList(await CommonModel.GetClientData(SessionModel.CompanyId), "Name", "Text");
            CallDetailsModel.CategoryList = new SelectList(_dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.BrandList = new SelectList(_dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.SubCategoryList = new SelectList(_dropdown.BindSubCategory(CallDetailsModel.DeviceCategoryId), "Value", "Text");
            CallDetailsModel.ProductList = new SelectList(_dropdown.BindProduct(CallDetailsModel.DeviceBrandId.ToString()+","+ CallDetailsModel.DeviceSubCategoryId.ToString()), "Value", "Text");
            CallDetailsModel.StatusList = new SelectList(await CommonModel.GetStatusTypes("Client"), "Value", "Text");
            CallDetailsModel.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CallDetailsModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CallDetailsModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address"), "Value", "Text");
            CallDetailsModel.LocationList = new SelectList(_dropdown.BindLocationByPinCode(CallDetailsModel.PinNumber), "Value", "Text");
            CallDetailsModel.IsClientAddedBy = true;
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

        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> GetRepeatCall(FilterModel filter )
        {
            var previousCall = await _RepoCallLog.GetPreviousCall(filter);
            if(previousCall ==null)
                return Json("Not Found", JsonRequestBehavior.AllowGet);
            else
            return Json(previousCall, JsonRequestBehavior.AllowGet);
        }

        public  ActionResult OpenedCallsList()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId,Type='O' };
            if (session.UserTypeName.ToLower().Contains("client"))
                filter.ClientId = session.RefKey;
            var calls = _RepoCallLog.GetClientCalls(filter);
            return PartialView ("_OpenedCallsList", calls);

        }
        public ActionResult ClosedCallsList()
        {

            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, Type = 'C' };
            if (session.UserTypeName.ToLower().Contains("client"))
                filter.ClientId = session.RefKey;
            var calls = _RepoCallLog.GetClientCalls(filter);
            return PartialView("_ClosedCallsList", calls);
        }
        public ActionResult UploadedDataList()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, Type = 'D' };
            if (session.UserTypeName.ToLower().Contains("client"))
                filter.ClientId = session.RefKey;
            var calls = _RepoCallLog.GetClientCalls(filter);
            return PartialView("_UploadedDataList", calls);

        }

        public ActionResult FileDataList()
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, Type = 'F' };
            if (session.UserTypeName.ToLower().Contains("client"))
                filter.ClientId = session.RefKey;
            var calls = _RepoCallLog.GetFileList(filter);
            return PartialView("_FileDataList", calls);

        }

        public async Task<ActionResult> GetCallHistory (Guid DeviceId)
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, RefKey=DeviceId };
            var his = await _RepoCallLog.GetCallHistory(filter);
            return PartialView("_OrderHistory", his);
        }
    }
}