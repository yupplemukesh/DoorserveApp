using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace TogoFogo.Controllers
{
    public class ManageProviderController : Controller
    {
        private readonly string _connectionString =
           ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Path.GetFileNameWithoutExtension(fileFullName);
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
        // GET: Default
        public ActionResult ManageProvider()
        {
            ViewBag.Supported_Category = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProcessName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProviderCity = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ProviderState = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.COUNTRY = new SelectList(Enumerable.Empty<SelectListItem>());
            //ViewBag.ProcessName = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SupportedCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SubCategory = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.GstCategory = new SelectList(Enumerable.Empty<SelectListItem>());
           
            if (TempData["AddProvider"] != null)
            {
                ViewBag.AddProvider = TempData["AddProvider"].ToString();
                
            }
            if (TempData["EditProvider"] != null)
            {
               
                ViewBag.EditProvider = TempData["EditProvider"].ToString();
            }

            return View();
        }
        public ActionResult AddServiceProvider()
        {
           
            ViewBag.ProviderCity = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.GstCategory = new SelectList(dropdown.BindGst(), "Value", "Text");
            ViewBag.ProviderState = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
            //ViewBag.ProcessName = new SelectList(dropdown.BindProduct(), "Value", "Text");
            ViewBag.SupportedCategory = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");
            ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");

            return View();
        }
        [HttpPost]
        public ActionResult AddServiceProvider(ManageServiceProviderModel model)
        {
            try
            {
                model.User = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
                if (model.GST_No_File1 != null)
                {
                    model.GST_No_File = SaveImageFile(model.GST_No_File1);
                }
                if (model.Pancardno_File1 != null)
                {
                    model.Pancardno_File = SaveImageFile(model.Pancardno_File1);
                }
                if (model.UserPanNo_File1 != null)
                {
                    model.UserPanNo_File = SaveImageFile(model.UserPanNo_File1);
                }
                if (model.VoterIdCard_File1 != null)
                {
                    model.VoterIdCard_File = SaveImageFile(model.VoterIdCard_File1);
                }
                if (model.AadharCard_File1 != null)
                {
                    model.AadharCard_File = SaveImageFile(model.AadharCard_File1);
                }
                if (model.CancelledChequeFile1 != null)
                {
                    model.CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1);
                }
                if (model.CancelledChequeFile1 != null)
                {
                    model.CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1);
                }
                var finalValue = "";
                if (model.SupportedCategory != null)
                {
                   
                    finalValue = string.Join(",", model.SupportedCategory);

                }
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_ServiceProvider",
                        new
                        {
                            model.ProviderId,
                            model.ProcessName,
                            model.ProviderCode,
                            model.ProviderName,
                           SupportedCategory=finalValue,
                            model.OrganisationName,
                            model.Org_Code,
                            model.Org_IEC_No,
                            model.StatutoryType,
                            model.NumberOfCenter,
                            model.ProviderState,
                            model.ProviderCity,
                            model.FirstName,
                            model.LastName,
                            model.Mobile_No,
                            model.Email,
                            model.TaxType,
                            model.GST_No,
                            model.GST_No_File,
                            model.PancardNo,
                            model.Pancardno_File,
                            model.IsUser,
                            model.UserPanNo,
                            model.UserPanNo_File,
                            model.VoterIdCard,
                            model.VoterIdCard_File,
                            model.AadhaarNo,
                            model.AadharCard_File,
                            model.BankName,
                            model.BankAccNo,
                            model.CompanyNameAtBank,
                            model.IFSC_Code,
                            model.BankBranch,
                            model.CancelledChequeFile,
                            model.IsActive,
                            model.Comments,
                           model.User,
                            Action = "add",
                            model.AddressType,
                            model.Address,
                            model.Locality,
                            model.NearByLoc,
                            model.Pincode,
                            model.Country,
                            GstCategoryId= model.GstCategory
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 1)
                    {
                        TempData["AddProvider"] = "Successfully Added";
                    }
                    else
                    {
                        TempData["AddProvider"] = "Service Provider Already Exist";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageProvider");
        }
        public ActionResult ServiceProviderTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageServiceProviderModel>("GetProviderDetails", new { }, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }
        }
        public ActionResult EditServiceProvider(int ProviderId)
        {

            if (ProviderId == 0)
            {

                //ViewBag.ProcessName = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ProviderCity = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ProviderState = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
                //ViewBag.ProcessName = new SelectList(dropdown.BindProduct(), "Value", "Text");
                ViewBag.SupportedCategory = new SelectList(dropdown.BindCategory(), "Value", "Text");
                ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                ViewBag.GstCategory = new SelectList(dropdown.BindGst(), "Value", "Text");
                
            }
            else
            {
                //ViewBag.ProcessName = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ProviderCity = new SelectList(dropdown.BindLocation(), "Value", "Text");
                ViewBag.ProviderState = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
                //ViewBag.ProcessName = new SelectList(dropdown.BindProduct(), "Value", "Text");
                ViewBag.Supported_Category = new SelectList(dropdown.BindCategorySelectpicker(), "Value", "Text");
                ViewBag.SubCategory = new SelectList(dropdown.BindSubCategory(), "Value", "Text");
                ViewBag.ProviderState = new SelectList(dropdown.BindState(), "Value", "Text");
                ViewBag.GstCategory = new SelectList(dropdown.BindGst(), "Value", "Text");
                

                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<ManageServiceProviderModel>("select * from MstServiceProvider where ProviderId=@ProviderId", new { ProviderId = ProviderId },
                        commandType: CommandType.Text).FirstOrDefault();
                   
                    result.GstCategory = result.GstCategoryId;
                    if(!string.IsNullOrEmpty(result.SupportedCategory))
                    result.Supported_Category = result.SupportedCategory.Split(',');
                    return PartialView("EditServiceProvider", result);
                }
            }

           
            return View();
        }
        [HttpPost]
        public ActionResult EditServiceProvider(ManageServiceProviderModel model)
        {
            model.User = (Convert.ToString(Session["User_ID"]) == null ? 0 : Convert.ToInt32(Session["User_ID"]));
            try
            {
                if (model.GST_No_File1 != null)
                {
                    model.GST_No_File = SaveImageFile(model.GST_No_File1);
                }
                 if (model.Pancardno_File1 != null)
                {
                    model.Pancardno_File = SaveImageFile(model.Pancardno_File1);
                }
                 if (model.UserPanNo_File1 != null)
                {
                    model.UserPanNo_File = SaveImageFile(model.UserPanNo_File1);
                }
                 if (model.VoterIdCard_File1 != null)
                {
                    model.VoterIdCard_File = SaveImageFile(model.VoterIdCard_File1);
                }
                 if (model.AadharCard_File1 != null)
                {
                    model.AadharCard_File = SaveImageFile(model.AadharCard_File1);
                }
                 if (model.CancelledChequeFile1 != null)
                {
                    model.CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1);
                }
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_ServiceProvider",
                        new
                        {
                            model.ProviderId,
                            model.ProcessName,
                            model.ProviderCode,
                            model.ProviderName,
                            model.SupportedCategory,
                            model.OrganisationName,
                            model.Org_Code,
                            model.Org_IEC_No,
                            model.StatutoryType,
                            model.NumberOfCenter,
                            model.ProviderState,
                            model.ProviderCity,
                            model.FirstName,
                            model.LastName,
                            model.Mobile_No,
                            model.Email,
                            model.TaxType,
                            model.GST_No,
                            model.GST_No_File,
                            model.PancardNo,
                            model.Pancardno_File,
                            model.IsUser,
                            model.UserPanNo,
                            model.UserPanNo_File,
                            model.VoterIdCard,
                            model.VoterIdCard_File,
                            model.AadhaarNo,
                            model.AadharCard_File,
                            model.BankName,
                            model.BankAccNo,
                            model.CompanyNameAtBank,
                            model.IFSC_Code,
                            model.BankBranch,
                            model.CancelledChequeFile,
                            model.IsActive,
                            model.Comments,
                            model.User,
                            Action = "edit",
                            model.AddressType,
                            model.Address,
                            model.Locality,
                            model.NearByLoc,
                            model.Pincode,
                            model.Country,
                            GstCategoryId = model.GstCategory
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["EditProvider"] = "Successfully Updated";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageProvider");
        }
    }
}