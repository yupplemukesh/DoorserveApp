using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using System.Data;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class ProblemObservedController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Problem Observed")]
        public ActionResult ManageProblemObserved()
        {
            ViewBag.SubCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Device_Category = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["ProblemObserved"] != null)
            {
                ViewBag.ProblemObserved = TempData["ProblemObserved"].ToString();
            }
            //if (TempData["EditProblemObserved"] != null)
            //{
            //    ViewBag.EditProblemObserved = TempData["EditProblemObserved"].ToString();
            //}
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Problem Observed")]
        public ActionResult AddProblemObserved()
        {
            var problemobserved = new ManageProblemObserved();
            using (var con = new SqlConnection(_connectionString))
            {
                problemobserved.CategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
                problemobserved.SubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

                //var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstProblemObserved", null, commandType: CommandType.Text).FirstOrDefault();
                //ViewBag.SortOrder = result + 1;
                return PartialView(problemobserved);

            }

        }
        [HttpPost]
        public ActionResult AddProblemObserved(ManageProblemObserved model)
        {
            //model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ?"0" : Convert.ToString(Session["User_ID"]));
           // model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.ProblemObserved == null)
                {

                }
                else
                {
                    var result = con.Query<int>("Add_Edit_ProblemObserved", 
                        new {
                            ProblemId="",
                           DeviceCategory=model.Device_Category,
                           model.SubCategory,
                           model.ProblemCode,
                           model.ProblemObserved,
                           model.IsActive,                            
                           model.SortOrder,
                            User = Convert.ToInt32(Session["User_Id"]),
                            Action ="add"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["ProblemObserved"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["ProblemObserved"] = "Problem Code Already Exist";
                    }
                }
                return RedirectToAction("ManageProblemObserved");
           }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Problem Observed")]
        public ActionResult ProblemObservedtable()
        {
            ManageProblemObserved objManageProblemObserved = new ManageProblemObserved();
            using (var con = new SqlConnection(_connectionString))
            {
                objManageProblemObserved.ManageProblemObservedList = con.Query<ManageProblemObserved>("GetProbObsrvDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
            
            }
            objManageProblemObserved._UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(objManageProblemObserved);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Problem Observed")]
        public ActionResult EditProblemObserved(int ProblemId)
        {
            ViewBag.Device_Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
            
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageProblemObserved>("Select * from MstProblemObserved where ProblemId=@ProblemId", new { ProblemId=ProblemId }, commandType: CommandType.Text).FirstOrDefault();
                ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(result.DeviceCategory), "Value", "Text");
                if (result != null)
                {
                    result.Device_Category = result.DeviceCategory.ToString();
                    //result.Category = result.Category_ID.ToString();

                }
                return View(result);
            }
           
        }
        [HttpPost]
        public ActionResult EditProblemObserved(ManageProblemObserved model)
        {
            //model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.ProblemId == null)
                {
                    TempData["ProblemObserved"] = "Problem Id Not Found";
                }
                else
                {
                    var result = con.Query<int>("Add_Edit_ProblemObserved",
                        new
                        {
                            model.ProblemId,
                            DeviceCategory=model.Device_Category,
                            model.SubCategory,
                            model.ProblemCode,
                            model.ProblemObserved,
                            model.IsActive,
                            model.SortOrder,
                            User = Convert.ToInt32(Session["User_Id"]),
                            Action = "edit"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["ProblemObserved"] = "Updated Successfully";
                    }

                }


                return RedirectToAction("ManageProblemObserved");

            }
        }

    }
}