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
using TogoFogo.Models;
using TogoFogo.Extension;
using TogoFogo.SaveImageCode;
using System.Collections.Generic;
using System.Data.OleDb;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    //[CustomAuthorize]
    public class MasterController : Controller
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
                            User = SessionModel.UserId,
                            Action = "add",
                            SessionModel.CompanyId
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
                        User = SessionModel.UserId,
                        Action = "edit",
                        SessionModel.CompanyId
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
            
                
                var result= con.Query<BrandModel>("Get_Brands", new { companyId= SessionModel.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
           }      
         
        }
        #endregion
        #region PRODUCT
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Products)]
        public ActionResult Product()
        {
            ViewBag.BrandName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Sub_Cat_Id = new SelectList(Enumerable.Empty<SelectListItem>());
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
                    _BrandName= new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text"),
                    _Category= new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text"),
                    _ProductColor= new SelectList(dropdown.BindProductColor(SessionModel.CompanyId), "Value", "Text"),
                    _SubCat= new SelectList(Enumerable.Empty<SelectList>())

                };               

                return PartialView(pm);
            }
        }
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
                            ProductColor = finalValue,
                            model.ProductId,
                            model.CategoryID,    
                            Brand_ID = model.BrandID,
                            SubCatId = model.SubCategoryId,
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
                            Action = "add",
                            SessionModel.CompanyId
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    var response = new ResponseModel();
                    if (result !=0)
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
                                  BrandId = model.BrandName
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

                var result = con.Query<ProductModel>("GetProductDetail", new { SessionModel.CompanyId },
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

           

            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Products)]
        public ActionResult EditProduct(int? ProductId, int? BrandID, string ProductName, int? CategoryID)
        {
            if (ProductId == 0 || ProductId == null)
            {
                ViewBag.BrandName = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.SubCategoryId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ProductColor = new SelectList(dropdown.BindProductColor(SessionModel.CompanyId), "Value", "Text");
            }
            else
            {
                using (var con = new SqlConnection(_connectionString))
                {
                
                    var result = con.Query<ProductModel>("Get_Single_Product", new { ProductName = ProductName },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result.Product_Color != null)
                    {
                        result.ProductColor = result.Product_Color.Split(',');
                        ViewBag.ProductColor = new SelectList(dropdown.BindProductColor(SessionModel.CompanyId), "Value", "Text");
                    }                    
                    if (result.SubCatId != null)
                    {
                        result.SubCategoryId = result.SubCatId.ToString();
                    }

                    ViewBag.BrandName = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
                    ViewBag.SubCategoryId = new SelectList(dropdown.BindSubCategory(result.CategoryID), "Value", "Text");
                    ViewBag.Category = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
                    ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(result.CategoryID), "Value", "Text");
                    if (result != null)
                    {
                        result.BrandName = result.BrandID.ToString();
                        result.Category = result.CategoryID.ToString();
                    }
                    return PartialView("EditProduct", result);
                }
            }
            return PartialView("EditProduct");
        }
        [HttpPost]
        public ActionResult EditProduct(ProductModel model)
        {
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
                        ProductColor=finalValue,
                        model.ProductId,
                        CategoryID = model.Category,
                        Brand_ID = model.BrandName,
                        SubCatId = model.SubCategoryId,
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
            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["DeviceProblem"] != null)
            {
                ViewBag.DeviceProblem = TempData["DeviceProblem"].ToString();
            }
            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Device_Problem)]
        public ActionResult AddDeviceProblem()
        {
            using (var con = new SqlConnection(_connectionString))
            {
               
                ViewBag.Category = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
               // var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstDeviceProblem", null, commandType: CommandType.Text).FirstOrDefault();
               // ViewBag.SortOrder = result + 1;
                return PartialView();
            }
        }
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
                            CatId = model.Category,
                            SubCatId = model.SubCategory,
                            model.ProblemID,
                            model.IsActive,
                            model.Problem,
                            model.SortOrder,
                            User = Convert.ToInt32(Session["User_Id"]),
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
                        response.Response = "Not Added Successfully";
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
                var result= con.Query<DeviceProblemModel>("GetProblemDetail", new { },
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
            
           
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Device_Problem)]
        public ActionResult EditDeviceProblem(int? ProblemID)
        {

            ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
            ViewBag.Category = new SelectList(dropdown.BindCategory(SessionModel.CompanyId), "Value", "Text");
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

                return View(result);
            }
            
        }
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
                            CatId = model.Category,
                            SubCatId = model.SubCategory,
                            model.ProblemID,
                            model.IsActive,
                            model.Problem,
                            model.SortOrder,
                            User = Convert.ToInt32(Session["User_Id"]),
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
   
            ViewBag.Brand = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            //ViewBag.Model = new SelectList(dropdown.BindModelName(), "Value", "Text");
            ViewBag.Model = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView();
        }
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
                               User = Convert.ToInt32(Session["User_Id"]),
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
                               User = Convert.ToInt32(Session["User_Id"]),
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
                var result = con.Query<ColorModel>("Select cm.ColorId,cm.ColorName,cm.IsActive,cm.Comments,cm.CreatedDate,cm.ModifyDate,cum.UserName 'CBy',cum1.UserName 'MBy' from Color_Master cm left join Create_User_Master cum on cum.Id=cm.CreatedBy left join Create_User_Master cum1 on cum1.Id=cm.ModifyBy", new { },
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

        public ActionResult RemoteValidationClientName(string ClientName, string CurrentClientName)
        {

            // bool ifEmailExist = false;
            try
            {
                if (ClientName == CurrentClientName)
                    return Json(true, JsonRequestBehavior.AllowGet);

                using (var con = new SqlConnection(_connectionString))
                {
                    var ifEmailExist = con.Query<bool>("Select 1 from MSTCLIENTS WHERE ISACTIVE=1 and ClientName=@ClientName",
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
                    var ifEmailExist = con.Query<bool>("Select 1 from MstServiceProviders WHERE ISACTIVE=1 and ProviderName=@ProviderName",
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
                    var ifEmailExist = con.Query<bool>("Select 1 from MSTServiceCenters WHERE ISACTIVE=1 and CenterName=@CenterName",
                    new { CenterName }).FirstOrDefault();
                    return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
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
    
            ViewBag.BrandName= new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");                        
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
            parts.BrandList = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
            parts.ProblemList = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
            parts.ModelList = new SelectList(Enumerable.Empty<SelectListItem>());
            return PartialView(parts);
        }
        [HttpPost]
        public ActionResult AddWebsiteData(Prob_Vs_price_matrix m)
        {
            m.UserId = SessionModel.UserId;
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new { m.Model_Id,Problem_Id=m.Problem,m.Market_Price,m.estimated_Price,m.Min_Price,m.Max_Price,action="Add", m.UserId },
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
                ViewBag.BrandName = new SelectList(dropdown.BindBrand(SessionModel.CompanyId), "Value", "Text");
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
        [HttpPost]
        public ActionResult EditWebsiteData(Prob_Vs_price_matrix m)
        {
            m.UserId = SessionModel.UserId;
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new { m.Model_Id, Problem_Id = m.Problem, m.Market_Price, m.estimated_Price, m.Min_Price, m.Max_Price, action = "edit", m.UserId },
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
               var result = con.Query<Prob_Vs_price_matrix>("Sp_Probles_VS_Price_matrix_List", null,
                   commandType: CommandType.StoredProcedure).ToList();
                return View(result);

            }          
           
        }

        #endregion
    }
}
