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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Spare_Type)]
        public ActionResult SpareIndex()
        {
            ManageSpareType mst = new ManageSpareType();
            mst.CategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            mst.SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, (int)MenuCode.Manage_Spare_Type)]
        public ActionResult AddSpareType()
        {
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Query<int>("select coalesce(MAX(SortOrder),0) from MstSpareType", null, commandType: CommandType.Text).FirstOrDefault();
            //    ViewBag.SortOrder = result + 1;
            //}

            var sparetype = new ManageSpareType {
                CategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text"),
                SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>())

        };
            
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
                            model.CategoryId,
                            model.SubCategoryId,
                            model.SortOrder,
                            model.IsActive,
                            User = SessionModel.UserId,
                            Action = "add",
                            SessionModel.CompanyId
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
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("SpareIndex");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Spare_Type)]
        public ActionResult SpareTypeTable()
        {
          
            using (var con = new SqlConnection(_connectionString))
            {

                var result = con.Query<ManageSpareType>("GetSpareTypeDetail", new {companyId= SessionModel.CompanyId }, commandType: CommandType.StoredProcedure).ToList();

               return View(result);
            }          
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Spare_Type)]
        public ActionResult EditSpareType(int SpareTypeId)
        {
            ManageSpareType mst = new ManageSpareType();
            if (SpareTypeId == 0)
            {
                mst.SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
                mst.CategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                using (var con = new SqlConnection(_connectionString))
                {
        
                    var result = con.Query<ManageSpareType>("Select * from MstSpareType where SpareTypeId=@SpareTypeId", new { SpareTypeId = SpareTypeId },
                        commandType: CommandType.Text).FirstOrDefault();
                    result.CategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
                    result.SubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
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
                            model.CategoryId,
                            model.SubCategoryId,
                            model.SortOrder,
                            model.IsActive,
                            User = SessionModel.UserId,
                            Action = "edit",
                            SessionModel.CompanyId
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Spare_Part_Name)]
        public ActionResult ManageSparePartName()
        {
            ManageSparePart msp = new ManageSparePart
            {
                SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CategoryList = new SelectList(Enumerable.Empty<SelectListItem>()),
                BrandList = new SelectList(Enumerable.Empty<SelectListItem>()),
                DeviceModelNameList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CTHNoList = new SelectList(Enumerable.Empty<SelectListItem>())

        };
           
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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Spare_Part_Name)]
        public ActionResult AddSparePartName()
        {
            var sparepart = new ManageSparePart();
            sparepart.CTHNoList = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
            sparepart.CategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
            sparepart.BrandList = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            sparepart.DeviceModelNameList = new SelectList(dropdown.BindProduct(sparepart.BrandId), "Value", "Text");
            sparepart.SpareTypeIdList = new SelectList(dropdown.BindSpareType(SessionModel.CompanyId), "Value", "Text");
            sparepart.SubCategoryList = new SelectList(dropdown.BindSubCategory(sparepart.CategoryId), "Value", "Text");
                    
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
                            model.PartId,
                            model.CategoryId,
                            model.SubCategory,
                            model.SpareTypeId,
                            model.BrandId,
                            model.PartName,
                            model.ProductId,
                            model.TUPC,
                            model.TGFGCode,
                            model.SpareCode,
                            model.CTHNo,
                            model.Part_Image,
                            model.IsActive,
                            model.SortOrder,
                            User = SessionModel.UserId,
                            SessionModel.CompanyId,
                            Action = "add"                       
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
                        response.IsSuccess = false;
                        response.Response = "Spare Part Name Already Exist";
                        TempData["response"] = response;                      
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("ManageSparePartName");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Spare_Part_Name)]
        public ActionResult AddSparePartNametable()
        {         
           
            using (var con = new SqlConnection(_connectionString))
            {

                var result= con.Query<ManageSparePart>("GetSparePartDetails", new { SessionModel.CompanyId}, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }          
                     
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Spare_Part_Name)]
        public ActionResult EditSpareName(int? SpareTypeId)
        {
            ManageSparePart msp = new ManageSparePart();
            if (SpareTypeId == 0)
            {
                msp.SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
                msp.CategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
                msp.BrandList = new SelectList(Enumerable.Empty<SelectListItem>());
                msp.DeviceModelNameList = new SelectList(Enumerable.Empty<SelectListItem>());
                msp.SpareTypeIdList= new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                ViewBag.CTHNo = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
                using (var con = new SqlConnection(_connectionString))
                {
                
                    var result = con.Query<ManageSparePart>("Select * from MstSparePart where SpareTypeId=@SpareTypeId", new { SpareTypeId = SpareTypeId },
                        commandType: CommandType.Text).FirstOrDefault();
                    result.SubCategoryList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                    result.CategoryList = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
                    result.BrandList = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
                    result.DeviceModelNameList = new SelectList(dropdown.BindProduct(SessionModel.CompanyId), "Value", "Text");
                    result.SpareTypeIdList = new SelectList(dropdown.BindSpareType(SessionModel.CompanyId), "Value", "Text");
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
                            model.BrandId,
                            model.CategoryId,
                            model.SpareCode,
                            model.ProductId,
                            model.SubCategory,
                            model.CTHNo,
                            model.Part_Image,
                            model.PartName,
                            model.SortOrder,
                            model.IsActive,
                            User = SessionModel.UserId,
                            Action = "edit",
                            TGFGCode = "",
                            model.TUPC,
                            SessionModel.CompanyId

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