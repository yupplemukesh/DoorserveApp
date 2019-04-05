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
        private readonly DropdownBindController _dropdown;
        public ClientDataController()
        {
            _RepoUploadFile = new UploadFiles();
            _dropdown = new DropdownBindController();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Import Customers")]
        public async Task<ActionResult> Index()
        {
            var clientData = new MainClientDataModel();
            Guid? clientId = null;
            if (Session["RoleName"].ToString().ToLower().Contains("client"))
                clientId = await CommonModel.GetClientIdByUser(Convert.ToInt32(Session["User_ID"]));
                clientData.UploadedData = await _RepoUploadFile.GetUploadedList(clientId);
                clientData.UploadedFiles = new List<FileDetailModel>();
                 
            clientData.client = new ClientDataModel();
            clientData.client.ClientList = new SelectList(await CommonModel.GetClientData(), "Name", "Text");
            clientData.client.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(), "Value", "Text");
            clientData.client.DeliveryTypeList = new SelectList(await CommonModel.GetDeliveryServiceType(), "Value", "Text");

            // new call Log
            clientData.NewCallLog = new UploadedExcelModel
            {
                ClientList = clientData.client.ClientList,
                ServiceTypeList = clientData.client.ServiceTypeList,
                DeliveryTypeList = clientData.client.DeliveryTypeList,
                BrandList = new SelectList(_dropdown.BindBrand(), "Value", "Text"),
                CategoryList=new SelectList(_dropdown.BindCategory(),"Value","Text"),
                ProductList=  new SelectList(Enumerable.Empty<SelectListItem>()),
                CustomerTypeList=new SelectList( await CommonModel.GetLookup("Customer Type"),"Value","Text" ),
                ConditionList=new SelectList(await CommonModel.GetLookup("Device Condition"), "Value", "Text"),
                address=new AddressDetail
                {
                    AddressTypelist= new SelectList(await CommonModel.GetLookup("ADDRESS"), "Value", "Text"),
                    CityList = new SelectList(Enumerable.Empty<SelectListItem>()),
                    StateList= new SelectList(Enumerable.Empty<SelectListItem>()),
                    CountryList = new SelectList(_dropdown.BindCountry(),"Value","Text"),
                }

        };





            return View(clientData);
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Import Customers")]
        public async Task<ActionResult> Create()
        {
            var clientDate = new ClientDataModel();
            clientDate.ClientList = new SelectList(await CommonModel.GetClientData(), "Name", "Text");
            clientDate.ServiceTypeList = new SelectList(await CommonModel.GetServiceType(), "Value", "Text");
            return View(clientDate);
        }
        [HttpPost]
        public async Task<ActionResult> Upload(ClientDataModel clientDataModel)
        {
            if (Session["RoleName"].ToString().ToLower().Contains("client"))
                clientDataModel.ClientId = await CommonModel.GetClientIdByUser(Convert.ToInt32(Session["User_ID"]));

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
                    dtExcelData.Columns.AddRange(new DataColumn[18] {
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
                new DataColumn("DEVICE PURCHASE FROM", typeof(string))
                });

                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [CUSTOMER NAME],[CUSTOMER  CONTACT],[CUSTOMER E-MAIL]," +
                        "[CUSTOMER ADDRESS TYPE],[CUSTOMER ADDRESS],[CUSTOMER COUNTRY],[CUSTOMER STATE],[CUSTOMER CITY]," +
                        "[CUSTOMER PINCODE],[DEVICE CATEGORY],[DEVICE BRAND],[DEVICE NAME],[DEVICE MODEL],[DEVICE IMEI FIRST]," +
                        "[DEVICE IMEI SECOND],[DEVICE SLN],[DEVICE DOP],[DEVICE PURCHASE FROM]  FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                }
                try
                {
                    clientDataModel.UserId = Convert.ToInt32(Session["User_ID"]);
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


       


        }

    }
