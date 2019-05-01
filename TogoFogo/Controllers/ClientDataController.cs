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
        private readonly DropdownBindController _dropdown;
        private SessionModel user;
        public ClientDataController()
        {
            _RepoUploadFile = new UploadFiles();
            _dropdown = new DropdownBindController();
            _RepoCallLog = new CallLog();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Assign Calls")]
        public async Task<ActionResult> Index()
        {
            user = Session["User"] as SessionModel;
            ViewBag.PageNumber = (Request.QueryString["grid-page"] == null) ? "1" : Request.QueryString["grid-page"];
            bool IsClient = false;
            var filter = new FilterModel { CompId=user.CompanyId };
            if (user.UserRole.ToLower().Contains("client"))
            {
                filter.ClientId = user.RefKey;
                IsClient = true;
            }
              var clientData = await _RepoUploadFile.GetUploadedList(filter);
            clientData.Client = new ClientDataModel();
            clientData.Client.IsClient = IsClient;
            clientData.Client.ClientList = new SelectList(await CommonModel.GetClientData(user.CompanyId), "Name", "Text");
            clientData.Client.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(user.CompanyId), "Value", "Text");
            clientData.Client.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(user.CompanyId), "Value", "Text");

            // new call Log
            clientData.NewCallLog = new UploadedExcelModel
            {
                ClientList = clientData.Client.ClientList,
                ServiceTypeList = clientData.Client.ServiceTypeList,
                DeliveryTypeList = clientData.Client.DeliveryTypeList,
                BrandList = new SelectList(_dropdown.BindBrand(user.CompanyId), "Value", "Text"),
                CategoryList=new SelectList(_dropdown.BindCategory(user.CompanyId),"Value","Text"),
                ProductList=  new SelectList(Enumerable.Empty<SelectListItem>()),
                CustomerTypeList=new SelectList( await CommonModel.GetLookup("Customer Type"),"Value","Text" ),
                ConditionList=new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text"),
                IsClient=IsClient,
               // address=new AddressDetail
                //{
                    AddressTypelist= new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text"),
                    CityList = new SelectList(Enumerable.Empty<SelectListItem>()),
                    StateList= new SelectList(Enumerable.Empty<SelectListItem>()),
                    CountryList = new SelectList(_dropdown.BindCountry(),"Value","Text"),
               // }

        };
            return View(clientData);
        }

        public async Task<ActionResult> GetAssignedCalls()
        {
            var filter = new FilterModel { CompId = user.CompanyId };
            if (user.UserRole.ToLower().Contains("client"))
                filter.ClientId = user.RefKey;
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
                return path+"\\"+savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Assign Calls")]
        public async Task<ActionResult> Create()
        {
            user = Session["User"] as SessionModel;
            var clientDate = new ClientDataModel();
            clientDate.ClientList = new SelectList(await CommonModel.GetClientData(user.CompanyId), "Name", "Text");
            clientDate.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(user.CompanyId), "Value", "Text");
            return View(clientDate);
        }
        [HttpPost]
        public async Task<ActionResult> Upload(ClientDataModel clientDataModel)
        {

            user = Session["User"] as SessionModel;
            clientDataModel.CompanyId = user.CompanyId;
            clientDataModel.UserId = user.UserId;
            if (clientDataModel.IsClient)
                clientDataModel.ClientId =user.RefKey;        
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
                    if(!response.IsSuccess)
                        System.IO.File.Delete(excelPath);
                    TempData["response"] = response;
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    if(System.IO.File.Exists(Server.MapPath(excelPath)))
                       System.IO.File.Delete(Server.MapPath(excelPath));
                      return RedirectToAction("index");

                }
            }
            return RedirectToAction("index");

        }

        [HttpPost]
        public async Task<ActionResult> NewCallLog(UploadedExcelModel uploads)
        {
            user = Session["User"] as SessionModel;
            uploads.UserId = user.UserId;
            uploads.CompanyId = user.CompanyId;
            var response = await _RepoCallLog.NewCallLog(uploads);
            TempData["response"] = response;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {
            var user = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = user.CompanyId,tabIndex=tabIndex };
            if (user.UserRole.ToLower().Contains("Client"))
                filter.ClientId = user.RefKey;
            var response = await _RepoUploadFile.GetExportAssingedCalls(filter);
            byte[] filecontent; 
            string[] columns;
            if (tabIndex == 'O')
            {
                columns = new string[]{"CRN","CreatedOn","ClientName","CustomerName","CustomerContactNumber","CustomerEmail",
                "ServiceTypeName","DeliveryTypeName"};
                filecontent = ExcelExportHelper.ExportExcel(response.Calls.OpenCalls, "", true, columns);
            }
            else if (tabIndex=='C')
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
    }

    }
