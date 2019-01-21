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

namespace TogoFogo.Controllers
{
    public class GstCategoryController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        // GET: GstCategory
        public ActionResult Gst()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                
            }
            return View();
        }
        public ActionResult AddGst()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGst(GstCategoryModel model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_GstCategory",
                        new
                        {
                            model.GstCategoryId,
                            model.GSTCategory,
                            model.IsActive,
                            model.Comments,
                            User="",
                            
                            ACTION = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["Message"] = "Gst Category Already Exist";

                    }
                    else
                    {
                        TempData["Message"] = "Successfully Added TRC";
                    }
                }

            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Gst");
        }

        public ActionResult GstTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<GstCategoryModel>("Select * from MstGstCategory", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }

        public ActionResult EditGst(int? gstCategoryId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<GstCategoryModel>("Select * from MstGstCategory Where GstCategoryId=@GstCategoryId",
                    new { @GstCategoryId = gstCategoryId }, commandType: CommandType.Text).FirstOrDefault();
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult EditGst(GstCategoryModel model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_GstCategory",
                        new
                        {
                            model.GstCategoryId,
                            model.GSTCategory,
                            model.IsActive,
                            model.Comments,
                            User = "",

                            ACTION = "edit"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["Message"] = "Updated Successfully";

                    }
                    else
                    {
                        TempData["Message"] = "Something Went Wrong";
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Gst");
        }
    }
}