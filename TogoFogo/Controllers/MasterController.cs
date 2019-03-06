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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Brands")]
        public ActionResult AddBrand()
        {
            return View();
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
                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));

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
                            model.CreatedBy,
                            model.ModifyBy ,
                            model.DeleteBy ,
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["Message"] = "Brand Name Already Exist";

                    }
                    else
                    {
                        TempData["Message"] = "Successfully Added";
                    }
                }

            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("Brand");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Brands")]
        public ActionResult EditBrand(int BrandId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<BrandModel>("Get_Single_Brand", new { BrandId = BrandId },
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
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
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
                        model.CreatedBy,
                        model.ModifyBy,
                        model.DeleteBy,
                        Action = "edit"
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 2)
                {
                    TempData["SubmitBrand"] = "Updated Successfully";
                }

                return RedirectToAction("Brand", "Master");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Brands")]
        public ActionResult BrandTable()
        {
            BrandModel objBrandModel = new BrandModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objBrandModel.ListBrandModel = con.Query<BrandModel>("Get_Brands", new { }, commandType: CommandType.StoredProcedure).ToList();
                
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objBrandModel._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objBrandModel._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objBrandModel._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objBrandModel._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objBrandModel._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objBrandModel._UserActionRights.Create = true;
                objBrandModel._UserActionRights.Edit = true;
                objBrandModel._UserActionRights.Delete = true;
                objBrandModel._UserActionRights.View = true;
                objBrandModel._UserActionRights.History = true;
                objBrandModel._UserActionRights.ExcelExport = true;

            }
            return View(objBrandModel);

        }
        #endregion
        #region PRODUCT
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Products")]
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


            var _UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];
            return View(_UserActionRights);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Products")]
        public ActionResult AddProduct()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                //var Subcat = con.Query<string>("SELECT DISTINCT SubCatName, SubCatId FROM MstSubCategory", null, commandType: CommandType.Text).ToList();
                ViewBag.BrandName = new SelectList(dropdown.BindBrand(), "Value", "Text");
                ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
                //ViewBag.Sub_Cat_Id = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                ViewBag.ProductColor = new SelectList(dropdown.BindProductColor(), "Value", "Text");

                return PartialView();
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

                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_Products",
                        new
                        {
                            ProductColor = finalValue,
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
                            model.User,
                            model.CreatedBy,
                            model.ModifyBy,
                            Action = "add"
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
                        TempData["AddProduct"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["AddProduct"] = "Product Name Already Exist";
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return RedirectToAction("Product");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Products")]
        public ActionResult ProductTable()
        {
            ProductModel objProductModel = new ProductModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objProductModel._ProductModelList = con.Query<ProductModel>("GetProductDetail", new { },
                    commandType: CommandType.StoredProcedure).ToList();           
              
            }

            objProductModel._UserActionRights = (UserActionRights)HttpContext.Items["ActionsRights"];

            return View(objProductModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Products")]
        public ActionResult EditProduct(int? ProductId, int? BrandID, string ProductName, int? CategoryID)
        {
            if (ProductId == 0 || ProductId == null)
            {
                ViewBag.BrandName = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.SubCategoryId = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ProductColor = new SelectList(dropdown.BindProductColor(), "Value", "Text");
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
                        ViewBag.ProductColor = new SelectList(dropdown.BindProductColor(), "Value", "Text");
                    }                    
                    if (result.SubCatId != null)
                    {
                        result.SubCategoryId = result.SubCatId.ToString();
                    }
                    ViewBag.BrandName = new SelectList(dropdown.BindBrand(), "Value", "Text");
                    ViewBag.SubCategoryId = new SelectList(dropdown.BindSubCategory(result.CategoryID), "Value", "Text");
                    ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
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
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));

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
                        model.User,
                        model.CreatedBy,
                        model.ModifyBy,
                        Action = "edit"
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 2)
                {
                    TempData["EditProduct"] = "Product Updated Successfully";
                }
                else
                {
                    TempData["EditProduct"] = "Something Went Wrong";
                }
                return RedirectToAction("Product", "Master");
            }
        }
        #endregion
        #region ManageDeviceProblems
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Device Problem")]
        public ActionResult ManageDeviceProblems()
        {
            ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["DeviceProblem"] != null)
            {
                ViewBag.DeviceProblem = TempData["DeviceProblem"].ToString();
            }
            return View();
        }
        public ActionResult AddDeviceProblem()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
               // var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstDeviceProblem", null, commandType: CommandType.Text).FirstOrDefault();
               // ViewBag.SortOrder = result + 1;
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddDeviceProblem(DeviceProblemModel model)
        {
            model.User = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
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
                            model.User,
                            Action = "add"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["DeviceProblem"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["DeviceProblem"] = "Problem Already Exist";
                    }
                }
                return RedirectToAction("ManageDeviceProblems");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Device Problem")]
        public ActionResult DeviceProblemtable()
        {
            DeviceProblemModel objDeviceProblemModel = new DeviceProblemModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objDeviceProblemModel._DeviceProblemModelList = con.Query<DeviceProblemModel>("GetProblemDetail", new { },
                    commandType: CommandType.StoredProcedure).ToList();
              
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objDeviceProblemModel._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objDeviceProblemModel._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objDeviceProblemModel._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objDeviceProblemModel._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objDeviceProblemModel._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objDeviceProblemModel._UserActionRights.Create = true;
                objDeviceProblemModel._UserActionRights.Edit = true;
                objDeviceProblemModel._UserActionRights.Delete = true;
                objDeviceProblemModel._UserActionRights.View = true;
                objDeviceProblemModel._UserActionRights.History = true;
                objDeviceProblemModel._UserActionRights.ExcelExport = true;

            }

            return View(objDeviceProblemModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Device Problem")]
        public ActionResult EditDeviceProblem(int? ProblemID)
        {
            ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
            ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
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
                            model.User,
                            Action = "edit"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["DeviceProblem"] = "Successfully Updated";
                    }
                }
                return RedirectToAction("ManageDeviceProblems");

            }
        }
        #endregion
        #region ColorMaster
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Color Master")]
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
            return View();
        }
        public ActionResult AddColorMaster()
        {
            ViewBag.Brand = new SelectList(dropdown.BindBrand(), "Value", "Text");
            //ViewBag.Model = new SelectList(dropdown.BindModelName(), "Value", "Text");
            ViewBag.Model = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        [HttpPost]
        public ActionResult AddColorMaster(ColorModel m)
        {
            m.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
            m.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
            using (var con = new SqlConnection(_connectionString))
            {
                var result1 = con.Query<int>("Insert_Into_Color_Master",
                           new
                           {
                               m.ColorName,
                               m.IsActive,
                               m.Comments,
                               m.CreatedBy,
                               m.ModifyBy,
                               Action = "add",
                               m.ColorId
                           }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result1 == 1)
                {
                    TempData["Message"] = "Added Successfully";
                }
                else
                {
                    TempData["Message"] = "Something went wrong";
                }
            }
            return RedirectToAction("ColorMaster", "Master");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Color Master")]
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
         
            m.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
            using (var con = new SqlConnection(_connectionString))
            {
                var result1 = con.Query<int>("Insert_Into_Color_Master",
                           new
                           {
                               m.ColorName,
                               m.IsActive,
                               m.Comments,
                               m.CreatedBy,
                               m.ModifyBy,
                               Action = "edit",
                               m.ColorId,

                           }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result1 == 2)
                {
                    TempData["Message"] = "Updated Successfully";
                }
                else
                {
                    TempData["Message"] = "Something went wrong";
                }
            }
            return RedirectToAction("ColorMaster", "Master");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Color Master")]
        public ActionResult ColorTable()
        {
            ColorModel objColorModel = new ColorModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objColorModel._ColorModelList = con.Query<ColorModel>("Select cm.ColorId,cm.ColorName,cm.IsActive,cm.Comments,cm.CreatedDate,cm.ModifyDate,cum.UserName 'CreatedBy',cum1.UserName 'ModifyBy' from Color_Master cm left join Create_User_Master cum on cum.Id=cm.CreatedBy left join Create_User_Master cum1 on cum1.Id=cm.ModifyBy", new { },
                    commandType: CommandType.Text).ToList();              
              
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objColorModel._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objColorModel._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objColorModel._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objColorModel._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objColorModel._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objColorModel._UserActionRights.Create = true;
                objColorModel._UserActionRights.Edit = true;
                objColorModel._UserActionRights.Delete = true;
                objColorModel._UserActionRights.View = true;
                objColorModel._UserActionRights.History = true;
                objColorModel._UserActionRights.ExcelExport = true;

            }

            return View(objColorModel);
        }
        #endregion
        #region RemoteValidation
        public ActionResult RemoteValidationforUserName(string Username,Int64 UserId=0)
        {
           // bool ifEmailExist = false;
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var ifEmailExist = con.Query<bool>("UspCheckUserExists",
                    new { Username, UserId}, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    //ifEmailExist = result==0 ? false : true;

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
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Spare Problem Price matrix")]
        public ActionResult Probs_price_Matrix()
        {
            ViewBag.BrandName= new SelectList(dropdown.BindBrand(), "Value", "Text");                        
            ViewBag.Problem = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
            ViewBag.Model_Id = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult AddWebsiteData()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddWebsiteData(Prob_Vs_price_matrix m)
        {
            m.UserId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new { m.Model_Id,Problem_Id=m.Problem,m.Market_Price,m.estimated_Price,m.Min_Price,m.Max_Price,action="Add", m.UserId },
                   commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 1)
                {
                    TempData["Message"] = "Successfully Added";
                }
                else {
                    TempData["Message"] = "Model And his Corresponding Problem is Already Registered";
                }
                return RedirectToAction("Probs_price_Matrix");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Spare Problem Price matrix")]
        public ActionResult EditWebsiteData(int websitePriceProblem, int ProblemId)
        {
            var result = new Prob_Vs_price_matrix();
            using (var con = new SqlConnection(_connectionString))
            {
                    result = con.Query<Prob_Vs_price_matrix>("sp_GetSingleRecord_Probles_VS_Price_Matrix ",
                    new { Problem_Id = ProblemId, model_ID = websitePriceProblem }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                ViewBag.Problem = new SelectList(dropdown.BindMstDeviceProblemAbhishek(), "Value", "Text");
                ViewBag.BrandName = new SelectList(dropdown.BindBrand(), "Value", "Text");
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
            m.UserId = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new { m.Model_Id, Problem_Id = m.Problem, m.Market_Price, m.estimated_Price, m.Min_Price, m.Max_Price, action = "edit", m.UserId },
                   commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 2)
                {
                    TempData["Message"] = "Successfully Updated";
                }
                else
                {
                    TempData["Message"] = "Nothing Updated";
                }
                return RedirectToAction("Probs_price_Matrix");
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Spare Problem Price matrix")]
        public ActionResult WebsiteDataTable()
         {
            Prob_Vs_price_matrix objProb_Vs_price_matrix = new Prob_Vs_price_matrix();
            using (var con = new SqlConnection(_connectionString))
            {
                objProb_Vs_price_matrix._Prob_Vs_price_matrixList = con.Query<Prob_Vs_price_matrix>("Sp_Probles_VS_Price_matrix_List", null,
                   commandType: CommandType.StoredProcedure).ToList();

                
            }

            UserActionRights objUserActiobRight = new UserActionRights();
            objProb_Vs_price_matrix._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objProb_Vs_price_matrix._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objProb_Vs_price_matrix._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objProb_Vs_price_matrix._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objProb_Vs_price_matrix._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objProb_Vs_price_matrix._UserActionRights.Create = true;
                objProb_Vs_price_matrix._UserActionRights.Edit = true;
                objProb_Vs_price_matrix._UserActionRights.Delete = true;
                objProb_Vs_price_matrix._UserActionRights.View = true;
                objProb_Vs_price_matrix._UserActionRights.History = true;
                objProb_Vs_price_matrix._UserActionRights.ExcelExport = true;

            }

            return View(objProb_Vs_price_matrix);
        }

        #endregion
    }
}
