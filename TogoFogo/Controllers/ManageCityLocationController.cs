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
            ManageLocation ml = new ManageLocation();
            ml._CountryList = new SelectList(Enumerable.Empty<SelectListItem>());
            ml._StateList = new SelectList(Enumerable.Empty<SelectListItem>());
          
            using (var con = new SqlConnection(_connectionString))
            {
                ml._CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
               
                //var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstDeviceProblem", null, commandType: CommandType.Text).FirstOrDefault();

                //ViewBag.SortOrder = result + 1;
                return View(ml);

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
    }
}