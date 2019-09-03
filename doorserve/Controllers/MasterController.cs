using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Dapper;
using doorserve.Models;
using doorserve.Extension;
using doorserve.SaveImageCode;
using System.Collections.Generic;
using System.Data.OleDb;
using doorserve.Permission;

namespace doorserve.Controllers
{
    //[CustomAuthorize]
    public class MasterController : BaseController
    {
        #region ConnectionString

        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();

        #endregion
        #region BRAND   
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Brands)]
        public ActionResult Brand()
        {
            var name = User.Identity.Name;      
            if (TempData["SubmitBrand"] != null)
            {
                ViewBag.SubmitBrand = TempData["SubmitBrand"].ToString();
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
       
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Brands)]
        public ActionResult AddBrand()
        {
            return PartialView();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Brands)]
        [HttpPost]
        public ActionResult AddBrand(BrandModel model)
        {
            
            try
            {
                if (model.BrandIMG != null)
                {
                    var mpc = new SaveImage();
                    Type type = mpc.GetType();

                    model.BrandImage = (string)type.InvokeMember("SaveImageFile",
                                            BindingFlags.Instance | BindingFlags.InvokeMethod |
                                            BindingFlags.NonPublic, null, mpc,
                                            new object[] { model.BrandIMG });
                    //model.BrandImage = SaveImageFile(model.BrandIMG);

                }             

                using (var con = new SqlConnection(_connectionString))
                {


                    var result = con.Query<int>("Add_Modify_Delete_Brand",
                        new
                        {
                            BrandId = "",
                            model.BrandName,
                            model.BrandImage,
                            model.BrandDescription,
                            model.MetaKeyword,
                            model.MetaDescription,
                            model.MetaTitle,
                            model.UrlName,
                            model.Header,
                            model.Footer,
                            model.IsRepair,
                            model.IsActive,
                            model.Comments,                           
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
                        response.Response = "Brand Name Already Exist";
                        TempData["response"] = response;                       
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("Brand");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Brands)]
        public ActionResult EditBrand(int brandId=0)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                 var result = con.Query<BrandModel>("Get_Single_Brand", new { BrandId = brandId },
                     commandType: CommandType.StoredProcedure).FirstOrDefault();                
                return PartialView("EditBrand", result);
            }
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Brands)]
        [HttpPost]
        public ActionResult EditBrand(BrandModel model)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.BrandIMG != null)
                {
                  
                    var mpc = new SaveImage();
                    Type type = mpc.GetType();

                    model.BrandImage = (string)type.InvokeMember("SaveImageFile",
                                            BindingFlags.Instance | BindingFlags.InvokeMethod |
                                            BindingFlags.NonPublic, null, mpc,
                                            new object[] { model.BrandIMG });
                }
              
                var result = con.Query<int>("Add_Modify_Delete_Brand"
                    , new
                    {
                        model.BrandId,
                        model.BrandName,
                        model.BrandImage,
                        model.BrandDescription,
                        model.IsRepair,
                        model.IsActive,
                        model.Comments,
                        model.MetaKeyword,
                        model.MetaDescription,
                        model.MetaTitle,
                        model.UrlName,
                        model.Header,
                        model.Footer,
                        User = CurrentUser.UserId,
                        Action = "edit",
                        CurrentUser.CompanyId
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                var response = new ResponseModel();
                if (result == 2)
                {
                    response.IsSuccess = true;
                    response.Response = "Updated Successfully";
                    TempData["response"] = response;
                   
                }

                return RedirectToAction("Brand", "Master");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Brands)]
        public ActionResult BrandTable()
        {
            BrandModel objBrandModel = new BrandModel();
            using (var con = new SqlConnection(_connectionString))
            {
               
                var result= con.Query<BrandModel>("Get_Brands", new { companyId= CurrentUser.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
           }      
         
        }
        #endregion
        #region PRODUCT
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Products)]
        public ActionResult Product()
        {
            ProductModel pm = new ProductModel();
            pm._BrandName = new SelectList(Enumerable.Empty<SelectListItem>());
            pm._Category = new SelectList(Enumerable.Empty<SelectListItem>());
            pm._SubCat = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["AddProduct"] != null)
            {
                ViewBag.AddProduct = TempData["AddProduct"].ToString();
            }
            if (TempData["EditProduct"] != null)
            {
                ViewBag.EditProduct = TempData["EditProduct"].ToString();
            }


        
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Products)]
        public ActionResult AddProduct()
        {
            using (var con = new SqlConnection(_connectionString))
            {               
                ProductModel pm = new ProductModel {
                    _BrandName= new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text"),
                    _Category= new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text"),
                    _ProductColor= new SelectList(dropdown.BindProductColor(null), "Value", "Text"),
                    _SubCat= new SelectList(Enumerable.Empty<SelectList>())

                };               

                return PartialView(pm);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Products)]
        [HttpPost]
        public ActionResult AddProduct(ProductModel model)
        {
            try
            {
                if (model.ProductImg != null)
                {                   
                    var mpc = new SaveImage();
                    Type type = mpc.GetType();
                    model.ProductImage = (string)type.InvokeMember("SaveImageFile",
                                            BindingFlags.Instance | BindingFlags.InvokeMethod |
                                            BindingFlags.NonPublic, null, mpc,
                                            new object[] { model.ProductImg });
                }          
                var finalValue = "";
                if (model.ProductColor != null)
                {
                    finalValue = string.Join(",", model.ProductColor);
                }

                using (var con = new SqlConnection(_connectionString))
                {                    
                    var result = con.Query<int>("Add_Edit_Delete_Products",
                        new
                        {
                            model.ProductId,
                            model.CategoryID,
                            Brand_ID = model.BrandID,
                            //SubCatId = model.SubCategoryId,
                            model.SubCatId,
                            model.ProductName,
                            model.AlternateProductName,
                            model.MRP,
                            model.MarketPrice,
                            model.TUPC,
                            model.ProductImage,
                            model.IsRepair,
                            model.IsActive,
                            model.Comments,
                            ProductColor = finalValue,
                            User = CurrentUser.UserId,                                                        
                            Action = "add",
                            CurrentUser.CompanyId
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result >0)
                    {
                        var problem1 = model.ProductColor.Length;
                        for (var i = 0; i <= problem1 - 1; i++)
                        {
                            var Data = model.ProductColor[i];
                            var result2 = con.Query<int>("Insert_Into_Single_Color_Record",
                              new
                              {
                                  ModelId = result,
                                  ColorId = Data,
                                  Action = "add",
                                  BrandId = model.BrandID,
                              }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        }

                        response.IsSuccess = true;
                        response.Response = "Successfully Added";
                        TempData["response"] = response;
                       
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Response = "Product Name Already Exist";
                        TempData["response"] = response;
                       
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return RedirectToAction("Product");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Products)]
        public ActionResult ProductTable()
        {            
            using (var con = new SqlConnection(_connectionString))
            {               
                var result = con.Query<ProductModel>("GetProductDetail", new { CurrentUser.CompanyId },
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

           

            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Products)]
        public ActionResult EditProduct(int? ProductId)
        {            
            ProductModel pm = new ProductModel();
            if (ProductId == 0 || ProductId == null)
            {
                pm._BrandName = new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");
                pm._Category = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
                pm._SubCat = new SelectList(Enumerable.Empty<SelectListItem>());
                pm._ProductColor = new SelectList(dropdown.BindProductColor(null), "Value", "Text");
            }
            else
            {
                using (var con = new SqlConnection(_connectionString))
                {

                    var result = con.Query<ProductModel>("Get_Single_Product", new { ProductId },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result.Product_Color != null)
                    {
                        result.ProductColor = result.Product_Color.Split(',');
                        //result._ProductColor = new SelectList(dropdown.BindProductColor(SessionModel.CompanyId), "Value", "Text");
                    }

                    if (result != null)
                    {
                        result.CategoryID.ToString();
                        result.SubCatId.ToString();
                        result.BrandID.ToString();
                    }
                  
                    result._BrandName = new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");
                    result._SubCat = new SelectList(dropdown.BindSubCategory(result.CategoryID), "Value", "Text");
                    result._Category = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
                    result._ProductColor = new SelectList(dropdown.BindProductColor(null), "Value", "Text");
                 
                    return PartialView("EditProduct", result);
                }
            }
            return PartialView("EditProduct");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Products)]
        [HttpPost]
        public ActionResult EditProduct(ProductModel model)
        {
            var SessionModel = Session["User"] as SessionModel;
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.ProductImg != null)
                {                  
                    var mpc = new SaveImage();
                    Type type = mpc.GetType();
                    model.ProductImage = (string)type.InvokeMember("SaveImageFile",
                                            BindingFlags.Instance | BindingFlags.InvokeMethod |
                                            BindingFlags.NonPublic, null, mpc,
                                            new object[] { model.ProductImg });
                }
                var finalValue = "";
                if (model.ProductColor != null)
                {
                    finalValue = string.Join(",", model.ProductColor);
                }             
                var result = con.Query<int>("Add_Edit_Delete_Products",
                    new
                    {
                        ProductColor = finalValue,
                        model.ProductId,
                        model.CategoryID,
                        Brand_ID = model.BrandID,                        
                        model.SubCatId,
                        model.ProductName,
                        model.AlternateProductName,
                        model.MRP,
                        model.MarketPrice,
                        model.TUPC,
                        model.ProductImage,
                        model.IsRepair,
                        model.IsActive,
                        model.Comments,
                        User = SessionModel.UserId,
                        Action = "edit",
                        SessionModel.CompanyId
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var response = new ResponseModel();
                if (result == 2)
                {
                    response.IsSuccess = true;
                    response.Response = "Product Updated Successfully";
                    TempData["response"] = response;                    
                }
                else
                {
                    response.IsSuccess = true;
                    response.Response = "Product Not Updated ";
                    TempData["response"] = response;                    
                }
                return RedirectToAction("Product", "Master");
            }
        }
        #endregion
        #region ManageDeviceProblems
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Device_Problem)]
        public ActionResult ManageDeviceProblems()
        {
            DeviceProblemModel dcm = new DeviceProblemModel();
            dcm.CatIdList = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["DeviceProblem"] != null)
            {
                ViewBag.DeviceProblem = TempData["DeviceProblem"].ToString();
            }
           
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Device_Problem)]
        public ActionResult AddDeviceProblem()
        {            
            using (var con = new SqlConnection(_connectionString))
            {

                DeviceProblemModel dcm = new DeviceProblemModel();


                dcm.CatIdList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
                dcm.SubCatIdList = new SelectList(dropdown.BindSubCategory(dcm.CatId), "Value", "Text");
               
                return PartialView(dcm);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Device_Problem)]
        [HttpPost]
        public ActionResult AddDeviceProblem(DeviceProblemModel model)
        {           
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.Problem == null)
                {
                }
                else
                {
                    var result = con.Query<int>("Add_Edit_Problem"
                        , new
                        {
                            model.CatId,
                            model.SubCatId,
                            model.ProblemID,
                            model.IsActive,
                            model.Problem,
                            model.SortOrder,
                            User = CurrentUser.UserId,
                            CurrentUser.CompanyId,
                            Action = "add"
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
                        response.Response = "Device Problem alreay Exit";
                        TempData["response"] = response;                        
                    }
                }
                return RedirectToAction("ManageDeviceProblems");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Device_Problem)]
        public ActionResult DeviceProblemtable()
        {           
            using (var con = new SqlConnection(_connectionString))
            {                
                var result= con.Query<DeviceProblemModel>("GetProblemDetail", new { CurrentUser.CompanyId },
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
            
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Device_Problem)]
        public ActionResult EditDeviceProblem(int? ProblemID)
        {            
            DeviceProblemModel dcm = new DeviceProblemModel();
            dcm.CatIdList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
            dcm.SubCatIdList = new SelectList(dropdown.BindSubCategory(dcm.CatId), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<DeviceProblemModel>("select * from MstDeviceProblem WHERE ProblemID=@ProblemID",
                    new { ProblemID = ProblemID },
                    commandType: CommandType.Text).FirstOrDefault();
                if (result != null)
                {
                    result.Category = result.CatId.ToString();
                    result.SubCategory = result.SubCatId.ToString();
                }
                result.CatIdList = new SelectList(dropdown.BindCategory(CurrentUser.CompanyId), "Value", "Text");
                result.SubCatIdList = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                return View(result);
            }
            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Device_Problem)]
        [HttpPost]
        public ActionResult EditDeviceProblem(DeviceProblemModel model)
        {            
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.ProblemID == null)
                {
                    TempData["DeviceProblem"] = "Problem Id Not Found";
                }
                else
                {
                    var result = con.Query<int>("Add_Edit_Problem"
                        , new
                        {
                            model.CatId,
                            model.SubCatId,
                            model.ProblemID,
                            model.IsActive,
                            model.Problem,
                            model.SortOrder,
                            User = CurrentUser.UserId,
                            CurrentUser.CompanyId,
                            Action = "edit"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result == 2)
                    {
                        response.IsSuccess = true;
                        response.Response = "Successfully Updated";
                        TempData["response"] = response;                       
                    }
                }
                return RedirectToAction("ManageDeviceProblems");

            }
        }
        #endregion
        #region ColorMaster
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Color_Master)]
        public ActionResult ColorMaster()
        {
            ViewBag.Brand = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ModelId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Model = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.pd = new SelectList(Enumerable.Empty<SelectListItem>());

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Color_Master)]
        public ActionResult AddColorMaster()
        {
           
            ViewBag.Brand = new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");
            //ViewBag.Model = new SelectList(dropdown.BindModelName(), "Value", "Text");
            ViewBag.Model = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Color_Master)]
        [HttpPost]
        public ActionResult AddColorMaster(ColorModel m)
        {            
            using (var con = new SqlConnection(_connectionString))
            {
                var result1 = con.Query<int>("Insert_Into_Color_Master",
                           new
                           {
                               m.ColorId,
                               m.ColorName,
                               m.IsActive,
                               m.Comments,
                               User = CurrentUser.UserId,
                               CurrentUser.CompanyId,
                               Action = "add"                               
                           }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var response = new ResponseModel();
                if (result1 == 1)
                {
                    response.IsSuccess = true;
                    response.Response = "Added Successfully";
                    TempData["response"] = response;
                   
                }
                else
                {
                    response.IsSuccess = true;
                    response.Response = "Something went wrong";
                    TempData["response"] = response;                    
                }
            }
            return RedirectToAction("ColorMaster", "Master");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Color_Master)]
        public ActionResult EditColorMaster(int ColorId)
        {
            ViewBag.pd = new SelectList(dropdown.BindModelName(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ColorModel>("select * from color_Master where colorId=@colorId", new { ColorId },
                    commandType: CommandType.Text).FirstOrDefault();
                if (result.ModelId != null)
                {
                    result.pd = result.ModelId.Split(',');
                }
                return PartialView("EditColorMaster", result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit}, (int)MenuCode.Color_Master)]
        [HttpPost]
        public ActionResult EditColorMaster(ColorModel m)
        {        
           using (var con = new SqlConnection(_connectionString))
            {                
                var result1 = con.Query<int>("Insert_Into_Color_Master",
                           new
                           {
                               m.ColorId,
                               m.ColorName,
                               m.IsActive,
                               m.Comments,
                               User = CurrentUser.UserId,
                               CurrentUser.CompanyId,
                               Action = "edit",                            

                           }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                var response = new ResponseModel();
                if (result1 == 2)
                {
                    response.IsSuccess = true;
                    response.Response = "Updated Successfully";
                    TempData["response"] = response;
                   
                }
                else
                {
                    response.IsSuccess = true;
                    response.Response = "Something went wrong";
                    TempData["response"] = response;                   
                }              

            }
            return RedirectToAction("ColorMaster", "Master");           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Color_Master)]
        public ActionResult ColorTable()
        {
            
            using (var con = new SqlConnection(_connectionString))
            {               
                var result = con.Query<ColorModel>("Select cm.ColorId,cm.ColorName,cm.IsActive,cm.Comments,cm.CreatedDate,cm.ModifyDate,cum.UserName 'CBy',cum1.UserName 'MBy' from Color_Master cm left join Create_User_Master cum on cum.Id=cm.CreatedBy left join Create_User_Master cum1 on cum1.Id=cm.ModifyBy", new { CurrentUser.CompanyId, },
                    commandType: CommandType.Text).ToList();
                return View(result);
            }        
           
        }
        #endregion
        #region RemoteValidation

       

        public ActionResult RemoteValidationforUserName(string Username, string CurrentUserName, Int64 UserId = 0)
        {

            // bool ifEmailExist = false;
            try
            {
                if (Username == CurrentUserName)
                    return Json(true, JsonRequestBehavior.AllowGet);

                using (var con = new SqlConnection(_connectionString))
                {
                    var ifEmailExist = con.Query<bool>("UspCheckUserExists",
                    new { Username, UserId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //ifEmailExist = result==0 ? false : true;

                    return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult RemoteValidationConEmailAddress(string ConEmailAddress, string CurrentEmail)
       {
            // return RemoteValidationforUserName(ConEmailAddress, CurrentEmail, 0);
            try
            {
                var cmail = CurrentEmail.Trim();
                if (ConEmailAddress == cmail)
                    return Json(true, JsonRequestBehavior.AllowGet);
                using (var con = new SqlConnection(_connectionString))
                {
                   bool response = true;
                    var ifEmailExist = con.Query<int>("Select count(1) from tblContactPersons WHERE  Email=@ConEmailAddress",
                    new { ConEmailAddress }, commandType: CommandType.Text).FirstOrDefault();
                    //ifEmailExist = result==0 ? false : true;UspCheckEmailExist
                   if (ifEmailExist > 0)
                        response = false;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }




        public ActionResult RemoteValidationClientName(string ClientName, string CurrentClientName)
        {

            // bool ifEmailExist = false;
            try
            {
                if (ClientName == CurrentClientName)
                    return Json(true, JsonRequestBehavior.AllowGet);

                using (var con = new SqlConnection(_connectionString))
                {
                    var ifEmailExist = con.Query<bool>("Select 1 from MSTCLIENTS WHERE  ClientName=@ClientName",
                  new { ClientName }).FirstOrDefault();
                    //ifEmailExist = result==0 ? false : true;

                    return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult RemoteValidationProviderName(string ProviderName, string CurrentProviderName)
        {

            // bool ifEmailExist = false;
            try
            {
                if (ProviderName == CurrentProviderName)
                    return Json(true, JsonRequestBehavior.AllowGet);
                using (var con = new SqlConnection(_connectionString))
                {
                    var ifEmailExist = con.Query<bool>("Select 1 from MstServiceProviders WHERE  ProviderName=@ProviderName",
                    new { ProviderName }).FirstOrDefault();
                    return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult RemoteValidationCenterName(string CenterName, string CurrentCenterName)
        {

            // bool ifEmailExist = false;
            try
            {
                if (CenterName == CurrentCenterName)
                    return Json(true, JsonRequestBehavior.AllowGet);
                using (var con = new SqlConnection(_connectionString))
                {
                    var ifEmailExist = con.Query<bool>("Select 1 from MSTServiceCenters WHERE   CenterName=@CenterName",
                    new { CenterName }).FirstOrDefault();
                    return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }



        public ActionResult RemoteValidationForProblem(string Problem, string CurrentProblem)
        {

            // bool ifEmailExist = false;
            try
            {
                if (Problem == CurrentProblem)
                    return Json(true, JsonRequestBehavior.AllowGet);
                using (var con = new SqlConnection(_connectionString))
                {
                    var ifProblemExist = con.Query<bool>("Select 1 from MstDeviceProblem WHERE   Problem = @Problem",
                    new { Problem }).FirstOrDefault();
                    return Json(!ifProblemExist, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        #endregion

        #region WebsiteProblemList
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Spare_Problem_Price_matrix)]
        public ActionResult Probs_price_Matrix()
        {            
            ViewBag.BrandName= new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");                        
            ViewBag.Problem = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
            ViewBag.Model_Id = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Spare_Problem_Price_matrix)]
        public ActionResult AddWebsiteData()
        {            
            var parts = new Prob_Vs_price_matrix();
            parts.BrandList = new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");
            parts.ProblemList = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
            parts.ModelList = new SelectList(dropdown.BindModelName(),"value","text");
            return PartialView(parts);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Spare_Problem_Price_matrix)]
        [HttpPost]
        public ActionResult AddWebsiteData(Prob_Vs_price_matrix m)
        {            
            m.UserId = CurrentUser.UserId;
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new {
                    m.Model_Id,
                    Problem_Id =m.Problem,
                    m.Market_Price,
                    m.estimated_Price,
                    m.Min_Price,
                    m.Max_Price,
                    action ="Add",
                    m.UserId,
                    CurrentUser.CompanyId
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
                    response.Response = "Model And his Corresponding Problem is Already Registered";
                    TempData["response"] = response;                    
                }
                return RedirectToAction("Probs_price_Matrix");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Spare_Problem_Price_matrix)]
        public ActionResult EditWebsiteData(int websitePriceProblem, int ProblemId)
        {            
            var result = new Prob_Vs_price_matrix();
            using (var con = new SqlConnection(_connectionString))
            {
              

                result = con.Query<Prob_Vs_price_matrix>("sp_GetSingleRecord_Probles_VS_Price_Matrix ",
                    new { Problem_Id = ProblemId, model_ID = websitePriceProblem }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                ViewBag.Problem = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
                ViewBag.BrandName = new SelectList(dropdown.BindBrand(CurrentUser.CompanyId), "Value", "Text");
                if (result.Model_Id != null)
                {
                    ViewBag.Model_Id = new SelectList(dropdown.BindProduct(Int32.Parse(result.BrandName)), "Value", "Text");
                }
                else {
                    ViewBag.Model_Id = new SelectList(Enumerable.Empty<SelectListItem>());
                }
                
            }
            
            result.Problem = result.Problem_Id;
            
            return PartialView("EditWebsiteData", result);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Spare_Problem_Price_matrix)]
        [HttpPost]
        public ActionResult EditWebsiteData(Prob_Vs_price_matrix m)
        {           
            m.UserId = CurrentUser.UserId;
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", 
                    new { m.Model_Id,
                        Problem_Id = m.Problem,
                        m.Market_Price,
                        m.estimated_Price,
                        m.Min_Price,
                        m.Max_Price,
                        action = "edit",
                        m.UserId,
                        CurrentUser.CompanyId
                    },
                   commandType: CommandType.StoredProcedure).FirstOrDefault();
                var response = new ResponseModel();
                if (result == 2)
                {
                    response.IsSuccess = true;
                    response.Response = "Successfully Updated";
                    TempData["response"] = response;                   
                }
                else
                {
                    response.IsSuccess = true;
                    response.Response = "Not Updated Successfully";
                    TempData["response"] = response;                    
                }
                return RedirectToAction("Probs_price_Matrix");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Spare_Problem_Price_matrix)]
        public ActionResult WebsiteDataTable()
         {           
            using (var con = new SqlConnection(_connectionString))
            {                
                var result = con.Query<Prob_Vs_price_matrix>("Sp_Probles_VS_Price_matrix_List", new { CurrentUser.CompanyId },
                   commandType: CommandType.StoredProcedure).ToList();
                return View(result);

            }          
           
        }

        #endregion
    }
}
