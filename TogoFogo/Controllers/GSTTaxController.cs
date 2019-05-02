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
    public class GSTTaxController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        private SessionModel user;
        // GET: GSTTax
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.GST_Tax)]
        public ActionResult Gst()
        {
            
            return View();
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.GST_Tax)]
        public ActionResult AddGst()
        {
            user = Session["User"] as SessionModel;

            GstTaxModel gm = new GstTaxModel();
            gm.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            gm.StateList = new SelectList(Enumerable.Empty<SelectList>());
            gm.GstcategoryList = new SelectList(dropdown.BindGst(user.CompanyId), "Value", "Text");
            gm.DeviceCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
            gm.DeviceSubCategoryList = new SelectList(Enumerable.Empty<SelectList>());
            gm.ApplicableTaxTypeList = new SelectList(CommonModel.GetApplicationTax(), "Value", "Text");
            gm.GstHSNCodeList = new SelectList(dropdown.BindGstHsnCode(),"Value","Text");
            gm.SACList = new SelectList(CommonModel.SAC_NumberList(), "Text", "Text");
            gm.CTHNumberList = new SelectList(CommonModel.CTH_NumberList(), "Text", "Text");
            return View(gm);
        }

        [HttpPost]
        public ActionResult AddGst(GstTaxModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        var result = con.Query<int>("Add_Edit_Delete_GstTax",
                            new
                            {
                                model.GstTaxId,
                                model.CountryId,
                                //StateId=model.State,
                                model.StateId,
                                model.Applicable_Tax,
                                model.GstCategoryId,
                                model.Device_Cat,
                                model.Device_SubCat,
                                model.GstHeading,
                                model.Gst_HSNCode_Id,
                                model.CTH_NumberId,
                                model.SACId,
                                Product_Sale_Range=model.Product_Sale_From+"-"+model.Product_Sale_TO,                                
                                model.CGST,
                                model.SGST,
                                model.IGST,
                                model.Product_Cat,
                                model.Product_SubCat,
                                model.Description_Goods,
                                model.Gst_Applicable_date,
                                model.IsActive,
                                model.Comments,
                                User=Convert.ToInt32(Session["User_Id"]),
                                Action="I"
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var response = new ResponseModel();
                        if (result == 1)
                        {
                            response.IsSuccess = true;
                            response.Response = "Submitted Successfully";
                            TempData["response"] = response;

                        }
                        else
                        {
                            response.IsSuccess = true;
                            response.Response = "Something Went Wrong";
                            TempData["response"] = response;

                        }
                    }

                    //return View(model);
                    return RedirectToAction("Gst");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("Gst");
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.View}, (int)MenuCode.GST_Tax)]
        public ActionResult GstTaxtable()
        {
            var objGstTaxModel = new List<GstTaxModel>();
            using (var con = new SqlConnection(_connectionString))
            {
                objGstTaxModel= con.Query<GstTaxModel>("Get_GstTax", new { }, commandType: CommandType.StoredProcedure).ToList();
              
            }         
            return View(objGstTaxModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.GST_Tax)]
        public ActionResult EditGstTax(int Gsttaxid)
        {          
           
            using (var con = new SqlConnection(_connectionString))
            {
                user = Session["User"] as SessionModel;
                var result2 = con.Query<GstTaxModel>("SELECT * from MstGstTax Where GstTaxId=@GstTaxId", new { @GstTaxId = Gsttaxid },
                commandType: CommandType.Text).FirstOrDefault();
                result2.SACList = new SelectList(CommonModel.SAC_NumberList(), "Text", "Text");
                result2.CTHNumberList = new SelectList(CommonModel.CTH_NumberList(), "Text", "Text");
                result2.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                result2.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                result2.GstcategoryList = new SelectList(dropdown.BindGst(user.CompanyId), "Value", "Text");
                result2.DeviceCategoryList = new SelectList(dropdown.BindCategory(user.CompanyId), "Value", "Text");
                result2.DeviceSubCategoryList = new SelectList(dropdown.BindSubCategory(result2.Device_Cat), "Value", "Text");
                result2.ApplicableTaxTypeList = new SelectList(CommonModel.GetApplicationTax(),"Value","Text");
                result2.GstHSNCodeList = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");
                if (result2 != null)
                {

                    if (result2.Product_Sale_Range != null)
                    {
                        if (result2.Product_Sale_Range.Contains("-"))
                        {
                            string productSale = result2.Product_Sale_Range;
                            string[] parts = productSale.ToString().Split('-');
                            result2.Product_Sale_From = parts[0];
                            result2.Product_Sale_TO = parts[1];
                        }
                        else
                        {
                            result2.Product_Sale_From = result2.Product_Sale_Range;
                        }

                    }

                }
                return View(result2);
            }

            
             }
       
         [HttpPost]
         public ActionResult EditGstTax(GstTaxModel model)
         {
             try
             {
                 if (ModelState.IsValid)
                 {
                     using (var con = new SqlConnection(_connectionString))
                     {
                         var result = con.Query<int>("Add_Edit_Delete_GstTax",
                             new
                             {
                                 model.GstTaxId,
                                 model.CountryId,
                                // StateId=model.State,
                                   model.StateId,
                                 model.Applicable_Tax,
                                 model.GstCategoryId,
                                 model.Device_Cat,
                                 model.Device_SubCat,
                                 model.GstHeading,
                                 model.Gst_HSNCode_Id,
                                 model.CTH_NumberId,
                                 model.SACId,
                                 Product_Sale_Range = model.Product_Sale_From + "-" + model.Product_Sale_TO,
                                 model.CGST,
                                 model.SGST,
                                 model.IGST,
                                 model.Product_Cat,
                                 model.Product_SubCat,
                                 model.Description_Goods,
                                 model.Gst_Applicable_date,
                                 model.IsActive,
                                 model.Comments,
                                 User = Convert.ToInt32(Session["User_Id"]),
                                 Action = "U"
                             }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                        var response = new ResponseModel();
                        if (result == 2)
                        {
                            response.IsSuccess = true;
                            response.Response = "Updated Successfully";
                            TempData["response"] = response;

                        }
                        else
                        {
                            response.IsSuccess = true;
                            response.Response = "Something Went Wrong";
                            TempData["response"] = response;

                        }                        
                     }
                    //return View(model);
                    return RedirectToAction("Gst");
                }
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 throw;
             }
             return RedirectToAction("Gst");
         }
    }
}