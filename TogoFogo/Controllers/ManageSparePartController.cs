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
                string path = Server.MapPath("~/UploadedImages");
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
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstSpareType", null, commandType: CommandType.Text).FirstOrDefault();
            //    ViewBag.SortOrder = result + 1;
            //}
            ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

            return View();
        }
        [HttpPost]
        public ActionResult AddSpareType(ManageSpareType model)
        {
            try
            {
                //model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                //model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));

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
                            User = Convert.ToInt32(Session["User_Id"]),
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
            ManageSpareType objManageSpareType = new ManageSpareType();
            using (var con = new SqlConnection(_connectionString))
            {
                objManageSpareType._ManageSpareTypeList = con.Query<ManageSpareType>("GetSpareTypeDetail", new { }, commandType: CommandType.StoredProcedure).ToList();

               // return View(result);
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objManageSpareType._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objManageSpareType._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objManageSpareType._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objManageSpareType._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objManageSpareType._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objManageSpareType._UserActionRights.Create = true;
                objManageSpareType._UserActionRights.Edit = true;
                objManageSpareType._UserActionRights.Delete = true;
                objManageSpareType._UserActionRights.View = true;
                objManageSpareType._UserActionRights.History = true;
                objManageSpareType._UserActionRights.ExcelExport = true;

            }
            return View(objManageSpareType);
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
                //model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
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
                            User = Convert.ToInt32(Session["User_Id"]),
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
            ViewBag.CTHNo = new SelectList(Enumerable.Empty<SelectListItem>());
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
            ViewBag.CTHNo = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstSparePart", null,
            //        commandType: CommandType.Text).FirstOrDefault();

            //    ViewBag.SortOrder = result + 1;
            //}

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
                //model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                //model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
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
                            model.CTHNo,
                            model.Part_Image,
                            model.PartName,
                            model.SortOrder,
                            model.IsActive,
                            User = Convert.ToInt32(Session["User_Id"]),
                            Action = "add",
                            TGFGCode="",
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
            ManageSparePart objManageSparePart = new ManageSparePart();
            
            using (var con = new SqlConnection(_connectionString))
            {
                objManageSparePart._ManageSparePartList = con.Query<ManageSparePart>("GetSparePartDetails", new { }, commandType: CommandType.StoredProcedure).ToList();

                
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objManageSparePart._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objManageSparePart._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objManageSparePart._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objManageSparePart._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objManageSparePart._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objManageSparePart._UserActionRights.Create = true;
                objManageSparePart._UserActionRights.Edit = true;
                objManageSparePart._UserActionRights.Delete = true;
                objManageSparePart._UserActionRights.View = true;
                objManageSparePart._UserActionRights.History = true;
                objManageSparePart._UserActionRights.ExcelExport = true;

            }
            return View(objManageSparePart);
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
                ViewBag.CTHNo = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
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
                //model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
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
                            model.CTHNo,
                            model.Part_Image,
                            model.PartName,
                            model.SortOrder,
                            model.IsActive,
                            User = Convert.ToInt32(Session["User_Id"]),
                            Action = "edit",
                            TGFGCode = "",
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