using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dapper;
using TogoFogo.Models;
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class ManageSACCodesController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
       



        // GET: ManageSACCodes
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.GST_HSN_SAC_Codes)]
        public ActionResult SacCodes()
        {
            ManageSACCodesViewModel Ms = new ManageSACCodesViewModel();
            Ms.Rights = (UserActionRights)HttpContext.Items["ActionsRights"];


            if (TempData["Message"] != null)
            {
                Ms.Message = TempData["Message"].ToString();

            }
            return View(Ms);
        }

        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.GST_HSN_SAC_Codes)]
        public ActionResult AddSacCodes()
        {
           var SessionModel = Session["User"] as SessionModel;
            SacCodesModel sm = new SacCodesModel
            {
                CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                StateList = new SelectList(Enumerable.Empty<SelectList>()),
                GstList = new SelectList(dropdown.BindGst(null), "Value", "Text"),
                AplicationTaxTypeList = new SelectList(CommonModel.GetApplicationTax(), "Value", "Text"),
            };
            return View(sm);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create}, (int)MenuCode.GST_HSN_SAC_Codes)]
        [HttpPost]
        public ActionResult AddSacCodes(SacCodesModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                      var SessionModel = Session["User"] as SessionModel;
                        var result = con.Query<int>("Add_Edit_Delete_SACCodes",
                            new
                            {
                                model.SacCodesId,
                                model.CountryId,
                                model.StateId,
                                model.Applicable_Tax,
                                model.GstCategoryId,
                                model.GstHeading,
                                model.Gst_HSN_Code,
                                model.CTH_Number,
                                model.SAC,
                                //model.Product_Sale_Range,
                                Product_Sale_Range = model.Product_Sale_From + "-" + model.Product_Sale_TO,
                                model.CGST,
                                model.SGST_UTGST,
                                model.IGST,
                                model.GstProductCat,
                                model.GstProductSubCat,
                                model.Description_Of_Goods,
                                model.IsActive,
                                model.Comments,
                                User = SessionModel.UserId,
                                SessionModel.CompanyId,
                                ACTION = "I"
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["Message"] = "Successfully Added";

                        }
                        else
                        {
                            TempData["Message"] = "Sorry please try again";
                        }
                    }
                    return RedirectToAction("SacCodes");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("SacCodes");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.GST_HSN_SAC_Codes)]
        public ActionResult SacCodesTable()
        {
           
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
                var result = con.Query<SacCodesModel>("Get_SacCodes", new { companyId = SessionModel.CompanyId }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.GST_HSN_SAC_Codes)]
        public async Task<ActionResult> EditSacCode(int sacCodeId)
        {
          

         
            using (var con = new SqlConnection(_connectionString))
            {
                var SessionModel = Session["User"] as SessionModel;
                var result = con.Query<SacCodesModel>("Select * from MstSacCodes Where SacCodesId=@SacCodesId", new { @SacCodesId = sacCodeId },
                    commandType: CommandType.Text).FirstOrDefault();
                result.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                result.StateList = new SelectList(dropdown.BindState(result.CountryId), "Value", "Text");
                result.GstList = new SelectList(dropdown.BindGst(null), "Value", "Text");
                var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
               result.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
                result.InitialHSNCode = result.Gst_HSN_Code;
                if (result != null)
                {

                    if (result.Product_Sale_Range != null)
                    {
                        if (result.Product_Sale_Range.Contains("-"))
                        {
                            string productSale = result.Product_Sale_Range;
                            string[] parts = productSale.ToString().Split('-');
                            result.Product_Sale_From = parts[0];
                            result.Product_Sale_TO = parts[1];
                        }
                        else
                        {
                            result.Product_Sale_From = result.Product_Sale_Range;
                        }

                    }

                }
                return View(result);
            }
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.GST_HSN_SAC_Codes)]
        [HttpPost]
        public ActionResult EditSacCode(SacCodesModel model)
        {
          var SessionModel = Session["User"] as SessionModel;
            try
            {
                if (ModelState.IsValid)
                {

                    using (var con = new SqlConnection(_connectionString))
                    {
                      
                        var result = con.Query<int>("Add_Edit_Delete_SACCodes",
                            new
                            {
                                model.SacCodesId,
                                model.CountryId,
                                model.StateId,
                                model.Applicable_Tax,
                                model.GstCategoryId,
                                model.GstHeading,
                                model.Gst_HSN_Code,
                                model.CTH_Number,
                                model.SAC,
                                //model.Product_Sale_Range,
                                Product_Sale_Range = model.Product_Sale_From + "-" + model.Product_Sale_TO,
                                model.CGST,
                                model.SGST_UTGST,
                                model.IGST,
                                model.GstProductCat,
                                model.GstProductSubCat,
                                model.Description_Of_Goods,
                                model.IsActive,
                                model.Comments,
                                User = SessionModel.UserId,
                                SessionModel.CompanyId,
                                ACTION = "U"
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 2)
                        {
                            TempData["Message"] = "Updated Successfully";

                        }
                        else
                        {
                            TempData["Message"] = "Not Updated Successfully";
                        }
                    }


                    return RedirectToAction("SacCodes");
                 
                }

                else
                {
                 
                    model.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                    model.StateList = new SelectList(dropdown.BindState(model.CountryId), "Value", "Text");
                    model.GstList = new SelectList(dropdown.BindGst(null), "Value", "Text");
                    return View(model);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


       public JsonResult IsHSNCodeAlreadyExist(string Gst_HSN_Code, string InitialHSNCode)
        {
            try
            {
                if (Gst_HSN_Code == InitialHSNCode)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                return IsExist(Gst_HSN_Code) ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }


        }
        public bool IsExist(string Hsncode)
        {

            using (var con = new SqlConnection(_connectionString))
            {

                var result = con.Query<int>("SELECT count(*) from MstSacCodes where Gst_HSN_Code like " + Hsncode + " ").SingleOrDefault();
                if (result > 0)
                {

                    return false;
                }
                else
                {
                    return true;
                }

            }

        }
    }
}