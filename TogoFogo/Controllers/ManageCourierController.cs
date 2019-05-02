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
using TogoFogo.Permission;

namespace TogoFogo.Controllers
{
    public class ManageCourierController : Controller
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        DropdownBindController dropdown = new DropdownBindController();
        // File Save Code
        private string SaveImageFile(HttpPostedFileBase file,string folder)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages/"+folder);
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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, (int)MenuCode.Manage_Courier)]
        public ActionResult ManageCourier()
        {            
            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.Query<ManageCourierModel>("GETCourierMasterData", null, commandType: CommandType.StoredProcedure).ToList();
                return View(result);
            }           
            
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, (int)MenuCode.Manage_Courier)]
        public async Task<ActionResult> Create()
        {
            DropdownBindController drop = new DropdownBindController();
            ManageCourierModel courierModel = new ManageCourierModel
            {
                CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text"),
                StateList = new SelectList(Enumerable.Empty<SelectListItem>()),
                CityList = new SelectList(Enumerable.Empty<SelectListItem>()),
                PincodeList = new SelectList(Enumerable.Empty<SelectListItem>()),
                ApplicableTaxTypeList = new SelectList(await CommonModel.GetApplicationTaxType(), "Value", "Text"),
                PersonAddressTypeList = new SelectList(CommonModel.GetAddressTypes(), "Value", "Text"),
                AWBNumberUsedList = new SelectList(await CommonModel.GetAWBNumberUsedTypes(), "Value", "Text"),
                AgreementSignupList = new SelectList(await CommonModel.GetAgreementSignup(), "Value", "Text"),
                LegalDocumentVerificationList = new SelectList(await CommonModel.GetLegalDocumentVerification(), "Value", "Text")
            };
            return View(courierModel);
        }
        [HttpPost]
        public ActionResult Create(ManageCourierModel model)
        {
            try
            {          
                           
                    if (ModelState.IsValid)
                    {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        string UploadedCourierFile = SaveImageFile(model.UploadedCourierFilePath, "Courier/Logo");
                        string UploadedGSTFile= SaveImageFile(model.UploadedGSTFilePath, "Courier/Gst");
                        string PANCardFile = SaveImageFile(model.PANCardFilePath, "Courier/PanCards");
                        string UserPANCardFile = SaveImageFile(model.UserPANCardFilePath, "Courier/Pancards");
                        string VoterIDFile = SaveImageFile(model.VoterIDFilePath, "Courier/VoterCards");
                        string AadhaarCardFile = SaveImageFile(model.AadhaarCardFilePath, "Courier/AdharCards");
                        string AgreementScanFile = SaveImageFile(model.AgreementScanFilePath, "Courier/ScanAgreement");
                        string CancelledChequeFile = SaveImageFile(model.CancelledChequeFilePath, "Courier/CancelledCheques");
                        var result = con.Query<int>("Add_Modify_Delete_Courier",
                            new
                            {
                                //Settings
                                model.CourierId,
                                model.CourierName,
                                model.CourierCode,
                                model.CourierBrandName,
                                model.Priority,
                                model.CourierTAT,
                                model.AWBNumber, 
                                CountryId = model.Country,                             
                                StateId = model.StateDropdown,
                                CityId = model.CityDropdown,                            
                                UploadedCourierFile,
                                model.IsReverse,
                                model.IsAllowPreference,
                                //Organisation
                                model.CourierCompanyName,
                                model.OrganizationCode,
                                model.StatutoryType,
                                model.ApplicableTaxType,
                                model.GSTNumber,
                                UploadedGSTFile,
                                model.PANCardNumber,
                                PANCardFile,
                                model.BikeMakeandModel,
                                model.BikeNumber,
                                //Address and Contact Person
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
                                model.UserPANCard,
                                UserPANCardFile,
                                model.VoterIDCardNo,
                                VoterIDFile,
                                model.AadhaarCardNo,
                                AadhaarCardFile,
                                model.IsUser,
                                //Service Charge
                                SC_Country = model.SC_CountryDropdown,
                                SC_Pincode = model.SC_PincodeDropdown,
                                model.Currency,
                                model.ServiceChargeType,
                                model.ValueRange,
                                WeightRange = model.WeightRange1 + "-" + model.WeightRange2,
                                Volume = model.Volumn1 + "-" + model.Volumn2,
                                model.ServiceCharge,
                                model.ApplicableFromDate,
                                model.ItemType,
                                //Agreement
                                model.LegalDocumentVerificationStatus,
                                model.AgreementSignupStatus,
                                model.AgreementStartDate,
                                model.AgreementEndDate,
                                model.AgreementNumber,
                                AgreementScanFile,
                                //Bank Details
                                model.BankName,
                                model.BankAccountNumber,
                                model.CompanyNameatBank,
                                model.IFSCCode,
                                model.BankBranch,
                                CancelledChequeFile,
                                model.PaymentCycle,
                                //Registration
                                model.LuluandSky_Status,
                                model.Comments,
                                model.IsActive,                               
                                User = Convert.ToInt32(Session["User_ID"]),
                                Action = "I",                              
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        var response = new ResponseModel();
                        if (result == 1)
                        {
                            response.IsSuccess = true;
                            response.Response = "New Courier Successfully Added ";
                            TempData["response"] = response;
                            
                        }
                    
                    else
                    {
                            response.IsSuccess = true;
                            response.Response = "Courier Already Exist ";
                            TempData["response"] = response;

                        }

                    }
                    //return View(model);
                    return RedirectToAction("ManageCourier");
                }

            }
            catch (Exception e)
            {
                TempData["AddCourier"] = e;

            }

            return RedirectToAction("ManageCourier");
        }
       
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, (int)MenuCode.Manage_Courier)]
        public async Task<ActionResult> Edit(int? courierId)
        {

            using (var con = new SqlConnection(_connectionString))
            {               
                var folder = "/UploadedImages/Courier/";
                var result = con.Query<ManageCourierModel>("SELECT * from Courier_Master WHERE CourierId=@CourierId", new { CourierId = courierId },
                commandType: CommandType.Text).FirstOrDefault();
                result.UploadedCourierFileUrl= folder+ "Logo/"+result.UploadedCourierFile;
                result.UploadedGSTFileUrl = folder + "Gst/" + result.UploadedGSTFile;
                result.UserPANCardFileUrl = folder + "PanCards/" + result.UserPANCardFile;
                result.VoterIDFileUrl = folder + "VoterCards/" + result.VoterIDFile;
                result.AadhaarCardFileUrl = folder + "AdharCards/" + result.AadhaarCardFile;
                result.AgreementScanFileUrl = folder + "ScanAgreement/" + result.AgreementScanFile;
                result.CancelledChequeFileUrl = folder + "CancelledCheques/" + result.CancelledChequeFile;
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
                    result.SC_PincodeDropdown = result.SC_Pincode;
                    string volume = result.Volume.ToString();
                    result.CountryList = new SelectList(dropdown.BindCountry(), "Value", "Text");
                    result.StateList = new SelectList(dropdown.BindState(), "Value", "Text");
                    result.CityList = new SelectList(dropdown.BindLocation(), "Value", "Text");
                    result.PincodeList = new SelectList(dropdown.BindPincodeListByCountry(Convert.ToInt32(result.SC_Country)), "Value", "Text");
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

                return View(result);
            }
        }
        [HttpPost]
        public ActionResult Edit(ManageCourierModel model)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    using (var con = new SqlConnection(_connectionString))
                {                        
                        var path = Server.MapPath("/UploadedImages/Courier/");
                        
                        if (model.UploadedCourierFilePath != null && model.UploadedCourierFile != null)
                        {


                            if (System.IO.File.Exists(path + "Logo/ " + model.UploadedCourierFile))
                            {
                                System.IO.File.Delete(path + "Logo/" + model.UploadedCourierFile);
                            }                         
                        }
                        if (model.UploadedCourierFilePath != null)
                        {                        

                            model.UploadedCourierFile = SaveImageFile(model.UploadedCourierFilePath, "Courier/Logo");
                        }

                        if (model.UploadedGSTFilePath != null && model.UploadedGSTFile != null)
                        {
                            if (System.IO.File.Exists(path + "Gst/ " + model.UploadedGSTFile))
                            {
                                System.IO.File.Delete(path + "Gst/" + model.UploadedGSTFile);
                            }
                        }
                        if (model.UploadedGSTFilePath != null)
                        {

                            model.UploadedGSTFile = SaveImageFile(model.UploadedGSTFilePath, "Courier/Logo");
                        }


                        if (model.PANCardFilePath != null && model.PANCardFile != null)
                        {
                            if (System.IO.File.Exists(path + "PanCards/ " + model.PANCardFile))
                            { 
                                System.IO.File.Delete(path + "PanCards/" + model.PANCardFile);
                            }
                        }
                        if (model.PANCardFilePath != null)
                        {

                            model.PANCardFile = SaveImageFile(model.PANCardFilePath, "Courier/Logo");
                        }

                        if (model.UserPANCardFilePath != null && model.UserPANCardFile != null)
                        {
                            if (System.IO.File.Exists(path + "PanCards/ " + model.UserPANCardFile))
                            { 
                                System.IO.File.Delete(path + "PanCards/" + model.UserPANCardFile);
                            }
                        }
                        if (model.UserPANCardFilePath != null)
                        {

                            model.UserPANCardFile = SaveImageFile(model.UserPANCardFilePath, "Courier/Logo");
                        }

                        if (model.VoterIDFilePath != null && model.VoterIDFile != null)
                        {
                            if (System.IO.File.Exists(path + "VoterCards/ " + model.VoterIDFile))
                            { 
                                System.IO.File.Delete(path + "VoterCards/" + model.VoterIDFile);
                            }
                        }
                        if (model.VoterIDFilePath != null)
                        {

                            model.VoterIDFile = SaveImageFile(model.VoterIDFilePath, "Courier/Logo");
                        }
                        if (model.AadhaarCardFilePath != null && model.AadhaarCardFile != null)
                        {
                            if (System.IO.File.Exists(path + "AdharCards/ " + model.AadhaarCardFile))
                            { 
                                System.IO.File.Delete(path + "AdharCards/" + model.AadhaarCardFile);
                            }
                        }
                        if (model.AadhaarCardFilePath != null)
                        {

                            model.AadhaarCardFile = SaveImageFile(model.AadhaarCardFilePath, "Courier/Logo");
                        }

                        if (model.AgreementScanFilePath != null && model.AgreementScanFile != null)
                        {
                            if (System.IO.File.Exists(path + "ScanAgreement/ " + model.AgreementScanFile))
                            { 
                                System.IO.File.Delete(path + "ScanAgreement/" + model.AgreementScanFile);
                            }
                        }
                        if (model.AgreementScanFilePath != null)
                        {

                            model.AgreementScanFile = SaveImageFile(model.AgreementScanFilePath, "Courier/Logo");
                        }

                        if (model.CancelledChequeFilePath != null && model.CancelledChequeFile != null)
                        {
                            if (System.IO.File.Exists(path + "CancelledCheques/ " + model.CancelledChequeFile))
                            { 
                                System.IO.File.Delete(path + "CancelledCheques/" + model.CancelledChequeFile);
                            }
                        }
                        if (model.CancelledChequeFilePath != null)
                        {

                            model.CancelledChequeFile = SaveImageFile(model.CancelledChequeFilePath, "Courier/Logo");
                        }

                        var result = con.Query<int>("Add_Modify_Delete_Courier",
                            new
                            {
                                //Settings
                                model.CourierId,
                                model.CourierName,
                                model.CourierCode,
                                model.CourierBrandName,
                                model.Priority,
                                model.CourierTAT,
                                model.AWBNumber,
                                CountryId = model.Country,
                                StateId = model.StateDropdown,
                                CityId = model.CityDropdown,
                                model.UploadedCourierFile,
                                model.IsReverse,
                                model.IsAllowPreference,
                                //Organisation
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
                                //Address and Contact Person
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
                                model.UserPANCard,
                                model.UserPANCardFile,
                                model.VoterIDCardNo,
                                model.VoterIDFile,
                                model.AadhaarCardNo,
                                model.AadhaarCardFile,
                                model.IsUser,
                                //Service Charge
                                SC_Country = model.SC_CountryDropdown,
                                SC_Pincode = model.SC_PincodeDropdown,
                                model.Currency,
                                model.ServiceChargeType,
                                model.ValueRange,
                                WeightRange = model.WeightRange1 + "-" + model.WeightRange2,
                                Volume = model.Volumn1 + "-" + model.Volumn2,
                                model.ServiceCharge,
                                model.ApplicableFromDate,
                                model.ItemType,
                                //Agreement
                                model.LegalDocumentVerificationStatus,
                                model.AgreementSignupStatus,
                                model.AgreementStartDate,
                                model.AgreementEndDate,
                                model.AgreementNumber,
                                model.AgreementScanFile,
                                //Bank Details
                                model.BankName,
                                model.BankAccountNumber,
                                model.CompanyNameatBank,
                                model.IFSCCode,
                                model.BankBranch,
                                model.CancelledChequeFile,
                                model.PaymentCycle,
                                //Registration
                                model.LuluandSky_Status,
                                model.Comments,
                                model.IsActive,                              
                                User = Convert.ToInt32(Session["User_ID"]),
                                Action = "U",                               
                            }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                        
                        var response = new ResponseModel();
                        if (result == 2)
                        {
                            response.IsSuccess = true;
                            response.Response = "Courier Name Updated Successfully";
                            TempData["response"] = response;
                            TempData.Keep("response");
                        
                        }
                   
                    else
                    {
                            response.IsSuccess = true;
                            response.Response = "Courier Name Not Updated Successfully";
                            TempData["response"] = response;
                            TempData.Keep("response");

                        }
                   
                    return RedirectToAction("ManageCourier");
                }
                }
               
            }
            catch (Exception e)
            {
                TempData["AddCourier"] = e;
                //throw;
            }

            return RedirectToAction("ManageCourier");
        }
      
    }
}