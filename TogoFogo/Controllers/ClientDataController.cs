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
        private readonly DropdownBindController _dropdown;
        
        public ClientDataController()
        {
            _RepoUploadFile = new UploadFiles();
            _dropdown = new DropdownBindController();
            _RepoCallLog = new CallLog();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> Index()
        {
            ViewBag.PageNumber = (Request.QueryString["grid-page"] == null) ? "1" : Request.QueryString["grid-page"];
            bool IsClient = false;
            var filter = new FilterModel { CompId = SessionModel.CompanyId };
            if (SessionModel.UserRole.ToLower().Contains("client"))
            {
                filter.ClientId = SessionModel.RefKey;
                IsClient = true;
            }
            var clientData = _RepoUploadFile.GetUploadedList(filter);

            var serviceType = await CommonModel.GetServiceType(SessionModel.CompanyId);
            var deliveryType = await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId);
            clientData.Client = new ClientDataModel();
            clientData.Client.IsClient = IsClient;
            clientData.Client.ClientList = new SelectList(await CommonModel.GetClientData(SessionModel.CompanyId), "Name", "Text");
            clientData.Client.ServiceTypeList = new SelectList(serviceType, "Value", "Text");
            clientData.Client.DeliveryTypeList = new SelectList(deliveryType, "Value", "Text");

            // new call Log
            clientData.NewCallLog = new UploadedExcelModel
            {
                ClientList = clientData.Client.ClientList,
                ServiceTypeList = clientData.Client.ServiceTypeList,
                DeliveryTypeList = clientData.Client.DeliveryTypeList,
                BrandList = new SelectList(_dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text"),
                CategoryList = new SelectList(_dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text"),
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
            var filter = new FilterModel { CompId = SessionModel.CompanyId };
            if (SessionModel.UserRole.ToLower().Contains("client"))
                filter.ClientId = SessionModel.RefKey;
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

            var clientDate = new ClientDataModel();
            clientDate.ClientList = new SelectList(await CommonModel.GetClientData(SessionModel.CompanyId), "Name", "Text");
            clientDate.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            return View(clientDate);
        }
        [HttpPost]
        public async Task<ActionResult> Upload(ClientDataModel clientDataModel)
        {


            clientDataModel.CompanyId = SessionModel.CompanyId;
            clientDataModel.UserId = SessionModel.UserId;
            if (clientDataModel.IsClient)
                clientDataModel.ClientId = SessionModel.RefKey;
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
                new DataColumn("CUSTOMER  CONTACT", typeof(string)),
                new DataColumn("CUSTOMER E-MAIL", typeof(string)),
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
                new DataColumn("DEVICE SLN", typeof(string)),
                new DataColumn("DEVICE DOP", typeof(DateTime)),
                new DataColumn("DEVICE PURCHASE FROM", typeof(string)),
                new DataColumn("DEVICE CONDITION", typeof(string))
                });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [CUSTOMER TYPE], [CUSTOMER NAME],[CUSTOMER  CONTACT],[CUSTOMER E-MAIL]," +
                        "[CUSTOMER ADDRESS TYPE],[CUSTOMER ADDRESS],[CUSTOMER COUNTRY],[CUSTOMER STATE],[CUSTOMER CITY]," +
                        "[CUSTOMER PINCODE],[DEVICE CATEGORY],[DEVICE BRAND],[DEVICE NAME],[DEVICE MODEL],[DEVICE IMEI FIRST]," +
                        "[DEVICE IMEI SECOND],[DEVICE SLN],[DEVICE DOP],[DEVICE PURCHASE FROM],[DEVICE CONDITION]  FROM [" + sheet1 + "]", excel_con))
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

        [HttpPost]
        public async Task<ActionResult> NewCallLog(UploadedExcelModel uploads)
        {

            uploads.UserId = SessionModel.UserId;
            uploads.CompanyId = SessionModel.CompanyId;
            if (SessionModel.UserRole.ToLower().Contains("client"))
                uploads.ClientId = SessionModel.RefKey;
            var response = await _RepoCallLog.NewCallLog(uploads);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {

            var filter = new FilterModel { CompId = SessionModel.CompanyId, tabIndex = tabIndex };
            if (SessionModel.UserRole.ToLower().Contains("Client"))
                filter.ClientId = SessionModel.RefKey;
            var response = await _RepoUploadFile.GetExportAssingedCalls(filter);
            byte[] filecontent;
            string[] columns;
            if (tabIndex == 'O')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail",
                "ServiceTypeName","DeliveryTypeName"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.OpenCalls, "", true, columns);
            }
            else if (tabIndex == 'C')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail",
                "ServiceTypeName","DeliveryTypeName"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.CloseCalls, "", true, columns);
            }
            else if (tabIndex == 'D')
            {
                columns = new string[]{"UploadedDate","CRN","ClientName","CustomerName","CustomerContactNumber","CustomerEmail",
                "ServiceTypeName","DeliveryTypeName","DeviceCategory"};
                filecontent = ExcelExportHelper.ExportExcel(response.UploadedData, "", true, columns);
            }
            else
            {
                {
                    columns = new string[]{"UploadedDate","UserName","UploadedFilename","ServiceType","ServiceDeliveryType",
                    "TotalRecords","UploadedRecords","FailedRecords"};
                    filecontent = ExcelExportHelper.ExportExcel(response.UploadedFiles, "", true, columns);
                }
            }
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Excel.xlsx");

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Assign_Calls)]
        public async Task<ActionResult> Edit(string Crn)
        {
            var CallDetailsModel = await _centerRepo.GetCallsDetailsById(Crn);
            CallDetailsModel.BrandList = new SelectList(_dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CategoryList = new SelectList(_dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.ProductList = new SelectList(_dropdown.BindProduct(CallDetailsModel.DeviceBrandId), "Value", "Text");
            CallDetailsModel.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(SessionModel.CompanyId), "Value", "Text");
            CallDetailsModel.CustomerTypeList = new SelectList(await CommonModel.GetLookup("Customer Type"), "Value", "Text");
            CallDetailsModel.ConditionList = new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text");
            CallDetailsModel.AddressTypelist = new SelectList(await CommonModel.GetLookup("Address Type list"), "Value", "Text");
            CallDetailsModel.CountryList = new SelectList(_dropdown.BindCountry(), "Value", "Text");
            CallDetailsModel.StateList = new SelectList(_dropdown.BindState(CallDetailsModel.CountryId), "Value", "Text");
            CallDetailsModel.CityList = new SelectList(_dropdown.BindLocation(CallDetailsModel.StateId), "Value", "Text");    
          
            return PartialView("_NewCallLogForm", CallDetailsModel);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(CallStatusDetailsModel callStatusDetails)
        {
            try
            {

                callStatusDetails.UserId = SessionModel.UserId;
                var response = await _centerRepo.UpdateCallsStatusDetails(callStatusDetails);
                TempData["response"] = response;
                //return Json("Ok", JsonRequestBehavior.AllowGet);
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
