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

namespace TogoFogo.Controllers
{
    public class ManageSACCodesController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
       
      
        // GET: ManageSACCodes
        public ActionResult SacCodes()
        {
            /*ViewBag.CountryId = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.StateId = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.GstCategoryId = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.Gst_HSNCode_Id = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.CTH_NumberId = new SelectList(Enumerable.Empty<SelectList>());
            ViewBag.SACId = new SelectList(Enumerable.Empty<SelectList>());

            sm.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            sm.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
            sm.GstList = new SelectList(dropdown.BindGst(), "Value", "Text");
            sm.GstHsnCodeList = new SelectList(dropdown.BindGstHsnCode(), "Value", "Text");*/
        
           
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();

            }
            return View();
        }

        
        public ActionResult AddSacCodes()
        {
            SacCodesModel sm = new SacCodesModel();
            
            sm.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            sm.StateList = new SelectList(Enumerable.Empty<SelectList>());
            sm.GstList = new SelectList(dropdown.BindGst(), "Value", "Text");
           
            sm.AplicationTaxTypeList = new SelectList(CommonModel.GetApplicationTax(), "Value", "Text");
            return View(sm);
        }
        [HttpPost]
        public ActionResult AddSacCodes(SacCodesModel model)
        {
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
                                model.Applicable_Tax_Type,
                                model.GstCategoryId,
                                model.GstHeading,
                                model.Gst_HSN_Code,
                                model.CTH_Number,
                                model.SAC,
                                model.Product_Sale_Range,
                                model.CGST,
                                model.SGST_UTGST,
                                model.IGST,
                                model.GstProductCat,
                                model.GstProductSubCat,
                                model.Description_Of_Goods,
                                model.IsActive,
                                model.Comments,
                                User = Convert.ToInt32(Session["User_ID"]),
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
        public ActionResult SacCodesTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                //var result = con.Query<SacCodesModel>("Select * from MstSacCodes", new { }, commandType: CommandType.Text).ToList();
               var result= con.Query < SacCodesModel >("select c.Cnty_Name,s.St_Name,m.SacCodesId, m.Applicable_Tax_Type,m.GstCategoryid,m.GstHeading,m.Gst_HSN_Code,m.CTH_Number,m.SAC,m.Product_Sale_Range, m.CGST, m.SGST_UTGST, m.IGST, m.GstProductCat, m.GstProductSubCat, m.Description_Of_Goods, m.IsActive, m.Comments, m.CreatedDate, m.ModifyDate, u.Username Cby,u1.Username Mby from MstSacCodes m join create_User_Master u on u.id = m.CreatedBy left outer join create_User_Master u1 on m.ModifyBy = u1.id left outer join MstCountry c on c.Cnty_ID = m.CountryId left outer join mststate s on s.St_ID = m.StateId", new { }, commandType: CommandType.Text).ToList();
                return View(result);
            }
        }

        public async Task<ActionResult> EditSacCode(int sacCodeId)
        {
          

         
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<SacCodesModel>("Select * from MstSacCodes Where SacCodesId=@SacCodesId", new { @SacCodesId = sacCodeId },
                    commandType: CommandType.Text).FirstOrDefault();
                result.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                result.StateList = new SelectList(dropdown.BindState(result.CountryId), "Value", "Text");
                result.GstList = new SelectList(dropdown.BindGst(), "Value", "Text");
                var applicationTaxTypeList = await CommonModel.GetApplicationTaxType();
               result.AplicationTaxTypeList = new SelectList(applicationTaxTypeList, "Value", "Text");
              
                return View(result);
            }
        }
        [HttpPost]
        public ActionResult EditSacCode(SacCodesModel model)
        {
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
                                model.Applicable_Tax_Type,
                                model.GstCategoryId,
                                model.GstHeading,
                                model.Gst_HSN_Code,
                                model.CTH_Number,
                                model.SAC,
                                model.Product_Sale_Range,
                                model.CGST,
                                model.SGST_UTGST,
                                model.IGST,
                                model.GstProductCat,
                                model.GstProductSubCat,
                                model.Description_Of_Goods,
                                model.IsActive,
                                model.Comments,
                                User = Convert.ToInt32(Session["User_ID"]),
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
                    model.GstList = new SelectList(dropdown.BindGst(), "Value", "Text");
                    return View(model);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}