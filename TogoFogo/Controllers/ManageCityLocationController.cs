using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class ManageCityLocationController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: ManageCityLocation
        public ActionResult ManageCityLocation()
        {
            ViewBag.CountryName = new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.StateName = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddLocation"] != null)
            {
                ViewBag.AddLocation = TempData["AddLocation"].ToString();
            }
            if (TempData["EditLocation"] != null)
            {
                ViewBag.AddLocation = TempData["EditLocation"].ToString();
            }
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Cities/Locations")]
        public ActionResult AddCityLocation()
        {
            ViewBag.CountryName = new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.StateName = new SelectList(Enumerable.Empty<SelectListItem>());
            using (var con = new SqlConnection(_connectionString))
            {
                ViewBag.CountryName = new SelectList(dropdown.BindCountry(), "Value", "Text");
                //var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstDeviceProblem", null, commandType: CommandType.Text).FirstOrDefault();

                //ViewBag.SortOrder = result + 1;
                return View();

            }
        }
        [HttpPost]
        public ActionResult AddCityLocation(ManageLocation model)
        {
            try
            {
                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_Location",
                        new
                        {
                            LocationId="",
                            model.LocationName,
                            StateId=model.StateName,
                            CountryId=model.CountryName,
                            model.IsActive,
                            model.Comments,
                            model.CreatedBy,
                            model.ModifyBy,
                            model.DeleteBy,
                            Action="add"
                           
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["AddLocation"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["AddLocation"] = "Location Name Already Exist";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageCityLocation");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Cities/Locations")]
        public ActionResult ManageCityTable()
        {
            ManageLocation objManageLocation = new ManageLocation();

            using (var con = new SqlConnection(_connectionString))
            {
                objManageLocation.ManageLocationList = con.Query<ManageLocation>("GetLocationDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
               
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objManageLocation._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objManageLocation._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objManageLocation._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objManageLocation._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objManageLocation._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objManageLocation._UserActionRights.Create = true;
                objManageLocation._UserActionRights.Edit = true;
                objManageLocation._UserActionRights.Delete = true;
                objManageLocation._UserActionRights.View = true;
                objManageLocation._UserActionRights.History = true;
                objManageLocation._UserActionRights.ExcelExport = true;

            }

            return View(objManageLocation);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Cities/Locations")]
        public ActionResult EditCityLocation(int LocationId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageLocation>("Select * from MstLocation where LocationId=@LocationId", new { LocationId = LocationId },
                    commandType: CommandType.Text).FirstOrDefault();
                ViewBag.CountryName = new SelectList(dropdown.BindCountry(), "Value", "Text");
                ViewBag.StateName = new SelectList(dropdown.BindState(), "Value", "Text");
                if (result != null)
                {
                    result.CountryName = result.CountryId.ToString();
                    result.StateName = result.StateId.ToString();

                }
                return PartialView("EditCityLocation", result);
            }

        }
        [HttpPost]
        public ActionResult EditCityLocation(ManageLocation model)
        {
            try
            {
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_Location",
                        new
                        {
                            model.LocationId,
                            model.LocationName,
                            StateId = model.StateName,
                            CountryId = model.CountryName,
                            model.IsActive,
                            model.Comments,
                            model.CreatedBy,
                            model.ModifyBy,
                            model.DeleteBy,
                            Action = "edit"

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["EditLocation"] = "Successfully Updated";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageCityLocation");
        }
    }
}