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
using System.Threading.Tasks;

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
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageCourierModel>("GETCourierMasterData", null, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }
        }
        public async Task<ActionResult> Create()
        {
            ManageCourierModel courierModel = new ManageCourierModel();
            courierModel.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
            courierModel.StateList = new SelectList(Enumerable.Empty<SelectListItem>());
            courierModel.CityList = new SelectList(Enumerable.Empty<SelectListItem>());
            courierModel.PincodeList = new SelectList(Enumerable.Empty<SelectListItem>());
            courierModel.ApplicableTaxTypeList = new SelectList(await CommonModel.GetApplicationTaxType(), "Value", "Text");
            courierModel.PersonAddressTypeList = new SelectList(CommonModel.GetAddressTypes(),"Value","Text");
            courierModel.AWBNumberUsedList = new SelectList(await CommonModel.GetAWBNumberUsedTypes(),"Value","Text");
            courierModel.AgreementSignupList = new SelectList(await CommonModel.GetAgreementSignup(),"Value","Text");
            courierModel.LegalDocumentVerificationList = new SelectList(await CommonModel.GetLegalDocumentVerification(), "Value", "Text");
            return View(courierModel);
        }
        [HttpPost]
        public ActionResult Create(ManageCourierModel model)
        {
            try
            {
                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
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
                                Action = "I",
                                User = ""
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 1)
                        {
                            TempData["AddCourier"] = "Successfully Added";
                        }
                    }
                    else
                    {
                        return View(model);
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

        public async Task<ActionResult> Edit(int? courierId)
        {

            using (var con = new SqlConnection(_connectionString))
            {    
                var result = con.Query<ManageCourierModel>("SELECT * from Courier_Master WHERE CourierId=@CourierId", new { CourierId = courierId },
                commandType: CommandType.Text).FirstOrDefault();               
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
                    result.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                    result.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                    result.CityList = new SelectList(dropdown.BindLocation(), "Value", "Text");
                    result.PincodeList = new SelectList(Enumerable.Empty<SelectListItem>());
                    result.ApplicableTaxTypeList = new SelectList(await CommonModel.GetApplicationTaxType(), "Value", "Text");
                    result.PersonAddressTypeList = new SelectList(CommonModel.GetAddressTypes(), "Value", "Text");
                    result.AWBNumberUsedList = new SelectList(await CommonModel.GetAWBNumberUsedTypes(), "Value", "Text");
                    result.AgreementSignupList = new SelectList(await CommonModel.GetAgreementSignup(), "Value", "Text");
                    result.LegalDocumentVerificationList = new SelectList(await CommonModel.GetLegalDocumentVerification(), "Value", "Text");
                    string[] parts = volume.ToString().Split('-');
                    result.Volumn1 = parts[0];
                    result.Volumn2 = parts[1];
                    string weight = result.WeightRange.ToString();
                    string[] parts1 = weight.ToString().Split('-');
                    result.WeightRange1 = parts[0];
                    result.WeightRange2 = parts[1];
                    result.AgreementEndDate = result.AgreementEndDate;

                }

                return PartialView("Edit", result);
            }
        }
        [HttpPost]
        public ActionResult Edit(ManageCourierModel model)
        {
            try
            {
                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
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
                                Action = "U",
                                User = ""
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        if (result == 2)
                        {
                            TempData["AddCourier"] = "Updated Successfully";
                        }
                    }
                    else
                    {
                        return View(model);
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