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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Cities_Locations)]
        public ActionResult ManageCityLocation()
        {
            LocationViewModel ls = new LocationViewModel();
            ls.Rights  = (UserActionRights)HttpContext.Items["ActionsRights"];

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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Cities_Locations)]
        [HttpPost]
        public ActionResult AddCityLocation(ManageLocation model)
        {
            try
            {              
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
                            User = Convert.ToInt32(Session["User_Id"]),
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

            using (var con = new SqlConnection(_connectionString))
            {
              var result = con.Query<ManageLocation>("GetLocationDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
                // objManageLocation.ManageLocationList
                return View(result);
            }            
            //return View(objManageLocation);

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Cities_Locations)]
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
                    result.CountryName =Convert.ToString(result.CountryId);
                    result.StateName = result.StateId.ToString();

                }
                return PartialView("EditCityLocation", result);
            }

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Cities_Locations)]
        [HttpPost]
        public ActionResult EditCityLocation(ManageLocation model)
        {
            try
            {
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
                            User = Convert.ToInt32(Session["User_Id"]),
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
    }
}