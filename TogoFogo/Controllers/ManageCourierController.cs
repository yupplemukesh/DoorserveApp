using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using Microsoft.Ajax.Utilities;
using TogoFogo.Models;

namespace TogoFogo.Controllers
{
    public class ManageCourierController : Controller
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
        // GET: ManageCourier
        public ActionResult ManageCourier()
        {
            if (TempData["AddCourier"] != null)
            {
                ViewBag.AddCourier = TempData["AddCourier"].ToString();

            }
            ViewBag.COUNTRY = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.StateDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CityDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            //ViewBag.PinCodeDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCountry = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonState = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCity = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SC_CountryDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SC_PincodeDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonStateDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCityDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCountryDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }
        public ActionResult AddCourier()
        {
            ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.StateDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.CityDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            //ViewBag.PinCodeDropdown = new SelectList(dropdown.BindPinCode(), "Value", "Text");
            ViewBag.PersonCountry = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.PersonState = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCity = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SC_CountryDropdown = new SelectList(dropdown.BindCountry(), "Value", "Text");
            ViewBag.SC_PincodeDropdown = new SelectList(dropdown.BindPinCode(), "Value", "Text");
            ViewBag.PersonStateDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCityDropdown = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.PersonCountryDropdown = new SelectList(dropdown.BindCountry(), "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult AddCourier(ManageCourierModel model)
        {
            try
            {
                //if (model.UploadedCourierFile1 != null)
                //{
                //    model.UploadedCourierFile = SaveImageFile(model.UploadedCourierFile1);
                //}
                //if (model.UploadedGSTFile1 != null)
                //{
                //    model.UploadedGSTFile = SaveImageFile(model.UploadedGSTFile1);
                //}
                //if (model.UserPANCardFile1 != null)
                //{
                //    model.UserPANCardFile = SaveImageFile(model.UserPANCardFile1);
                //}
                //if (model.UserPANCardFile1 != null)
                //{
                //    model.UserPANCardFile = SaveImageFile(model.UserPANCardFile1);
                //}
                //if (model.VoterIDFile1 != null)
                //{
                //    model.VoterIDFile = SaveImageFile(model.VoterIDFile1);
                //}
                //if (model.AadhaarCardFile1 != null)
                //{
                //    model.AadhaarCardFile = SaveImageFile(model.AadhaarCardFile1);
                //}

                //if (model.PANCardFile1 != null)
                //{
                //    model.PANCardFile = SaveImageFile(model.PANCardFile1);
                //}
                //if (model.AgreementScanFile1 != null)
                //{
                //    model.AgreementScanFile = SaveImageFile(model.AgreementScanFile1);
                //}
                //if (model.CancelledChequeFile1 != null)
                //{
                //    model.CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1);
                //}
                using (var con = new SqlConnection(_connectionString))
                {
                    if (ModelState.IsValid)
                    {
                        var result = con.Query<int>("Add_Modify_Delete_Courier",
                            new
                            {

                                model.CourierId,
                                model.UploadedCourierFile,
                                model.CourierName,
                                model.CourierCode,
                                model.CourierBrandName,
                                model.Priority,
                                model.CourierTAT,
                                model.AWBNumber,
                                model.IsReverse,
                                model.IsAllowPreference,
                                CountryId = model.Country,
                                StateId = model.StateDropdown,
                                CityId = model.CityDropdown,
                                model.CourierCompanyName,
                                model.OrganizationCode,
                                model.StatutoryType,
                                model.ApplicableTaxType,
                                model.GSTNumber,
                                model.UploadedGSTFile,
                                model.PANCardNumber,
                                model.PANCardFile,
                                model.BikeMakeandModel,
                                model.BikeNumber,
                                model.PersonAddresstype,
                                PersonCountry = model.PersonCountryDropdown,
                                PersonState = model.PersonStateDropdown,
                                PersonCity = model.PersonCityDropdown,
                                model.FullAddress,
                                model.Locality,
                                model.NearByLocation,
                                model.Pincode,
                                model.FirstName,
                                model.LastName,
                                model.MobileNumber,
                                model.EmailAddress,
                                model.IsUser,
                                model.UserPANCard,
                                model.UserPANCardFile,
                                model.VoterIDCardNo,
                                model.VoterIDFile,
                                model.AadhaarCardNo,
                                model.AadhaarCardFile,
                                model.ItemType,
                                SC_Country = model.SC_CountryDropdown,
                                SC_Pincode = model.SC_PincodeDropdown,
                                model.Currency,
                                model.ServiceChargeType,
                                model.ValueRange,
                                WeightRange = model.WeightRange1 + "-" + model.WeightRange2,
                                Volume = model.Volumn1 + "-" + model.Volumn2,
                                model.ServiceCharge,
                                model.ApplicableFromDate,
                                model.LegalDocumentVerificationStatus,
                                model.AgreementSignupStatus,
                                model.AgreementStartDate,
                                model.AgreementEndDate,
                                model.AgreementNumber,
                                model.AgreementScanFile,
                                model.BankName,
                                model.BankAccountNumber,
                                model.CompanyNameatBank,
                                model.IFSCCode,
                                model.BankBranch,
                                model.CancelledChequeFile,
                                model.PaymentCycle,
                                model.LuluandSky_Status,
                                model.IsActive,
                                model.Comments,

                                Action = "add",
                                User = ""
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["AddCourier"] = "Successfully Added";
                        }
                    }
                    else
                    {
                        TempData["AddCourier"] = "Please fill the required fields.";
                    }
                }

            }
            catch (Exception e)
            {
                TempData["AddCourier"] = e;

            }

            return RedirectToAction("ManageCourier");
        }

        public ActionResult CourierTable()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageCourierModel>("GETCourierMasterData", null, commandType: CommandType.StoredProcedure).ToList();

                return View(result);
            }

        }

        public ActionResult EditCourier(int? courierId)
        {

            using (var con = new SqlConnection(_connectionString))
            {
                ViewBag.COUNTRY = new SelectList(dropdown.BindCountry(), "Value", "Text");
                ViewBag.StateDropdown = new SelectList(dropdown.BindState(), "Value", "Text");
                ViewBag.CityDropdown = new SelectList(dropdown.BindLocation(), "Value", "Text");
                ViewBag.PinCodeDropdown = new SelectList(dropdown.BindPinCode(), "Value", "Text");
                ViewBag.PersonCountry = new SelectList(dropdown.BindCountry(), "Value", "Text");

                ViewBag.PersonCity = new SelectList(dropdown.BindLocation(), "Value", "Text");
                ViewBag.SC_CountryDropdown = new SelectList(dropdown.BindCountry(), "Value", "Text");
                ViewBag.SC_PincodeDropdown = new SelectList(dropdown.BindPinCode(), "Value", "Text");
                ViewBag.PersonStateDropdown = new SelectList(dropdown.BindState(), "Value", "Text");
                ViewBag.PersonCityDropdown = new SelectList(dropdown.BindLocation(), "Value", "Text");
                ViewBag.PersonCountryDropdown = new SelectList(dropdown.BindCountry(), "Value", "Text");
                var result = con.Query<ManageCourierModel>("SELECT * from Courier_Master WHERE CourierId=@CourierId", new { CourierId = courierId },
                commandType: CommandType.Text).FirstOrDefault();
                ViewBag.PersonState = new SelectList(dropdown.BindState(), "Value", "Text");
                if (result != null)
                {

                    result.Country = result.CountryId;
                    result.StateDropdown = result.StateId;
                    result.CityDropdown = result.CityId;
                    result.PinCodeDropdown = result.Pincode;

                    result.SC_CountryDropdown = result.SC_Country;
                    result.PersonCountryDropdown = result.PersonCountry;
                    result.PersonCityDropdown = result.PersonCity;
                    result.PersonStateDropdown = result.PersonState;
                    string volume = result.Volume.ToString();
                    string[] parts = volume.ToString().Split('-');
                    result.Volumn1 = parts[0];
                    result.Volumn2 = parts[1];
                    string weight = result.WeightRange.ToString();
                    string[] parts1 = weight.ToString().Split('-');
                    result.WeightRange1 = parts[0];
                    result.WeightRange2 = parts[1];
                    result.AgreementEndDate = result.AgreementEndDate;

                }

                return PartialView("EditCourier", result);
            }
        }
        [HttpPost]
        public ActionResult EditCourier(ManageCourierModel model)
        {
            try
            {
                if (model.UploadedCourierFile1 != null)
                {
                    model.UploadedCourierFile = SaveImageFile(model.UploadedCourierFile1);
                }
                if (model.UploadedGSTFile1 != null)
                {
                    model.UploadedGSTFile = SaveImageFile(model.UploadedGSTFile1);
                }
                if (model.UserPANCardFile1 != null)
                {
                    model.UserPANCardFile = SaveImageFile(model.UserPANCardFile1);
                }
                if (model.UserPANCardFile1 != null)
                {
                    model.UserPANCardFile = SaveImageFile(model.UserPANCardFile1);
                }
                if (model.VoterIDFile1 != null)
                {
                    model.VoterIDFile = SaveImageFile(model.VoterIDFile1);
                }
                if (model.AadhaarCardFile1 != null)
                {
                    model.AadhaarCardFile = SaveImageFile(model.AadhaarCardFile1);
                }

                if (model.PANCardFile1 != null)
                {
                    model.PANCardFile = SaveImageFile(model.PANCardFile1);
                }
                if (model.AgreementScanFile1 != null)
                {
                    model.AgreementScanFile = SaveImageFile(model.AgreementScanFile1);
                }
                if (model.CancelledChequeFile1 != null)
                {
                    model.CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1);
                }
                using (var con = new SqlConnection(_connectionString))
                {
                    var result = con.Query<int>("Add_Modify_Delete_Courier",
                        new
                        {

                            model.CourierId,
                            model.UploadedCourierFile,
                            model.CourierName,
                            model.CourierCode,
                            model.CourierBrandName,
                            model.Priority,
                            model.CourierTAT,
                            model.AWBNumber,
                            model.IsReverse,
                            model.IsAllowPreference,
                            CountryId = model.Country,
                            StateId = model.StateDropdown,
                            CityId = model.CityDropdown,
                            model.CourierCompanyName,
                            model.OrganizationCode,
                            model.StatutoryType,
                            model.ApplicableTaxType,
                            model.GSTNumber,
                            model.UploadedGSTFile,
                            model.PANCardNumber,
                            model.PANCardFile,
                            model.BikeMakeandModel,
                            model.BikeNumber,
                            model.PersonAddresstype,
                            PersonCountry = model.PersonCountryDropdown,
                            PersonState = model.PersonStateDropdown,
                            PersonCity = model.PersonCityDropdown,
                            model.FullAddress,
                            model.Locality,
                            model.NearByLocation,
                            model.Pincode,
                            model.FirstName,
                            model.LastName,
                            model.MobileNumber,
                            model.EmailAddress,
                            model.IsUser,
                            model.UserPANCard,
                            model.UserPANCardFile,
                            model.VoterIDCardNo,
                            model.VoterIDFile,
                            model.AadhaarCardNo,
                            model.AadhaarCardFile,
                            model.ItemType,
                            SC_Country = model.SC_CountryDropdown,
                            SC_Pincode = model.SC_PincodeDropdown,
                            model.Currency,
                            model.ServiceChargeType,
                            model.ValueRange,
                            WeightRange = model.WeightRange1 + "-" + model.WeightRange2,
                            Volume = model.Volumn1 + "-" + model.Volumn2,
                            model.ServiceCharge,
                            model.ApplicableFromDate,
                            model.LegalDocumentVerificationStatus,
                            model.AgreementSignupStatus,
                            model.AgreementStartDate,
                            model.AgreementEndDate,
                            model.AgreementNumber,
                            model.AgreementScanFile,
                            model.BankName,
                            model.BankAccountNumber,
                            model.CompanyNameatBank,
                            model.IFSCCode,
                            model.BankBranch,
                            model.CancelledChequeFile,
                            model.PaymentCycle,
                            model.LuluandSky_Status,
                            model.IsActive,
                            model.Comments,

                            Action = "edit",
                            User = ""
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result == 2)
                    {
                        TempData["AddCourier"] = "Updated Successfully";
                    }
                }

            }
            catch (Exception e)
            {

                throw;
            }

            return RedirectToAction("ManageCourier");
        }
    }
}