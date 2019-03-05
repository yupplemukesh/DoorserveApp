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
        [PermissionBasedAuthorize(new Actions[] { Actions.View }, "Manage Courier")]
        public ActionResult ManageCourier()
        {
            ManageCourierModel objManageCourierModel = new ManageCourierModel();
            using (var con = new SqlConnection(_connectionString))
            {
                objManageCourierModel._ManageCourierModelList = con.Query<ManageCourierModel>("GETCourierMasterData", null, commandType: CommandType.StoredProcedure).ToList();
               
            }
            UserActionRights objUserActiobRight = new UserActionRights();
            objManageCourierModel._UserActionRights = objUserActiobRight;
            string rights = Convert.ToString(HttpContext.Items["ActionsRights"]);
            if (!string.IsNullOrEmpty(rights))
            {
                string[] arrRights = rights.ToString().Split(',');
                for (int i = 0; i < arrRights.Length; i++)
                {
                    if (Convert.ToInt32(arrRights[i]) == 2)
                    {
                        objManageCourierModel._UserActionRights.Create = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 3)
                    {
                        objManageCourierModel._UserActionRights.Edit = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 4)
                    {
                        objManageCourierModel._UserActionRights.Delete = true;
                    }
                    else if (Convert.ToInt32(arrRights[i]) == 6)
                    {
                        objManageCourierModel._UserActionRights.Delete = true;
                    }
                }
            }
            else
            {

                objManageCourierModel._UserActionRights.Create = true;
                objManageCourierModel._UserActionRights.Edit = true;
                objManageCourierModel._UserActionRights.Delete = true;
                objManageCourierModel._UserActionRights.View = true;
                objManageCourierModel._UserActionRights.History = true;
                objManageCourierModel._UserActionRights.ExcelExport = true;

            }
            return View(objManageCourierModel);
        }
        [PermissionBasedAuthorize(new Actions[] { Actions.Create }, "Manage Courier")]
        public async Task<ActionResult> Create()
        {
            DropdownBindController drop = new DropdownBindController();
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
                        string UploadedCourierFile = SaveImageFile(model.UploadedCourierFile1, "Courier/Logo");
                        string UploadedGSTFile= SaveImageFile(model.UploadedGSTFile1, "Courier/Gst");
                        string PANCardFile = SaveImageFile(model.PANCardFile1, "Courier/PanCards");
                        string UserPANCardFile = SaveImageFile(model.UserPANCardFile1, "Courier/Pancards");
                        string VoterIDFile = SaveImageFile(model.VoterIDFile1, "Courier/VoterCards");
                        string AadhaarCardFile = SaveImageFile(model.AadhaarCardFile1, "Courier/AdharCards");
                        string AgreementScanFile = SaveImageFile(model.AgreementScanFile1, "Courier/ScanAgreement");
                        string CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1, "Courier/CancelledCheques");
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
                                model.CreatedBy,
                                model.ModifyBy,
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
                        var errors = ModelState.Values.SelectMany(v => v.Errors);
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

        //public ActionResult CourierTable()
        //{
        //using (var con = new SqlConnection(_connectionString))
        //{
        //    var result = con.Query<ManageCourierModel>("GETCourierMasterData", null, commandType: CommandType.StoredProcedure).ToList();

        //    return View(result);
        //}
        //  return View();
        //}
        [PermissionBasedAuthorize(new Actions[] { Actions.Edit }, "Manage Courier")]
        public async Task<ActionResult> Edit(int? courierId)
        {

            using (var con = new SqlConnection(_connectionString))
            {
                string folder = "~/UploadedImages/Courier/";
                var result = con.Query<ManageCourierModel>("SELECT * from Courier_Master WHERE CourierId=@CourierId", new { CourierId = courierId },
                commandType: CommandType.Text).FirstOrDefault();
                result.UploadedCourierFile = folder+ "Logo/" + result.UploadedCourierFile;               
                result.UploadedGSTFile = folder+ "Gst/" + result.UploadedGSTFile;                
                result.UserPANCardFile = folder + "PanCards/" + result.UserPANCardFile;
                result.VoterIDFile = folder + "VoterCards/" + result.VoterIDFile;
                result.AadhaarCardFile = folder + "AdharCards/" + result.AadhaarCardFile;
                result.AgreementScanFile = folder + "ScanAgreement/" + result.AgreementScanFile;
                result.CancelledChequeFile = folder + "CancelledCheques/" + result.CancelledChequeFile;
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
                model.CreatedBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                model.ModifyBy = (Convert.ToString(Session["User_ID"]) == null ? "0" : Convert.ToString(Session["User_ID"]));
                using (var con = new SqlConnection(_connectionString))
                {
              
                    if (ModelState.IsValid)
                    {
                        string UploadedCourierFile = SaveImageFile(model.UploadedCourierFile1, "Courier/Logo");
                        string UploadedGSTFile = SaveImageFile(model.UploadedGSTFile1, "Courier/Gst");
                        string PANCardFile = SaveImageFile(model.PANCardFile1, "Courier/PanCards");
                        string UserPANCardFile = SaveImageFile(model.UserPANCardFile1, "Courier/Pancards");
                        string VoterIDFile = SaveImageFile(model.VoterIDFile1, "Courier/VoterCards");
                        string AadhaarCardFile = SaveImageFile(model.AadhaarCardFile1, "Courier/AdharCards");
                        string AgreementScanFile = SaveImageFile(model.AgreementScanFile1, "Courier/ScanAgreement");
                        string CancelledChequeFile = SaveImageFile(model.CancelledChequeFile1, "Courier/CancelledCheques");
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
                                model.CreatedBy,
                                model.ModifyBy,
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
        private string SaveImageFile(HttpPostedFileBase file,string folderName)
        {
            try
            {
                string path = Server.MapPath("~/UploadedImages/"+ folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileFullName = file.FileName;
                var fileExtention = Path.GetExtension(fileFullName);
                var fileName = Guid.NewGuid();
                var savedFileName = fileName + fileExtention;
                file.SaveAs(Path.Combine(path, savedFileName));
                return savedFileName;
            }
            catch (Exception ex)
            {

                return ViewBag.Message = ex.Message;
            }
        }
    }
}