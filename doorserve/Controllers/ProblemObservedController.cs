using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Models;
using System.Data;
using doorserve.Permission;

namespace doorserve.Controllers
{
    public class ProblemObservedController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Problem_Observed)]
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Problem_Observed)]
        public ActionResult AddProblemObserved()
        {
      
            var problemobserved = new ManageProblemObserved();
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
                problemobserved.CategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
                problemobserved.SubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

                //var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstProblemObserved", null, commandType: CommandType.Text).FirstOrDefault();
                //ViewBag.SortOrder = result + 1;
                return PartialView(problemobserved);

            }

        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Problem_Observed)]
        [HttpPost]
        public ActionResult AddProblemObserved(ManageProblemObserved model)
        {          
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
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
                            User = SessionModel.UserId,
                            SessionModel.CompanyId,
                            Action ="add"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
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
                        response.Response = "Problem Code Already Exist";
                        TempData["response"] = response;
                     }
                }
                return RedirectToAction("ManageProblemObserved");
           }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Problem_Observed)]
        public ActionResult ProblemObservedtable()
        {
          
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
                var result = con.Query<ManageProblemObserved>("GetProbObsrvDetails", new { SessionModel.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
            
            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Problem_Observed)]
        public ActionResult EditProblemObserved(int ProblemId)
        {
            var SessionModel = Session["User"] as SessionModel;


            ViewBag.Device_Category = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Problem_Observed)]
        [HttpPost]
        public ActionResult EditProblemObserved(ManageProblemObserved model)
        {           
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
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
                            User = SessionModel.UserId,
                            SessionModel.CompanyId,
                            Action = "edit"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Updated Successfully";
                        TempData["response"] = response;                        
                    }

                }


                return RedirectToAction("ManageProblemObserved");

            }
        }

    }
}