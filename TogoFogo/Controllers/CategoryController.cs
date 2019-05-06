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
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        private SessionModel user;
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
        [HttpPost]
        public ActionResult AddCategory(DeviceCategoryModel model)
        {
            try
            {

                user = Session["User"] as SessionModel;
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
                            User = user.UserId,
                            Action = "add",
                            companyId=user.CompanyId

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

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("DeviceCategory");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Device_Category)]
        public ActionResult DeviceCategoryTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                user = Session["User"] as SessionModel;
                var result = con.Query<DeviceCategoryModel>("USPGetCategoryList", new {companyId=user.CompanyId}, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }            
        }
        [HttpPost]
        public ActionResult DeviceCategoryTable(int CatId)
        {
            return View();
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
        [HttpPost]
        public ActionResult EditDeviceCategory(DeviceCategoryModel model)
        {
            try
            {
                //model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                using (var con = new SqlConnection(_connectionString))
                {

                    user = Session["User"] as SessionModel;
                    var result = con.Query<int>("Add_Modify_Delete_Category",
                        new
                        {
                            model.CatId,
                            model.CatName,
                            model.IsRepair,
                            model.IsActive,
                            model.SortOrder,
                            model.Comments,                            
                            User = user.UserId,
                            Action = "edit", 
                            companyId=user.CompanyId                           
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
            user = Session["User"] as SessionModel;
            ViewBag.DeviceCategory = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
            
            return View();
        }
        [HttpPost]
        public ActionResult AddSubCategory(SubcategoryModel model)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    user = Session["User"] as SessionModel;
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
                            User = user.UserId,
                            Action = "add",
                           user.CompanyId
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

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("DeviceSubCategory");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Device_Sub_Category)]
        public ActionResult DeviceSubCategoryTable()
        {
            SubcategoryModel objSubcategoryModel = new SubcategoryModel();
            using (var con = new SqlConnection(_connectionString))
            {
                user = Session["User"] as SessionModel;
                var result1 = con.Query<SubcategoryModel>("GetSubCategoryDetails ", new {companyId=user.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
                return View(result1);
            };   

          
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Device_Sub_Category)]
        public ActionResult EditDeviceSubCategory(int SubCatId)
        {

            using (var con = new SqlConnection(_connectionString))
            {
                user = Session["User"] as SessionModel;
                var result = con.Query<SubcategoryModel>("select CatId,SubCatId,SubCatName,SortOrder,IsRequiredIMEI1,IsRequiredIMEI2,IsRequiredSerialNo,Comments,SRNOLength,IsActive,IsRepair,Sr_no_req from MstSubCategory where SubCatId=@SubCatId", new { SubCatId },
                    commandType: CommandType.Text).FirstOrDefault();
                ViewBag.DeviceCategory = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
             
                if (result != null)
                {
                    //result.DeviceCategory = result.CatId.ToString();
                    result.CatId = result.CatId;
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
                    user = Session["User"] as SessionModel;
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
                            User = user.UserId,
                            Action = "edit",
                      user.CompanyId
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Updated Successfully";
                        TempData["response"] = response;                      

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