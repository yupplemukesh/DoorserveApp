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
using Dapper;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Permission;
using doorserve.Repository;
using doorserve.Repository.ImportFiles;

namespace doorserve.Controllers
{
    public class ManageCityLocationController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private readonly DropdownBindController dropdown;
        private readonly IUploadFiles _RepoUploadFile;

        public ManageCityLocationController()
        {
            _RepoUploadFile = new UploadFiles();
            dropdown= new DropdownBindController();

        }
        // GET: ManageCityLocation
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Cities_Locations)]
        public ActionResult ManageCityLocation()
        {
            LocationViewModel ls = new LocationViewModel();
         

            if (TempData["AddLocation"] != null)
            {
                ls.AddLocation = TempData["AddLocation"].ToString();
            }
            if (TempData["EditLocation"] != null)
            {
                ls.EditLocation = TempData["EditLocation"].ToString();
            }
            return View(ls);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Cities_Locations)]
        public ActionResult AddCityLocation()
        {
            ManageLocation ml = new ManageLocation();
            ml._CountryList = new SelectList(Enumerable.Empty<SelectListItem>());
            ml._StateList = new SelectList(Enumerable.Empty<SelectListItem>());
          
            using (var con = new SqlConnection(_connectionString))
            {
                ml._CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
               
                return View(ml);

            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Cities_Locations)]
        [HttpPost]
        [ValidateModel]
        public ActionResult AddCityLocation(ManageLocation model)
        {
            try
            {              
                using (var con = new SqlConnection(_connectionString))
                {
                    var session = Session["User"] as SessionModel;
                    var result = con.Query<int>("Add_Edit_Delete_Location",
                        new
                        {
                            LocationId = "",
                            model.LocationName,
                            model.StateId,
                            model.CountryId,
                            model.DistrictName,
                            model.PinCode,
                            model.IsActive,
                            model.Comments,
                            User = session.UserId,
                            Action ="add"
                           
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 1)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Added";
                        TempData["response"] = response;
                       
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Response = "Location Name Already Exist";
                        TempData["response"] = response;
                   
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageCityLocation");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Cities_Locations)]
        public ActionResult ManageCityTable()
        {
            ManageLocation objManageLocation = new ManageLocation();
            ViewBag.PageNumber = (Request.QueryString["grid-page"] == null) ? "1" : Request.QueryString["grid-page"];

            using (var con = new SqlConnection(_connectionString))
            {
              var result = con.Query<ManageLocation>("GetLocationDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
                // objManageLocation.ManageLocationList
                return View(result);
            }            
            //return View(objManageLocation);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Cities_Locations)]
        public ActionResult EditCityLocation(int ? LocationId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
               // ManageLocation ml = new ManageLocation();
                var result = con.Query<ManageLocation>("Select * from MstLocation where LocationId=@LocationId", new { LocationId = LocationId },
                    commandType: CommandType.Text).FirstOrDefault();
                result._CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                result._StateList = new SelectList(dropdown.BindState(), "Value", "Text");
               
                /*if (result != null)
                {
                    result.CountryName =Convert.ToString(result.CountryId);
                    result.StateName = result.StateId.ToString();

                }*/
                return PartialView("EditCityLocation", result);
            }

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Cities_Locations)]
        [HttpPost]
        [ValidateModel]
        public ActionResult EditCityLocation(ManageLocation model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var session = Session["User"] as SessionModel;
                    var result = con.Query<int>("Add_Edit_Delete_Location",
                        new
                        {
                            model.LocationId,
                            model.LocationName,
                            model.StateId,
                            model.CountryId,
                            model.DistrictName,
                            model.PinCode,
                            model.IsActive,
                            model.Comments,
                            User = session.UserId,
                            Action = "edit"

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Updated";
                        TempData["response"] = response;
                       
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageCityLocation");
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
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }

        [HttpPost]
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Cities_Locations)]
        [ValidateModel]
        public async Task<ActionResult> Import(ProviderFileModel provider)
        {
            var SessionModel = Session["User"] as SessionModel;
            provider.CompanyId = SessionModel.CompanyId;
            provider.UserId = SessionModel.UserId;

            if (provider.DataFile != null)
            {
                string FileName = SaveFile(provider.DataFile, "Locations");
                var excelPath = Server.MapPath("~/Files/Locations/");
                string conString = string.Empty;
                string extension = Path.GetExtension(provider.DataFile.FileName);
                switch (extension)
                {
                    case ".xls": //Excel 97-03
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 or higher
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;

                }

                conString = string.Format(conString, excelPath+FileName);
                DataTable dtExcelData = new DataTable();
                using (OleDbConnection excel_con = new OleDbConnection(conString))
                {
                    excel_con.Open();

                    string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                    dtExcelData.Columns.AddRange(new DataColumn[6] {
                new DataColumn("Country Name", typeof(string)),
                new DataColumn("State Name", typeof(string)),
                new DataColumn("District Name", typeof(string)),
                new DataColumn("Pin Code", typeof(string)),
                new DataColumn("Location Name", typeof(string)),
                   new DataColumn("Is Active", typeof(string))
                });
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT [Country Name],[State Name], [District Name],[Pin Code],[Location Name] ,[IS Active] FROM [" + sheet1 + "] where [Country Name] is not null", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();

                }
                try
                {
                    provider.FileName=FileName;

                    var response = await _RepoUploadFile.UploadCityLocations(provider, dtExcelData);
                    if (!response.IsSuccess)
                        System.IO.File.Delete(excelPath);
                    TempData["response"] = response;
                    return RedirectToAction("ManageCityLocation");
                }
                catch (Exception ex)

                {
                    if (System.IO.File.Exists(excelPath))
                        System.IO.File.Delete(excelPath);
                    return RedirectToAction("ManageCityLocation");

                }
            }
            return RedirectToAction("index");

        }




        [PermissionBasedAuthorize(new Actions[] { Actions.ExcelExport }, (int)MenuCode.Manage_Cities_Locations)]
        public async Task<FileContentResult> ExportToExcel(char tabIndex)
        {
            var session = Session["User"] as SessionModel;
            var filter = new FilterModel { CompId = session.CompanyId, tabIndex = tabIndex };
            byte[] filecontent;
            if (tabIndex == 'T')
            {
                string[] columns = new string[] { "CountryName", "StateName", "DistrictName", "PinCode", "LocationName", "IsActive" };
                var providerData = new List<ManageLocation> { new ManageLocation
                {
                CountryName="India",
                StateName="Uttar Pradesh",
                DistrictName="Mathura",
                PinCode=281204,
                LocationName="Raya",
                IsActive=true
                }};           
                filecontent = ExcelExportHelper.ExportExcel(providerData, "", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "LocationTemplate.xlsx");
            }
            else
            {
                string[] columns = new string[] { "CountryName", "StateName", "DistrictName", "PinCode", "LocationName", "IsActive" };
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<ManageLocation>("GetLocationDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
                    // objManageLocation.ManageLocationList
                    filecontent = ExcelExportHelper.ExportExcel(result, "", false, columns);
                    return File(filecontent, ExcelExportHelper.ExcelContentType, "Location.xlsx");
                }
            }
        }
    }
}