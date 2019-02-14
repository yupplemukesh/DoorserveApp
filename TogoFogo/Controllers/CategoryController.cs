﻿using System;
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
    public class CategoryController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        // GET: Category
        public ActionResult DeviceCategory()
        {
            if (TempData["AddCategory"] != null)
            {
                ViewBag.AddCategory = TempData["AddCategory"].ToString();
            }

            if (TempData["EditCategory"] != null)
            {
                ViewBag.EditCategory = TempData["EditCategory"].ToString();
            }

            return View();
        }

        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(DeviceCategoryModel model)
        {
            try
            {
                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_Delete_Category",
                        new
                        {
                            CatId = "",
                            model.CatName,
                            model.IsRepair,
                            model.IsActive,
                            model.Comments,
                            model.CreatedBy,
                            Action = "add",
                            model.SortOrder,
                            model.ModifyBy,
                            model.DeleteBy 

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["AddCategory"] = "Category Already Exist";

                    }
                    else
                    {
                        TempData["AddCategory"] = "Successfully Added";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("DeviceCategory");
        }

        public ActionResult DeviceCategoryTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<DeviceCategoryModel>("Select * from MstCategory ORDER BY CASE WHEN SortOrder > 0 THEN 1 else  2  END,SortOrder asc", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }

        [HttpPost]
        public ActionResult DeviceCategoryTable(int CatId)
        {
            return View();
        }

        public ActionResult EditDeviceCategory(int CatId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<DeviceCategoryModel>("Select * from MstCategory where CatId=@CatId", new { CatId = CatId },
                    commandType: CommandType.Text).FirstOrDefault();
                return PartialView("EditDeviceCategory", result);
            }
        }

        [HttpPost]
        public ActionResult EditDeviceCategory(DeviceCategoryModel model)
        {
            try
            {
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_Delete_Category",
                        new
                        {
                            model.CatId,
                            model.CatName,
                            model.IsRepair,
                            model.IsActive,
                            model.Comments,
                            model.CreatedBy,
                            Action = "edit",
                            model.SortOrder,
                            model.ModifyBy,
                            model.DeleteBy
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["EditCategory"] = "Successfully Updated";

                    }

                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("DeviceCategory");
        }


        //Manage Sub Category

        public ActionResult DeviceSubCategory()
        {
            ViewBag.DeviceCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddSubCategory"] != null)
            {
                ViewBag.AddSubCategory = TempData["AddSubCategory"].ToString();
            }
            if (TempData["EditSubCategory"] != null)
            {
                ViewBag.EditSubCategory = TempData["EditSubCategory"].ToString();
            }

            return View();
        }

        public ActionResult AddSubCategory()
        {
            ViewBag.DeviceCategory = new SelectList(dropdown.BindCategory(), "Value", "Text");
            //using (var con = new SqlConnection(_connectionString))
            //{

            //    ViewBag.DeviceCategory = new SelectList(dropdown.BindCategory(), "Value", "Text");
            //    var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstSubCategory", null, commandType: CommandType.Text).FirstOrDefault();

            //    ViewBag.SortOrder = result + 1;
            //    return View();

            //}
            return View();
        }
        [HttpPost]
        public ActionResult AddSubCategory(SubcategoryModel model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_Delete_SubCategory",
                        new
                        {
                            model.CatId,
                            model.SubCatName,
                            model.SortOrder,
                            model.SubCatId,
                            model.IsRequiredIMEI1,
                            model.IsRequiredIMEI2,
                            model.IsRequiredSerialNo,
                            model.SRNOLength,
                            model.IsRepair,
                            model.IsActive,
                            model.Comments,
                            model.IMEILength,
                            model.CreatedBy,
                            Action = "add",
                            model.ModifyBy,
                            model.DeleteBy,
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["AddSubCategory"] = "Sub Category Already Exist";

                    }
                    else
                    {
                        TempData["AddSubCategory"] = "Successfully Added";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("DeviceSubCategory");
        }

        public ActionResult DeviceSubCategoryTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SubcategoryModel>("GetSubCategoryDetails ", new { }, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            };
        }
        public ActionResult EditDeviceSubCategory(int SubCatId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SubcategoryModel>("select * from MstSubCategory where SubCatId=@SubCatId", new { SubCatId = SubCatId },
                    commandType: CommandType.Text).FirstOrDefault();
                ViewBag.DeviceCategory = new SelectList(dropdown.BindCategory(), "Value", "Text");
                ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                if (result != null)
                {
                    result.DeviceCategory = result.CatId.ToString();

                }
                return PartialView("EditDeviceSubCategory", result);
            }
        }
        [HttpPost]
        public ActionResult EditDeviceSubCategory(SubcategoryModel model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_Delete_SubCategory",
                        new
                        {
                            model.CatId,
                            model.SubCatName,
                            model.SortOrder,
                            model.SubCatId,
                            model.IsRequiredIMEI1,
                            model.IsRequiredIMEI2,
                            model.IsRequiredSerialNo,
                            model.SRNOLength,
                            model.IsRepair,
                            model.IsActive,
                            model.Comments,
                            model.IMEILength,
                            model.CreatedBy,
                            Action = "edit",
                            model.ModifyBy,
                            model.DeleteBy
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["EditSubCategory"] = "Updated Successfully";

                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("DeviceSubCategory");
        }
    }
}