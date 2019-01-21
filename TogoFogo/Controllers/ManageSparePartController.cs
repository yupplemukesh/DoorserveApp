using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ManageSparePartController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                string path = Server.MapPath("~/Uploaded Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        public ActionResult SpareIndex()
        {
            ViewBag.SubCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddSparePart"] != null)
            {
                ViewBag.AddSparePart = TempData["AddSparePart"].ToString();
            }

            if (TempData["EditSparePart"] != null)
            {
                ViewBag.EditSparePart = TempData["EditSparePart"].ToString();
            }
            

            return View();
        }
        public ActionResult AddSpareType()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstSpareType", null, commandType: CommandType.Text).FirstOrDefault();
                ViewBag.SortOrder = result + 1;
            }
            ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

            return View();
        }
        [HttpPost]
        public ActionResult AddSpareType(ManageSpareType model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_SpareType",
                        new
                        {
                            SpareTypeId = "",
                            model.SpareTypeName,
                            CategoryId = model.Category,
                            SubCategoryid = model.SubCategory,
                            model.SortOrder,
                            model.IsActive,
                            CreatedBy = "",
                            ModifyBy = "",
                            DeleteBy = "",
                            Action = "add"

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["AddSparePart"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["AddSparePart"] = "Spare type Name Already Exist";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("SpareIndex");
        }
        public ActionResult SpareTypeTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageSpareType>("GetSpareTypeDetail", new { }, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }

        public ActionResult EditSpareType(int SpareTypeId)
        {
            if (SpareTypeId == 0)
            {
                ViewBag.SubCategory = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<ManageSpareType>("Select * from MstSpareType where SpareTypeId=@SpareTypeId", new { SpareTypeId = SpareTypeId },
                        commandType: CommandType.Text).FirstOrDefault();
                    ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
                    ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                    //testing
                    //ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(result.CategoryId), "Value", "Text");
                    if (result != null)
                    {
                        result.Category = result.CategoryId.ToString();
                        result.SubCategory = result.SubCategoryId.ToString();

                    }
                    return PartialView("EditSpareType", result);
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult EditSpareType(ManageSpareType model)
        {
            try
            {

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_SpareType",
                        new
                        {
                            model.SpareTypeId,
                            model.SpareTypeName,
                            CategoryId = model.Category,
                            SubCategoryid = model.SubCategory,
                            model.SortOrder,
                            model.IsActive,
                            CreatedBy = "",
                            ModifyBy = "",
                            DeleteBy = "",
                            Action = "edit"

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["EditSparePart"] = "Updated Successfully";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("SpareIndex");
        }

        public ActionResult ManageSparePartName()
        {
            ViewBag.SubCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Brand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.DeviceModelName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CTH_No = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddSpareName"] != null)
            {
                ViewBag.AddSpareName = TempData["AddSpareName"].ToString();
            }

            if (TempData["EditSpareName"] != null)
            {
                ViewBag.EditSpareName = TempData["EditSpareName"].ToString();
            }
            return View();
        }
        public ActionResult AddSparePartName()
        {
            ViewBag.CTH_No = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstSparePart", null,
                    commandType: CommandType.Text).FirstOrDefault();

                ViewBag.SortOrder = result + 1;
            }

            //ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
            ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.Brand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            ViewBag.DeviceModelName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SpareTypeName = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            return View();
        }

        [HttpPost]
        public ActionResult AddSparePartName(ManageSparePart model)
        {
            try
            {
                if (model.PartImage1 != null)
                {
                    model.Part_Image = SaveImageFile(model.PartImage1);

                }
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_SparePart",
                        new
                        {
                            PartId = "",
                            SpareTypeId = model.SpareTypeName,
                            BrandId=model.Brand,
                            CategoryId = model.Category,
                            SpareCode= model.SpareCode,
                            ProductId = model.DeviceModelName,
                            model.SubCategory,
                            model.CTH_No,
                            model.Part_Image,
                            model.PartName,
                            model.SortOrder,
                            model.IsActive,
                            CreatedBy = "",
                            ModifyBy = "",
                            DeleteBy = "",
                            Action = "add",
                            TGFG_Code="",
                            model.TUPC

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {

                        TempData["AddSpareName"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["AddSpareName"] = "Spare Part Name Already Exist";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageSparePartName");
        }

        public ActionResult AddSparePartNametable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageSparePart>("GetSparePartDetails", new { }, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        public ActionResult EditSpareName(int? SpareTypeId)
        {
            if (SpareTypeId == 0)
            {
                ViewBag.SubCategory = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Brand = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.DeviceModelName = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                ViewBag.CTH_No = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<ManageSparePart>("Select * from MstSparePart where SpareTypeId=@SpareTypeId", new { SpareTypeId = SpareTypeId },
                        commandType: CommandType.Text).FirstOrDefault();
                    ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                    ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
                    ViewBag.Brand = new SelectList(dropdown.BindBrand(), "Value", "Text");
                    ViewBag.DeviceModelName = new SelectList(dropdown.BindProduct(), "Value", "Text");
                    ViewBag.SpareTypeName = new SelectList(dropdown.BindSpareType(), "Value", "Text");
                    if (result != null)
                    {
                        result.Brand = result.BrandId.ToString();
                        result.Category = result.CategoryId.ToString();
                        result.DeviceModelName = result.ProductId.ToString();
                    }
                    return PartialView("EditSpareName", result);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult EditSpareName(ManageSparePart model)
        {
            try
            {
                if (model.PartImage1 != null)
                {
                    model.Part_Image = SaveImageFile(model.PartImage1);

                }
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_SparePart",
                        new
                        {
                           model.PartId,
                            model.SpareTypeId,
                            BrandId = model.Brand,
                            CategoryId = model.Category,
                            model.SpareCode,
                            ProductId = model.DeviceModelName,
                            model.SubCategory,
                            model.CTH_No,
                            model.Part_Image,
                            model.PartName,
                            model.SortOrder,
                            model.IsActive,
                            CreatedBy = "",
                            ModifyBy = "",
                            DeleteBy = "",
                            Action = "edit",
                            TGFG_Code = "",
                            model.TUPC

                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {

                        TempData["EditSpareName"] = "Successfully Updated";
                    }
                    else
                    {
                        TempData["EditSpareName"] = "Spare Part Name Already Exist";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }
            return RedirectToAction("ManageSparePartName");
        }
    }
}