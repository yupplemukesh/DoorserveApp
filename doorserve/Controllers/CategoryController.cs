using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Permission;

namespace doorserve.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
 
        // GET: Category
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Device_Category)]
        public ActionResult DeviceCategory()
        {
            CategoryViewModel Cs = new CategoryViewModel();         

            if (TempData["AddCategory"] != null)
            {
                Cs.AddCategory = TempData["AddCategory"].ToString();
            }

            if (TempData["EditCategory"] != null)
            {
                Cs.EditCategory = TempData["EditCategory"].ToString();
            }

            return View(Cs);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Device_Category)]
        public ActionResult AddCategory()
        {

            return PartialView (new DeviceCategoryModel());
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Device_Category)]
        [HttpPost]
        [ValidateModel]
        public ActionResult AddCategory(DeviceCategoryModel model)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("Add_Modify_Delete_Category",
                    new
                    {
                        CatId = "",
                        model.CatName,
                        model.IsRepair,
                        model.IsActive,
                        model.SortOrder,
                        model.Comments,
                        User = CurrentUser.UserId,
                        Action = "add",
                        companyId = CurrentUser.CompanyId

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
                    response.Response = "Category Already Exist";
                    TempData["response"] = response;

                }


            }        
            return RedirectToAction("DeviceCategory");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Device_Category)]
        public ActionResult DeviceCategoryTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<DeviceCategoryModel>("USPGetCategoryList", new {companyId= CurrentUser.CompanyId}, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }            
        }
 
     
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Device_Category)]
        public ActionResult EditDeviceCategory(int CatId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<DeviceCategoryModel>("Select * from MstCategory where CatId=@CatId", new { CatId = CatId },
                    commandType: CommandType.Text).FirstOrDefault();
                return PartialView("EditDeviceCategory", result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Device_Category)]
        [HttpPost]
        [ValidateModel]
        public ActionResult EditDeviceCategory(DeviceCategoryModel model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                  
                    var result = con.Query<int>("Add_Modify_Delete_Category",
                        new
                        {
                            model.CatId,
                            model.CatName,
                            model.IsRepair,
                            model.IsActive,
                            model.SortOrder,
                            model.Comments,                            
                            User = CurrentUser.UserId,
                            Action = "edit", 
                            companyId= CurrentUser.CompanyId                           
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Category Successfully Updated";
                        TempData["response"] = response;                       

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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Device_Sub_Category)]
        public ActionResult DeviceSubCategory()
        {
            ViewBag.DeviceCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            CategoryViewModel Cse = new CategoryViewModel();
            if (TempData["AddSubCategory"] != null)
            {
                Cse.AddSubCategory = TempData["AddSubCategory"].ToString();
            }
            if (TempData["EditSubCategory"] != null)
            {
                Cse.EditSubCategory = TempData["EditSubCategory"].ToString();
            }

            return View(Cse);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Device_Sub_Category)]
        public ActionResult AddSubCategory()
        {
            SubcategoryModel sm = new SubcategoryModel();
            sm.CategoryList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
            
            return View(sm);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, (int)MenuCode.Device_Sub_Category)]
        [HttpPost]
        [ValidateModel]
        public ActionResult AddSubCategory(SubcategoryModel model)
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
                            User = CurrentUser.UserId,
                            Action = "add",
                            CurrentUser.CompanyId
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result != 0)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Added";
                        TempData["response"] = response;
                        

                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Response = "Sub Category Already Exist";
                        TempData["response"] = response;
                        
                    }
                }          
                return RedirectToAction("DeviceSubCategory");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Device_Sub_Category)]
        public ActionResult DeviceSubCategoryTable()
        {
            SubcategoryModel objSubcategoryModel = new SubcategoryModel();
            using (var con = new SqlConnection(_connectionString))
            {
                var result1 = con.Query<SubcategoryModel>("GetSubCategoryDetails ", new {companyId= CurrentUser.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
                return View(result1);
            };   

          
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Device_Sub_Category)]
        public ActionResult EditDeviceSubCategory(int SubCatId)
        {
            SubcategoryModel sm = new SubcategoryModel();

            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SubcategoryModel>("select CatId,SubCatId,SubCatName,SortOrder,IsRequiredIMEI1,IsRequiredIMEI2,IsRequiredSerialNo,Comments,SRNOLength,IsActive,IsRepair,Sr_no_req,IMEILength from MstSubCategory where SubCatId=@SubCatId", new { SubCatId },
                    commandType: CommandType.Text).FirstOrDefault();
                result.CategoryList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
             
                if (result != null)
                {
                    //result.DeviceCategory = result.CatId.ToString();
                    result.CatId = result.CatId;
                }
                return PartialView("EditDeviceSubCategory", result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, (int)MenuCode.Device_Sub_Category)]
        [HttpPost]        
        [ValidateModel]
        public ActionResult EditDeviceSubCategory(SubcategoryModel model)
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
                            User = CurrentUser.UserId,
                            Action = "edit",
                            CurrentUser.CompanyId
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Updated Successfully";
                        TempData["response"] = response;                      

                    }
                }
                      return RedirectToAction("DeviceSubCategory");
        }
    }
}