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

namespace TogoFogo.Controllers
{
    public class GSTTaxController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // GET: GSTTax
        public ActionResult Gst()
        {
           /* ViewBag.CountryId = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.State = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.GstCategoryId = new SelectList(dropdown.BindGst(), "Value", "Text");
            ViewBag.Device_Cat = new SelectList(dropdown.BindCategory(), "Value", "Text");
            ViewBag.Device_SubCat = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.Gst_HSNCode_Id = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.CTH_NumberId = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.SACId = new SelectList(Enumerable.Empty<SelectList>());*/
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }
        public ActionResult AddGst()
        {
            GstTaxModel gm = new GstTaxModel();
            /* ViewBag.CountryId = new SelectList(dropdown.BindCountry(), "Value", "Text");
             ViewBag.State = new SelectList(Enumerable.Empty<SelectList>());
             ViewBag.GstCategoryId = new SelectList(dropdown.BindGst(), "Value", "Text");
             ViewBag.Device_Cat = new SelectList(dropdown.BindCategory(), "Value", "Text");
             ViewBag.Device_SubCat = new SelectList(Enumerable.Empty<SelectList>());
           
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<string>("Select Distinct Gst_HSN_Code from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                
                ViewBag.Gst_HSNCode_Id = new SelectList(result);
                var cthNumber = con.Query<string>("Select Distinct CTH_Number from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                ViewBag.CTH_NumberId = new SelectList(cthNumber);
                var Sac = con.Query<string>("Select Distinct SAC from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                ViewBag.SACId = new SelectList(Sac);
            }*/

            gm.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            gm.StateList = new SelectList(Enumerable.Empty<SelectList>());
            gm.GstcategoryList = new SelectList(dropdown.BindGst(), "Value", "Text");
            gm.DeviceCategoryList = new SelectList(dropdown.BindCategory(), "Value", "Test");
            gm.DeviceSubCategoryList = new SelectList(Enumerable.Empty<SelectList>());
            gm.ApplicableTaxTypeList = new SelectList(CommonModel.GetApplicationTax());
            gm.GstHSNCodeList = new SelectList(dropdown.BindGstHsnCode(),"Value","Text");
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<string>("select Distinct CTH_Number from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                var result1 = con.Query<string>("select Distinct SAC from MstSacCodes",new { },commandType:CommandType.Text).ToList();
                gm.CTHNumberList = new SelectList(result);
                gm.SACList = new SelectList(result1);
            }
           

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
                                StateId=model.State,
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
                        if (result == 1)
                        {
                            TempData["Message"] = "Submitted Successfully";

                        }
                        else
                        {
                            TempData["Message"] = "Something Went Wrong";
                        }
                    }

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

        public ActionResult GstTaxtable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<GstTaxModel>("Get_GstTax", new { }, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }

        /*public ActionResult EditGstTax(int gsttaxid)
        {
            ViewBag.CountryId = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.State = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.GstCategoryId = new SelectList(dropdown.BindGst(), "Value", "Text");
            ViewBag.Device_Cat = new SelectList(dropdown.BindCategory(), "Value", "Text");

             using (var con = new SqlConnection(_connectionString))
             {
                 var result = con.Query<string>("Select Distinct Gst_HSN_Code from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                 ViewBag.Gst_HSNCode_Id = new SelectList(result);
                 var cthNumber = con.Query<string>("Select Distinct CTH_Number from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                 ViewBag.CTH_NumberId = new SelectList(cthNumber);
                 var Sac = con.Query<string>("Select Distinct SAC from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
                 ViewBag.SACId = new SelectList(Sac);
                 var result2 = con.Query<GstTaxModel>("SELECT * from MstGstTax Where GstTaxId=@GstTaxId", new { @GstTaxId = gsttaxid },
                     commandType: CommandType.Text).FirstOrDefault();
                 ViewBag.Device_SubCat = new SelectList(dropdown.BindSubCategory(Int32.Parse(result2.Device_Cat)),"Value","Text");

                 if (result2 != null)
                 {

                     result2.State = result2.StateId;
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
                 ViewBag.State = new SelectList(dropdown.BindState(Int32.Parse(result2.CountryId)),"Value","Text");
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
                                 StateId=model.State,
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
                         if (result == 2)
                         {
                             TempData["Message"] = "Updated Successfully";

                         }
                         else
                         {
                             TempData["Message"] = "Something Went Wrong";
                         }
                     }
                     return View(model);
                 }
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 throw;
             }
             return RedirectToAction("Gst");
         }*/
    }
}