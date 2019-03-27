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
using TogoFogo.Permission;

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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Spare Type")]
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
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, "Manage Spare Type")]
        public ActionResult AddSpareType()
        {
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstSpareType", null, commandType: CommandType.Text).FirstOrDefault();
            //    ViewBag.SortOrder = result + 1;
            //}
            var sparetype = new ManageSpareType();
            sparetype.CategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            sparetype.SubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

            return PartialView(sparetype);
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
                            User = Convert.ToInt32(Session["User_Id"]),
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 1)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Added";
                  
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Response = "Spare type Name Already Exist";
               
                    }
                    TempData["response"] = response;
                    TempData.Keep("response");
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("SpareIndex");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Spare Type")]
        public ActionResult SpareTypeTable()
        {
          
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageSpareType>("GetSpareTypeDetail", new { }, commandType: CommandType.StoredProcedure).ToList();

               return View(result);
            }          
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Spare Type")]
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
                            User = Convert.ToInt32(Session["User_Id"]),
                            Action = "edit"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Updated Successfully";

                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Response = "Allready Exits";

                    }
                    TempData["response"] = response;
                    TempData.Keep("response");
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("SpareIndex");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Spare Part Name")]
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
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Spare Part Name")]
        public ActionResult AddSparePartName()
        {
            var sparepart = new ManageSparePart();
            sparepart.CTHNoList = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstSparePart", null,
            //        commandType: CommandType.Text).FirstOrDefault();

            //    ViewBag.SortOrder = result + 1;
            //}

            //ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
            sparepart.CategoryList = new SelectList(dropdown.BindCategory(), "Value", "Text");
            sparepart.BrandList = new SelectList(dropdown.BindBrand(), "Value", "Text");
            sparepart.DeviceModelNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            sparepart.SpareTypeNameList = new SelectList(dropdown.BindSpareType(), "Value", "Text");
            return PartialView(sparepart);
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
                    var response = new ResponseModel();
                    if (result == 1)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Added";
                        TempData["response"] = Response;
                       
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Response = "Spare Part Name Already Exist";
                        TempData["response"] = Response;                      
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("ManageSparePartName");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Spare Part Name")]
        public ActionResult AddSparePartNametable()
        {         
           
            using (var con = new SqlConnection(_connectionString))
            {
             var result= con.Query<ManageSparePart>("GetSparePartDetails", new { }, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }          
                     
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Spare Part Name")]
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
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Updated";
                                          
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Response = "Spare Part Name Already Exist";
                     
                    }
                    TempData["response"] = response;
                }

            }
            catch (Exception e)
            {

                throw e;
            }
            return RedirectToAction("ManageSparePartName");
        }
    }
}