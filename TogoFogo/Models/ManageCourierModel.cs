using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageCourierModel
    {
        [Required]
        public string Country { get; set; }
        [Required]
        public string StateDropdown { get; set; }
        [Required]
        public string CityDropdown { get; set; }
        public string PinCodeDropdown { get; set; }
        [DisplayName("Courier Logo")]
        public HttpPostedFileBase UploadedCourierFile1 { get; set; }
        [DisplayName("Gst Number")]
        public HttpPostedFileBase UploadedGSTFile1 { get; set; }
        [DisplayName("Pan Card Number")]
        public HttpPostedFileBase UserPANCardFile1 { get; set; }
        [DisplayName("ID Card Number")]
        public HttpPostedFileBase VoterIDFile1 { get; set; }
        [DisplayName("Aadhaar Card Number")]
        public HttpPostedFileBase AadhaarCardFile1 { get; set; }
        [DisplayName("Agreement Scan Copy")]
        public HttpPostedFileBase AgreementScanFile1 { get; set; }
        [DisplayName("Cancelled Cheque")]
        public HttpPostedFileBase CancelledChequeFile1 { get; set; }
        [DisplayName("Pan Card Number")]
        public HttpPostedFileBase PANCardFile1 { get; set; }
        [DisplayName("Country")]
        public string SC_CountryDropdown { get; set; }
        [DisplayName("Pincode")]
        public string SC_PincodeDropdown { get; set; }
        public string WeightRange1 { get; set; }
        public string WeightRange2 { get; set; }
        public string Volumn1 { get; set; }
        public string Volumn2 { get; set; }
        [DisplayName("State")]
        public string PersonStateDropdown { get; set; }
        [DisplayName("City/Location")]
        public string PersonCityDropdown { get; set; }
        [DisplayName("Country")]
        public string PersonCountryDropdown { get; set; }



        public string CourierId { get; set; }
        [DisplayName("Courier Logo")]
        public string UploadedCourierFile { get; set; }
        [Required]
        public string CourierName { get; set; }
        [Required]
        public string CourierCode { get; set; }
        public string CourierBrandName { get; set; }
        public string Priority { get; set; }
        [DisplayName("Courier TAT in days")]
        public string CourierTAT { get; set; }
        [DisplayName("AWB Number Used Type")]
        public string AWBNumber { get; set; }
        [DisplayName("Is Reverse Logistics?")]
        public string IsReverse { get; set; }
        [DisplayName("Is Allow Order Preference")]
        public string IsAllowPreference { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string CityId { get; set; }
        [DisplayName("Courier Name")]
        public string CourierCompanyName { get; set; }
        [DisplayName("Organization Code (CIN)")]
        public string OrganizationCode { get; set; }
        [DisplayName("Statutory Type")]
        public string StatutoryType { get; set; }
        [DisplayName("Applicable Tax Type")]
        public string ApplicableTaxType { get; set; }
        [DisplayName("GST Number")]
        public string GSTNumber { get; set; }
        public string UploadedGSTFile { get; set; }
        [DisplayName("PAN Card Number")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "* Invalid PAN Number")]
        public string PANCardNumber { get; set; }
        
        public string PANCardFile { get; set; }
        [DisplayName("Bike Make and Model")]
        public string BikeMakeandModel { get; set; }
        [DisplayName("Bike Number")]
        public string BikeNumber { get; set; }
        [DisplayName("Address type")]
        public string PersonAddresstype { get; set; }
        [DisplayName("Country")]
        public string PersonCountry { get; set; }
        [DisplayName("State/Province/Union Territory")]
        public string PersonState { get; set; }
        [DisplayName("City/Location")]
        public string PersonCity { get; set; }
        [DisplayName("Address")]
        public string FullAddress { get; set; }
        [DisplayName("Locality/PS/PO")]
        public string Locality { get; set; }
        [DisplayName("Near By Location")]
        public string NearByLocation { get; set; }
        [DisplayName("PIN/ZIP")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public string Pincode { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Mobile Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public string MobileNumber { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("Is User?")]
        public string IsUser { get; set; }
        [DisplayName("PAN Card Number")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "* Invalid PAN Number")]
        public string UserPANCard { get; set; }
        public string UserPANCardFile { get; set; }
        [DisplayName("Voter ID Card Number")]
        public string VoterIDCardNo { get; set; }
        public string VoterIDFile { get; set; }
        [DisplayName("Aadhaar Card Number")]
        public string AadhaarCardNo { get; set; }
        public string AadhaarCardFile { get; set; }
        [DisplayName("Item Type")]
        public string ItemType { get; set; }
        [DisplayName("Country")]
        public string SC_Country { get; set; }
        [DisplayName("PIN Code")]
        public string SC_Pincode { get; set; }
        public string Currency { get; set; }
        [DisplayName("Service Charge Type")]
        public string ServiceChargeType { get; set; }
        [DisplayName("Value")]
        public string ValueRange { get; set; }
        [DisplayName("Weight (Kg)")]
        public string WeightRange { get; set; }
        [DisplayName("Volume (QM)")]
        public string Volume { get; set; }
        [DisplayName("Service Charge (Selected Currency)")]
        public string ServiceCharge { get; set; }
        [DisplayName("Applicable From Date")]
        public string ApplicableFromDate { get; set; }
        [DisplayName("Legal Document Verification Status")]
        public string LegalDocumentVerificationStatus { get; set; }
        [DisplayName("Agreement Signup Status")]
        public string AgreementSignupStatus { get; set; }
        [DisplayName("Agreement Start Date")]
        public string AgreementStartDate { get; set; }
        [DisplayName("Agreement End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? AgreementEndDate { get; set; }
        [DisplayName("Agreement Number")]
        public string AgreementNumber { get; set; }
        public string AgreementScanFile { get; set; }
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [DisplayName("Company Name at Bank Account")]
        public string CompanyNameatBank { get; set; }
        [DisplayName("IFSC Code")]
        public string IFSCCode { get; set; }
        [DisplayName("Bank Branch")]
        public string BankBranch { get; set; }
        public string CancelledChequeFile { get; set; }
        [DisplayName("Payment Cycle")]
        public string PaymentCycle { get; set; }
        [DisplayName("Registration Status at Togofogo")]
        public string LuluandSky_Status { get; set; }
        [DisplayName("Is Active")]
        [Required]
        public string IsActive { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }

        public String Cnty_Name { get; set; }
        public string St_Name { get; set; }
        public string LocationName { get; set; }

        public string DaysRemaining { get; set; }
    }
}