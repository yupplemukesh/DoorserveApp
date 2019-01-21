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
        [CustomAuthorize]
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
                            model.Is_repair,
                            model.Is_Active,
                            model.Comments,
                            CreatedBy = "",
                            ModifyBy = "",
                            DeleteBy = "",
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

                var result = con.Query<int>("Add_Modify_Delete_Brand"
                    , new
                    {
                        model.BrandId,
                        model.BrandName,
                        model.BrandImage,
                        model.BrandDescription,
                        model.Is_repair,
                        model.Is_Active,
                        model.Comments,
                        model.MetaKeyword,
                        model.MetaDescription,
                        model.MetaTitle,
                        model.UrlName,
                        model.Header,
                        model.Footer,
                        CreatedBy = "",
                        ModifyBy = "",
                        DeleteBy = "",
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

        public ActionResult BrandTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<BrandModel>("Get_Brands", new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }

        }

        #endregion

        #region PRODUCT
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
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Edit_Delete_Products",
                        new
                        {
                            ProductColor = finalValue,
                            model.ProductId,
                            Category_ID = model.Category,
                            Brand_ID = model.BrandName,
                            SubCatId = model.Sub_Cat_Id,
                            model.ProductName,
                            model.Alt_ProductName,
                            model.MRP,
                            model.Market_Price,
                            model.TUPC,
                            model.ProductImage,
                            model.Is_repair,
                            model.Is_Active,
                            model.Comments,
                            model.User,
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
        public ActionResult ProductTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {               
                var result = con.Query<ProductModel>("GetProductDetail", new { },
                    commandType: CommandType.StoredProcedure).ToList();           
               return View(result);
            }
        }

        public ActionResult EditProduct(int? ProductId, int? BrandID, string ProductName, int? CategoryID)
        {
            if (ProductId == 0 || ProductId == null)
            {
                ViewBag.BrandName = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Category = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Sub_Cat_Id = new SelectList(Enumerable.Empty<SelectListItem>());
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
                        result.Sub_Cat_Id = result.SubCatId.ToString();
                    }
                    ViewBag.BrandName = new SelectList(dropdown.BindBrand(), "Value", "Text");
                    ViewBag.Sub_Cat_Id = new SelectList(dropdown.BindSubCategory(result.Category_ID), "Value", "Text");
                    ViewBag.Category = new SelectList(dropdown.BindCategory(), "Value", "Text");
                    ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(result.Category_ID), "Value", "Text");
                    if (result != null)
                    {
                        result.BrandName = result.Brand_ID.ToString();
                        result.Category = result.Category_ID.ToString();
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
                        Category_ID = model.Category,
                        Brand_ID = model.BrandName,
                        SubCatId = model.Sub_Cat_Id,
                        model.ProductName,
                        model.Alt_ProductName,
                        model.MRP,
                        model.Market_Price,
                        model.TUPC,
                        model.ProductImage,
                        model.Is_repair,
                        model.Is_Active,
                        model.Comments,
                        model.User,
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
        [CustomAuthorize]
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
                var result = con.Query<int>("SELECT coalesce(MAX(SortOrder),0) from MstDeviceProblem", null, commandType: CommandType.Text).FirstOrDefault();
                ViewBag.SortOrder = result + 1;
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddDeviceProblem(DeviceProblemModel model)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                if (model.Problem == null)
                {                }
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
                            User = "",
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

        public ActionResult DeviceProblemtable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<DeviceProblemModel>("GetProblemDetail", new { },
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
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
            return View();
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
                            User = "",
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
        [CustomAuthorize]
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
            using (var con = new SqlConnection(_connectionString))
            {
                var result1 = con.Query<int>("Insert_Into_Color_Master",
                           new
                           {
                               m.ColorName,
                               m.IsActive,
                               m.Comments,
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
                               m.ColorName,
                               m.IsActive,
                               m.Comments,
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

        public ActionResult ColorTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ColorModel>("Select * from Color_Master", new { },
                    commandType: CommandType.Text).ToList();              
                return View(result);
            }
        }
        #endregion

        #region UserRole

        [CustomAuthorize]
        public ActionResult UserRole()
        {
            ViewBag.UserType = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult CreateUserRole()
        {
            var name = User.Identity.Name;
            if (name != null)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<string>("select UserType from User_Type where UserTypeId in (select UserType from Create_User_Master where UserName=@UserName)", new{ @UserName=name },
                       commandType: CommandType.Text).FirstOrDefault();
                    if (result == "Admin")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeAdmin(), "Value", "Text");
                    }
                    else if (result == "Client")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeClient(), "Value", "Text");
                    }
                    else if (result == "Service Center")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeServiceCenter(), "Value", "Text");
                    }
                    else if (result == "Service Provider")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeService_Provider(), "Value", "Text");
                    }
                    else if (result == "Customer")
                    {
                        ViewBag.UserType = new SelectList(Enumerable.Empty<SelectListItem>());
                    }
                    else
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
                    }
                }
            }           
         return View();
        }
        [HttpPost]
        public ActionResult CreateUserRole(UserRoleModel m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_User_Role_Master",
                        new {
                            m.UserType,
                            m.UserRole,
                            m.IsActive,
                            m.CreatedBy,
                            Action="add",
                            ModifiedBy="",
                            m.RoleId
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        foreach (var item in m.TableData)
                        {
                            var RoleId = con.Query<int>("select RoleId from User_Role_Master where UserRole=@UserRole", new { @UserRole = m.UserRole }, commandType: CommandType.Text).FirstOrDefault();
                            var result1 = con.Execute("insert into User_Role_rights values(@RoleId,@MenuId,@ParentMenu_id)", new { RoleId = RoleId, MenuID = item.MenuId, ParentMenu_id = item.ParentMenu_id },
                            commandType: CommandType.Text);
                           
                        }
                        TempData["Message"] = " User Role Added Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Something went wrong";
                    }
                }
            }
            return RedirectToAction("UserRole");
        }
        public JsonResult EditUserRole(int ? RoleId)
        {
            ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");

            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserRoleModel>("Select * from User_Role_Master where RoleId=@RoleId", new { RoleId },
                    commandType: CommandType.Text).FirstOrDefault();


                result.UserType = result.UserTypeId;
                var result1 = con.Query<UserRolrightsModel>("select MenuId,ParentMenu_id from User_Role_rights where RoleId=@RoleId", new { RoleId },
                   commandType: CommandType.Text).ToList();
                if (result1 != null)
                {
                    result.TableData = result1;
                }
                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditUserRole(UserRoleModel m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_User_Role_Master",
                        new
                        {
                            m.UserType,
                            m.RoleId,
                            m.UserRole,
                            m.IsActive,
                            m.CreatedBy,
                            Action = "edit",
                            m.ModifiedBy
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["Message"] = "Updated Successfully";
                    }
                }
            }
            return RedirectToAction("UserRole", "Master");
        }
        public ActionResult TableRole()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserRoleModel>("Tableview_User_Role_Master", new { },
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public ActionResult UserRoleAddRights(int RoleId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserRoleModel>("select * from User_Role_Master where RoleId=@RoleId", new { RoleId },
                    commandType: CommandType.Text).FirstOrDefault();
                var resultUserType = con.Query<string>("select userType from User_Type where UserTypeId=@UserTypeId", new { result.UserTypeId }, commandType: CommandType.Text).FirstOrDefault();
                result.UserType = resultUserType;
                ViewBag.MenuMasters = new SelectList(dropdown.MenuMaster(), "Value", "Text");
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult UserRoleAddRights(UserRoleModel m)
        {
            if (m == null || m.foo == null || string.IsNullOrEmpty(m.RoleId))
            {
                return View(m);
            }
            foreach (var item in m.foo)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Execute("insert into User_Role_rights values(@RoleId,@MenuId,@ParentMenu_id)", new { RoleId = m.RoleId, MenuID = item, ParentMenu_id = m.MenuMasters },
                        commandType: CommandType.Text);
                }
            }
            return RedirectToAction("CreateUserType", "Master");

        }

        #endregion

        #region UserMaster
        [CustomAuthorize]
        public ActionResult UserMaster()
        {
            ViewBag.TRC = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ServiceProvider = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.MenuMasters = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.UserType = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.UserRole = new SelectList(Enumerable.Empty<SelectListItem>());
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult CreateUser()
        {           
            ViewBag.TRC = new SelectList(dropdown.BindTrc(), "Value", "Text");
            ViewBag.ServiceProvider = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
            ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
            ViewBag.UserRole = new SelectList(dropdown.BindUserRole(), "Value", "Text");
            return View();
        }
       
        [HttpPost]
        public ActionResult CreateUser(CreateUserModel m)
        {
            if (ModelState.IsValid)
            {
                Random r = new Random();
                int randomNumber = r.Next(999, 10000);
                string Password=randomNumber.ToString();
                string Passwrd = "";
                var mpc = new Email_send_code();
                Type type = mpc.GetType();
                var Status = (int)type.InvokeMember("sendmail_update",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, mpc,
                                        new object[] { m.Email_Address,Password,m.UserName });
                if (Status == 1)
                {                 
                    Passwrd =TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(Password,true);
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<string>("Insert_Into_Create_User_Master",
                            new
                            {
                                m.UserRole,
                                m.ID,
                                m.UserType,
                                m.Name,
                                m.Address,
                                m.Mobile,
                                m.Email_Address,
                                m.UserName,
                                m.ServiceProvider,
                                m.ServiceProviderType,
                                TrcId = m.TRC,
                                Password = Passwrd,
                                m.IsActive,
                                m.CreatedBy,
                                m.ModifiedBy,
                                Action = "add"
                            },
                            commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result != null)
                        {                            
                            foreach (var item in m.TableData)
                            {
                                var USERID = con.Query<int>("select Id from Create_User_Master where UserName=@User", new { @User = m.UserName },
                                    commandType: CommandType.Text).FirstOrDefault();

                                var AddRights = con.Execute("insert into user_rights_Test values(@UserId,@MenuId,@ParentMenu_id)",
                                    new { UserId = USERID, MenuID = item.MenuId, ParentMenu_id = item.ParentMenu_id },
                                    commandType: CommandType.Text);
                            }
                            TempData["Message"] = result;
                        }
                    }
                }
            }
            return RedirectToAction("UserMaster","Master");
        }
        public ActionResult EditUser(int ID)
        {
            ViewBag.TRC = new SelectList(dropdown.BindTrc(), "Value", "Text");
            ViewBag.ServiceProvider = new SelectList(dropdown.BindServiceProvider(), "Value", "Text");
            ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
            ViewBag.UserRole = new SelectList(dropdown.BindUserRole(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CreateUserModel>("select * from create_user_master where Id=@ID", new {ID},
                    commandType: CommandType.Text).FirstOrDefault();
                result.UserRole = result.RoleId;
                return PartialView("EditUser", result);
            }
        }
        [HttpPost]
        public ActionResult EditUser(CreateUserModel m)
        {            
            if (ModelState.IsValid)
            {               
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<string>("Insert_Into_Create_User_Master",
                        new
                        {
                            m.UserRole, m.ID,m.UserType,m.Name,m.Address,m.Mobile,m.Email_Address,m.UserName,m.ServiceProvider,m.ServiceProviderType,TrcId = m.TRC,
                            m.Password,m.IsActive,m.CreatedBy,m.ModifiedBy,Action = "edit"
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            return RedirectToAction("UserMaster","Master");
        }
        public ActionResult EditPassword(int? Id,string Email,string username)
        {
            var NewPassword = "";
            int Status = 0;
            if (Id != null)
            {               
                Random r = new Random();
                NewPassword = r.Next(999, 10000).ToString();
                var mpc = new Email_send_code();
                Type type = mpc.GetType();
                 Status = (int)type.InvokeMember("sendmail_update",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, mpc,
                                        new object[] { Email, NewPassword, username });
                if (Status == 1)
                {
                    NewPassword = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(NewPassword, true);/* base64Encode(randomNumber)*/;
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<string>("ChangePassword_In_Create_User_Master", new { ID = Id, Password = NewPassword, Action = "ChangePassword" },
                            commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }
                }                               
            }
            return RedirectToAction("UserMaster", "Master");
        }
        public ActionResult UserTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CreateUserModel>("Proc_create_user_master_List", null,
                    commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public ActionResult AddRights(int ID)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CreateUserModel>("select * from create_user_master where Id=@ID", new { ID },
                    commandType: CommandType.Text).FirstOrDefault();
                var result1 = con.Query<string>("select UserType from User_Type where UserTypeId=@UserTypeId", new { UserTypeId = result.UserType }, commandType: CommandType.Text).FirstOrDefault();
                result.UserType = result1;
                var result2 = con.Query<string>("select UserRole from user_role_master where RoleId=@RoleId", new { RoleId = result.RoleId }, commandType: CommandType.Text).FirstOrDefault();
                result.UserRole = result2;
                ViewBag.MenuMasters = new SelectList(dropdown.MenuMaster(), "Value", "Text");
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult AddRights(CreateUserModel m)
        {
            if (m == null || m.foo == null || string.IsNullOrEmpty(m.ID))
            {
                return View(m);
            }
            //using (var con = new SqlConnection(_connectionString))
            //{
            //    var result = con.Execute("insert into user_rights values(@UserId,@MenuId)", new { UserId = m.ID, MenuID = m.MenuMasters },
            //        commandType: CommandType.Text);

            //    //  ViewBag.MenuMasters = new SelectList(dropdown.MenuMaster(), "Value", "Text");

            //}
            foreach (var item in m.foo)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Execute("insert into user_rights_Test values(@UserId,@MenuId,@ParentMenu_id)", new { UserId = m.ID,MenuID=item, ParentMenu_id =m.MenuMasters},
                        commandType: CommandType.Text);

                  //  ViewBag.MenuMasters = new SelectList(dropdown.MenuMaster(), "Value", "Text");
                }
            }
            return RedirectToAction("UserMaster", "Master");
        }
        public JsonResult SubMenu(int? MainMenuId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<MenuMasterModel>("select * from menuTable where parentMenuId=@parentMenuId",
                    new { @parentMenuId =MainMenuId }, commandType: CommandType.Text).ToList();
               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SubMenuUserType(int? UserTypeID)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<MenuMasterModel>("select * from menuTable where MenuCap_ID in (select menuId from user_type_rights where usertypeId=@usertypeId ) ",
                    new { @usertypeId = UserTypeID }, commandType: CommandType.Text).ToList();
                foreach (var item in result)
                {
                    var parent = item.ParentMenuId;
                    var ParentMenuName = con.Query<string>(" select Menu_Name from menuTable where MenuCap_ID=@MenuCap_ID", new { MenuCap_ID = parent }, commandType: CommandType.Text).FirstOrDefault();
                    item.ParentMenuName = ParentMenuName;
                }
               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SubMenuRole(int? RoleID)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<MenuMasterModel>("select * from menuTable where MenuCap_ID in (select menuId from user_role_rights where RoleId=@RoleId ) ",
                    new { @RoleId = RoleID }, commandType: CommandType.Text).ToList();
                foreach (var item in result)
                {
                    var parent = item.ParentMenuId;
                    var ParentMenuName = con.Query<string>(" select Menu_Name from menuTable where MenuCap_ID=@MenuCap_ID", new { MenuCap_ID = parent }, commandType: CommandType.Text).FirstOrDefault();
                    item.ParentMenuName = ParentMenuName;
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectedView(int? UserId, int? MainMenuId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<submenuModel>("select MenuId from user_rights_Test where UserId=@UserId and ParentMenu_id=@ParentMenu_id",
                    new {UserId,@ParentMenu_id=MainMenuId }, commandType: CommandType.Text).ToList();
                //foreach (var item in result)
                //{
                //    var result1 = con.Query<submenuModel>("select Menu_Name,ID from mstmenu where ID=@ID",
                //    new { @ID = item.MenuId }, commandType: CommandType.Text).ToList();
                //}
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region RemoteValidation
        public ActionResult RemoteValidationforUserName( string Username)
        {
            bool ifEmailExist = false;
            try

            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<string>("select UserName from create_user_Master where UserName=@UserName",
                    new { @UserName=Username}, commandType: CommandType.Text).FirstOrDefault();
                    ifEmailExist = Username.Equals(result) ? true : false;

                    return Json(!ifEmailExist, JsonRequestBehavior.AllowGet);
                }                  
            }

            catch (Exception ex)

            {

                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        #endregion

        #region UserType
        public ActionResult CreateUserType()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }
        public ActionResult UserType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserType(UserTypeModel m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_User_Type",
                        new
                        {
                            m.UserType,
                            m.IsActive,
                            m.CreatedBy,
                            Action = "add",
                            ModifiedBy = "",
                            m.UserTypeId
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 0)
                    {
                        TempData["Message"] = "User Type Already Exist";

                    }
                    else
                    {
                        TempData["Message"] = "Successfully Added";
                    }
                }
            }
            return View();
        }
        public ActionResult TableUserType()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserTypeModel>("Select * from User_Type", new { },
                    commandType: CommandType.Text).ToList();
                return View(result);
            }
        }
        public ActionResult EditUserType(int UserTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserTypeModel>("Select * from User_Type where UserTypeId=@UserTypeId", new { UserTypeId },
                    commandType: CommandType.Text).FirstOrDefault();
                return PartialView("EditUserType", result);
            }
        }
        [HttpPost]
        public ActionResult EditUserType(UserTypeModel m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_User_Type",
                        new
                        {
                            m.UserType,
                            m.IsActive,
                            m.CreatedBy,
                            Action = "edit",
                            ModifiedBy = "",
                            m.UserTypeId
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["Message"] = "Updated Successfully";
                    }
                }
            }
            return RedirectToAction("CreateUserType", "Master");
        }

        public ActionResult UserTypeAddRights(int UserTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserTypeModel>("select * from user_type where UserTypeId=@UserTypeId", new { UserTypeId },
                    commandType: CommandType.Text).FirstOrDefault();

                ViewBag.MenuMasters = new SelectList(dropdown.MenuMaster(), "Value", "Text");
                return View(result);
            }
        }

        [HttpPost]
        public ActionResult UserTypeAddRights(UserTypeModel m)
        {
            if (m == null || m.foo == null || string.IsNullOrEmpty(m.UserTypeId))
            {
                return View(m);
            }
            foreach (var item in m.foo)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Execute("insert into User_Type_rights values(@UserTypeId,@MenuId,@ParentMenu_id)", new { UserTypeId = m.UserTypeId, MenuID = item, ParentMenu_id = m.MenuMasters },
                        commandType: CommandType.Text);
                }
            }
            return RedirectToAction("CreateUserType", "Master");

        }

        #endregion

        #region UserType1
        public ActionResult CreateUserType1()
        {
            return View();
        }
        public ActionResult UserTypeTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserTypeModel>("select * from User_Type order by CreatedDate Desc", new { },
                    commandType: CommandType.Text).ToList();

                return View(result);
            }
        }
        public ActionResult EditUserType1( int? UserTypeId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = new UserTypeModel();
                result = con.Query<UserTypeModel>("Select * from User_Type where UserTypeId=@UserTypeId", new { UserTypeId },
                    commandType: CommandType.Text).FirstOrDefault();
            
                
                
                var result1 = con.Query<TableMenuModel>("TableMenuListGrid",
                    new { }, commandType: CommandType.Text).ToList();
                
                result.TableMenu = result1;
                var checkedBox = con.Query<ViewStatusCheck>("select MenuId from User_Type_rights where UserTypeId=@UserTypeId", new { @UserTypeId = UserTypeId },
                    commandType: CommandType.Text).ToList();
                foreach (var i in result.TableMenu)
                {
                    foreach (var item in checkedBox)
                    {
                        if (item.MenuId == i.MenuCap_ID)
                        {
                            i.isNewlyEnrolled = true;
                            break;
                        }
                        
                    }
                }
             
                return View(result);
            }
        }
        public ActionResult AddNewUserType1()
        {
            var result = new UserTypeModel();
            using (var con = new SqlConnection(_connectionString))
            {
                

                var result1 = con.Query<TableMenuModel>("TableMenuListGrid",
                    new { }, commandType: CommandType.Text).ToList();

                result.TableMenu = result1;
            }
                return View(result);
        }
        [HttpPost]
        public ActionResult AddNewUserType1(UserTypeModel m)
        {
            if (m == null || m.TableMenu == null )
            {
                return View(m);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<string>("Insert_Into_User_type_Table"
                    , new
                    {

                        UserTypeId = m.UserTypeId,
                        UserType = m.UserType,
                        IsActive = m.IsActive,
                        Comments = m.Comments,
                        Action = "Add"
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == "Inserted")
                {
                    var UserTypeId = con.Query<string>("select UserTypeId from User_type where UserType=@UserType", new{ UserType = m.UserType},commandType: CommandType.Text).FirstOrDefault();

                    
                    foreach (var item in m.TableMenu)
                    {

                        if (item.isNewlyEnrolled == true)
                        {

                            var result1 = con.Query<int>("Insert_into_User_type_rights", new { UserTypeId = UserTypeId, MenuID = item.MenuCap_ID, ParentMenu_id = item.ParentMenuID, Action="Add" },
                                commandType: CommandType.StoredProcedure);

                        }

                    }
                }
            }

            return RedirectToAction("CreateUserType1");
        }
        [HttpPost]
        public ActionResult EditUserType1(UserTypeModel m)
        {
            if (m == null || m.TableMenu == null || string.IsNullOrEmpty(m.UserTypeId))
            {
                return View(m);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<string>("Insert_Into_User_type_Table"
                    , new {
                     
                        UserTypeId = m.UserTypeId,
                        UserType = m.UserType,
                        IsActive=m.IsActive,
                        Comments=m.Comments,
                        Action="Edit"
                        },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == "Updated")
                {
                    foreach (var item in m.TableMenu)
                    {
                        if (item.isNewlyEnrolled == true)
                        {

                            var result1 = con.Query<string>("Insert_into_User_type_rights", new { UserTypeId = m.UserTypeId, MenuID = item.MenuCap_ID, ParentMenu_id = item.ParentMenuID, Action = "Edit" },
                                commandType: CommandType.StoredProcedure);

                        }
                        else
                        {
                            var result1 = con.Execute("Delete from User_Type_Rights where UserTypeId=@UserTypeId and MenuID=@MenuID", new { UserTypeId = m.UserTypeId, MenuID = item.MenuCap_ID },
                                commandType: CommandType.Text);
                        }

                    }
                }
            }
           
            return RedirectToAction("CreateUserType1");
        }

        #endregion

        #region Role1
        public ActionResult CreateRole1()
        {
            ViewBag.UserType = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        public ActionResult RoleTable1()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<UserRoleModel>("List_of_User_Role_master", new { },
                    commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        [HttpGet]
        public ActionResult EditUserRole1(int? RoleID)
        {
            ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = new UserRole1();
                result = con.Query<UserRole1>("Select * from User_Role_master where RoleId=@RoleId", new { @RoleId= RoleID },
                    commandType: CommandType.Text).FirstOrDefault();
                result.UserType = result.UserTypeId;

                var result2 = con.Query<TableMenuModel>("select * from menuTable where MenuCap_ID in (select menuId from user_type_rights where usertypeId=@usertypeId ) ",
                   new { @usertypeId = result.UserTypeId }, commandType: CommandType.Text).ToList();
                var menu = "";
                foreach (var item in result2)
                {
                    var parent = item.ParentMenuID;

                    menu = item.Menu_Name;
                    var ParentMenuName = con.Query<string>(" select Menu_Name from menuTable where MenuCap_ID=@MenuCap_ID", new { MenuCap_ID = parent }, commandType: CommandType.Text).FirstOrDefault();
                    item.Menu_Name = ParentMenuName;
                    item.SubMenuName = menu;
                    var result3 = con.Query<string>("select MenuId from User_Role_rights where RoleId=@RoleId",
                    new { @RoleId = RoleID }, commandType: CommandType.Text).ToList();
                    if(result3.Any(m=>m==item.MenuCap_ID))
                    {
                        item.isNewlyEnrolled = true;
                    }

                }
                result.TableMenu = result2;
                //var result3 = con.Query<TableMenuModel>("select MenuId from User_Role_rights where RoleId@RoleId",
                //   new { @RoleId = RoleID }, commandType: CommandType.Text).ToList();
                
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult EditUserRole1(UserRole1 m)
        {
            if (m == null || m.TableMenu == null || string.IsNullOrEmpty(m.RoleId))
            {
                return View(m);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("Insert_into_User_Role_Master"
                    , new
                    {

                        m.UserType,
                        m.UserRole,
                        m.IsActive,
                        m.CreatedBy,
                        Action = "edit",
                        ModifiedBy = "",
                        m.RoleId,
                        m.Comments
                    },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == 2)
                {
                    foreach (var item in m.TableMenu)
                    {
                        if (item.isNewlyEnrolled == true)
                        {

                            var result1 = con.Query<string>("Insert_into_User_Role_rights", new { RoleId = m.RoleId, MenuID = item.MenuCap_ID, ParentMenu_id = item.ParentMenuID, Action = "Edit" },
                                commandType: CommandType.StoredProcedure);

                        }
                        else
                        {
                            var result1 = con.Execute("Delete from User_Role_rights where RoleId=@RoleId and MenuID=@MenuID", new { RoleId = m.RoleId, MenuID = item.MenuCap_ID },
                                commandType: CommandType.Text);
                        }

                    }
                }
            }

            return RedirectToAction("CreateRole1");
        }
        public ActionResult AddNewRole1()
        {
            var name = User.Identity.Name;
            var result = "";
            if (name != null)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                     result = con.Query<string>("select UserType from User_Type where UserTypeId in (select UserType from Create_User_Master where UserName=@UserName)", new { @UserName = name },
                       commandType: CommandType.Text).FirstOrDefault();
                    if (result == "Admin")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeAdmin(), "Value", "Text");

                    }
                    else if (result == "Client")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeClient(), "Value", "Text");
                    }
                    else if (result == "Service Center")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeServiceCenter(), "Value", "Text");
                    }
                    else if (result == "Service Provider")
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserTypeService_Provider(), "Value", "Text");
                    }
                    else if (result == "Customer")
                    {
                        ViewBag.UserType = new SelectList(Enumerable.Empty<SelectListItem>());
                    }
                    else
                    {
                        ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
                    }
                }
            }
          
            var result1 = new UserRole1();
            result1.TableMenu = new System.Collections.Generic.List<TableMenuModel>();
            return View(result1);
        }
        [HttpPost]
        public ActionResult AddNewRole1(UserRole1 m)
        {
            if (ModelState.IsValid)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Insert_into_User_Role_Master",
                        new
                        {
                            m.UserType,
                            m.UserRole,
                            m.IsActive,
                            m.CreatedBy,
                            Action = "add",
                            ModifiedBy = "",
                            m.RoleId,
                            m.Comments
                        },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        foreach (var item in m.TableMenu)
                        {
                            var RoleId = con.Query<int>("select RoleId from User_Role_Master where UserRole=@UserRole", new { @UserRole = m.UserRole }, commandType: CommandType.Text).FirstOrDefault();
                            if (item.isNewlyEnrolled == true)
                            {
                                var result1 = con.Execute("insert into User_Role_rights values(@RoleId,@MenuId,@ParentMenu_id)", new { RoleId = RoleId, MenuID = item.SubMenuName, ParentMenu_id = item.MenuCap_ID },
                            commandType: CommandType.Text);
                            }
                            

                        }
                        TempData["Message"] = " User Role Added Successfully";
                    }
                    else
                    {
                        TempData["Message"] = "Something went wrong";
                    }
                }
            }
            return RedirectToAction("CreateRole1");
        }

        #endregion

        #region User1
        public ActionResult CreateUser1()
        {
            return View();
        }
        public ActionResult UserTable1()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<CreateUserModel>("Proc_create_user_master_List", null,
                   commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        public ActionResult AddNewUser1()
        {
            ViewBag.UserRole = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
            var result = new User1();
            result.TableMenu = new System.Collections.Generic.List<TableMenuModel>();
            return View(result);
        }
        [HttpPost]
        public ActionResult AddNewUser1(User1 m)
        {

            if (ModelState.IsValid)
            {
                Random r = new Random();
                int randomNumber = r.Next(999, 10000);
                string Password = randomNumber.ToString();
                string Passwrd = "";
                var mpc = new Email_send_code();
                Type type = mpc.GetType();

                var Status = (int)type.InvokeMember("sendmail_update",
                                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                                        BindingFlags.NonPublic, null, mpc,
                                        new object[] { m.Email_Address, Password, m.UserName });
                if (Status == 1)
                {
                    Passwrd = TogoFogo.Encrypt_Decript_Code.encrypt_decrypt.Encrypt(Password, true);
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<string>("Insert_Into_Create_User_Master",
                            new
                            {
                                m.UserRole,
                                m.ID,
                                m.UserType,
                                m.Name,
                                m.Address,
                                m.Mobile,
                                m.Email_Address,
                                m.UserName,
                                m.ServiceProvider,
                                m.ServiceProviderType,
                                TrcId = m.TRC,
                                Password = Passwrd,
                                m.IsActive,
                                m.CreatedBy,
                                m.ModifiedBy,
                                Action = "add",
                                m.EmployeeId ,
                                m.AddressType,
                                m.Pincode,
                                m.City,
                                m.State,
                                m.Comments

                            },
                            commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result != null)
                        {

                            foreach (var item in m.TableMenu)
                            {
                                var USERID = con.Query<int>("select Id from Create_User_Master where UserName=@User", new { @User = m.UserName }, commandType: CommandType.Text).FirstOrDefault();

                                if (item.isNewlyEnrolled == true)
                                {

                                    var AddRights = con.Execute("insert into user_rights_Test values(@UserId,@MenuId,@ParentMenu_id)", new { UserId = USERID, MenuID = item.SubMenuName, ParentMenu_id = item.MenuCap_ID },
                                            commandType: CommandType.Text);
                                }
                            }
                            TempData["Message"] = result;
                        }
                    }
                }

            }

            return RedirectToAction("CreateUser1");
        }
        public ActionResult EditUser1(int? UserId)
        {
            //ViewBag.UserRole = new SelectList(Enumerable.Empty<SelectListItem>());
            
            ViewBag.UserType = new SelectList(dropdown.BindUserType(), "Value", "Text");
            var result = new User1();
           
            using (var con = new SqlConnection(_connectionString))
            {
                 result = con.Query<User1>("select * from create_user_master where Id=@ID", new { @ID =UserId },
                    commandType: CommandType.Text).FirstOrDefault();
                result.UserRole = result.RoleId;
                result.ID = UserId.ToString();
                ViewBag.UserRole = new SelectList(dropdown.BindUserRoleBYUserType(result.UserType), "Value", "Text");
                var result2 = con.Query<TableMenuModel>("select * from menuTable where MenuCap_ID in (select MenuId from User_Role_rights where RoleId=@RoleId ) ",
                   new { @RoleId = result.RoleId }, commandType: CommandType.Text).ToList();
                var menu = "";
                foreach (var item in result2)
                {
                    var parent = item.ParentMenuID;
                    menu = item.Menu_Name;
                    var ParentMenuName = con.Query<string>(" select Menu_Name from menuTable where MenuCap_ID=@MenuCap_ID", new { MenuCap_ID = parent }, commandType: CommandType.Text).FirstOrDefault();
                    item.Menu_Name = ParentMenuName;
                    item.SubMenuName = menu;
                    var result3 = con.Query<string>("select MenuId from user_rights_test where UserId=@UserId",
                     new { @UserId = UserId }, commandType: CommandType.Text).ToList();
                    if (result3.Any(m => m == item.MenuCap_ID))
                    {
                        item.isNewlyEnrolled = true;
                    }

                }
                result.TableMenu = result2;
               
            }

            //result.TableMenu = new System.Collections.Generic.List<TableMenuModel>();
            return View(result);
        }
        [HttpPost]
        public ActionResult EditUser1(User1 m)
        {
            if (m == null || m.TableMenu == null || string.IsNullOrEmpty(m.ID))
            {
                return View(m);
            }
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<string>("Insert_Into_Create_User_Master",
                             new
                             {
                                 m.UserRole,
                                 m.ID,
                                 m.UserType,
                                 m.Name,
                                 m.Address,
                                 m.Mobile,
                                 m.Email_Address,
                                 m.UserName,
                                 m.ServiceProvider,
                                 m.ServiceProviderType,
                                 TrcId = m.TRC,
                                 Password = "",
                                 m.IsActive,
                                 m.CreatedBy,
                                 m.ModifiedBy,
                                 Action = "edit",
                                 m.EmployeeId,
                                 m.AddressType,
                                 m.Pincode,
                                 m.City,
                                 m.State,
                                 m.Comments

                             },
                     commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result == "Updated")
                {
                    foreach (var item in m.TableMenu)
                    {
                        if (item.isNewlyEnrolled == true)
                        {

                            var result1 = con.Query<string>("Insert_into_user_rights_Test", new { UserId = m.ID, MenuID = item.MenuCap_ID, ParentMenu_id = item.ParentMenuID, Action = "Edit" },
                                commandType: CommandType.StoredProcedure);

                        }
                        else
                        {
                            var result1 = con.Execute("Delete from user_rights_Test where UserId=@UserId and MenuID=@MenuID", new { UserId = m.ID, MenuID = item.MenuCap_ID },
                                commandType: CommandType.Text);
                        }

                    }
                }
            }

            return RedirectToAction("CreateUser1");
        }

        #endregion

        #region WebsiteProblemList
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
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new { m.Model_Id,Problem_Id=m.Problem,m.Market_Price,m.estimated_Price,m.Min_Price,m.Max_Price,action="Add"},
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
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<int>("sp_insert_into_Probles_VS_Price_matrix", new { m.Model_Id, Problem_Id = m.Problem, m.Market_Price, m.estimated_Price, m.Min_Price, m.Max_Price, action = "edit" },
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
