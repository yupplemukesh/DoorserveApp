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

namespace TogoFogo.Controllers
{
    public class ProblemObservedController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
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
            return View();
        }

        public ActionResult AddProblemObserved()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                ViewBag.Device_Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
                ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

                var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstProblemObserved", null, commandType: CommandType.Text).FirstOrDefault();
                ViewBag.SortOrder = result + 1;
                return View();

            }

        }

        [HttpPost]
        public ActionResult AddProblemObserved(ManageProblemObserved model)
        {
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
                           model.CreatedBy,
                            model.ModifyBy,
                            model.DeleteBy,
                            Action="add"
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

        public ActionResult ProblemObservedtable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageProblemObserved>("GetProbObsrvDetails", new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
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
            return View();
        }
        [HttpPost]
        public ActionResult EditProblemObserved(ManageProblemObserved model)
        {
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
                            model.CreatedBy,
                            model.ModifyBy,
                            model.DeleteBy,
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