using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ManageCourierModel
    {
        public SelectList CountryList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList CityList { get; set; }
        public SelectList PincodeList { get; set; }
        public SelectList ApplicableTaxTypeList { get; set; }
        public SelectList PersonAddressTypeList { get; set; }
        public SelectList AWBNumberUsedList { get; set; }
        public SelectList AgreementSignupList { get; set; }
        public SelectList LegalDocumentVerificationList { get; set; }
        public int SerialNo { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string StateDropdown { get; set; }
        [Required]
        public string CityDropdown { get; set; }
        
        public string PinCodeDropdown { get; set; }
        [Required]
        [DisplayName("Courier Logo")]
        public HttpPostedFileBase UploadedCourierFile1 { get; set; }
        [Required]
        [DisplayName("Gst Number")]
        public HttpPostedFileBase UploadedGSTFile1 { get; set; }
        [Required]
        [DisplayName("Pan Card Number")]
        public HttpPostedFileBase UserPANCardFile1 { get; set; }
        [Required]
        [DisplayName("ID Card Number")]
        public HttpPostedFileBase VoterIDFile1 { get; set; }
        [Required]
        [DisplayName("Aadhaar Card Number")]
        public HttpPostedFileBase AadhaarCardFile1 { get; set; }
        [Required]
        [DisplayName("Agreement Scan Copy")]
        public HttpPostedFileBase AgreementScanFile1 { get; set; }
        [Required]
        [DisplayName("Cancelled Cheque")]
        public HttpPostedFileBase CancelledChequeFile1 { get; set; }
        [Required]
        [DisplayName("Pan Card Number")]
        public HttpPostedFileBase PANCardFile1 { get; set; }
        [Required]
        [DisplayName("Country")]
        public string SC_CountryDropdown { get; set; }
        [Required]
        [DisplayName("Pincode")]
        public string SC_PincodeDropdown { get; set; }
        public string WeightRange1 { get; set; }
        public string WeightRange2 { get; set; }
        public string Volumn1 { get; set; }
        public string Volumn2 { get; set; }
        [Required]
        [DisplayName("State")]
        public string PersonStateDropdown { get; set; }
        [Required]
        [DisplayName("City/Location")]
        public string PersonCityDropdown { get; set; }
        [Required]
        [DisplayName("Country")]
        public string PersonCountryDropdown { get; set; }

        public string CourierId { get; set; }
        
        [DisplayName("Courier Logo")]
        public string UploadedCourierFile { get; set; }
        [Required]
        public string CourierName { get; set; }
        [Required]
        public string CourierCode { get; set; }
        [Required]
        public string CourierBrandName { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        [DisplayName("Courier TAT in days")]
        public string CourierTAT { get; set; }
        [Required]
        [DisplayName("AWB Number Used Type")]
        public string AWBNumber { get; set; }
        [Required]
        [DisplayName("Is Reverse Logistics?")]
        public Boolean IsReverse { get; set; }
        [Required]
        [DisplayName("Is Allow Order Preference")]
        public Boolean IsAllowPreference { get; set; }
        
        public string CountryId { get; set; }
        
        public string StateId { get; set; }
        
        public string CityId { get; set; }
        [Required]
        [DisplayName("Courier Name")]
        public string CourierCompanyName { get; set; }
        [Required]
        [DisplayName("Organization Code (CIN)")]
        public string OrganizationCode { get; set; }
        [Required]
        [DisplayName("Statutory Type")]
        public string StatutoryType { get; set; }
        [Required]
        [DisplayName("Applicable Tax Type")]
        public string ApplicableTaxType { get; set; }
        [Required]
        [DisplayName("GST Number")]
        public string GSTNumber { get; set; }
        public string UploadedGSTFile { get; set; }
        [Required]
        [DisplayName("PAN Card Number")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "* Invalid PAN Number")]
        public string PANCardNumber { get; set; }
        public string PANCardFile { get; set; }
        [Required]
        [DisplayName("Bike Make and Model")]
        public string BikeMakeandModel { get; set; }
        [Required]
        [DisplayName("Bike Number")]
        public string BikeNumber { get; set; }
        [Required]
        [DisplayName("Address type")]
        public string PersonAddresstype { get; set; }
        [DisplayName("Country")]
        public string PersonCountry { get; set; }       
        [DisplayName("State/Province/Union Territory")]
        public string PersonState { get; set; }      
        [DisplayName("City/Location")]
        public string PersonCity { get; set; }
        [Required]
        [DisplayName("Address")]
        public string FullAddress { get; set; }
        [Required]
        [DisplayName("Locality/PS/PO")]
        public string Locality { get; set; }
        [Required]
        [DisplayName("Near By Location")]
        public string NearByLocation { get; set; }
        [Required]
        [DisplayName("PIN/ZIP")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public string Pincode { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Mobile Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public string MobileNumber { get; set; }
        [Required]
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailAddress { get; set; }
        [DisplayName("Is User?")]
        public Boolean IsUser { get; set; }
        [Required]
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
        public Boolean ItemType { get; set; }
        [DisplayName("Country")]
        public string SC_Country { get; set; }
        [DisplayName("PIN Code")]
        public string SC_Pincode { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
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
        [Required]
        [DisplayName("Legal Document Verification Status")]
        public string LegalDocumentVerificationStatus { get; set; }
        [Required]
        [DisplayName("Agreement Signup Status")]
        public string AgreementSignupStatus { get; set; }
        [Required]
        [DisplayName("Agreement Start Date")]
        public string AgreementStartDate { get; set; }
        [Required]
        [DisplayName("Agreement End Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? AgreementEndDate { get; set; }
        [DisplayName("Agreement Number")]
        public string AgreementNumber { get; set; }
        public string AgreementScanFile { get; set; }
        [Required]
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [Required]
        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [Required]
        [DisplayName("Company Name at Bank Account")]
        public string CompanyNameatBank { get; set; }
        [Required]
        [DisplayName("IFSC Code")]
        public string IFSCCode { get; set; }
        [Required]
        [DisplayName("Bank Branch")]
        public string BankBranch { get; set; }
        public string CancelledChequeFile { get; set; }
        [DisplayName("Payment Cycle")]
        public string PaymentCycle { get; set; }
        [DisplayName("Registration Status at Togofogo")]
        public string LuluandSky_Status { get; set; }
        [DisplayName("Is Active")]
        [Required]
        public Boolean IsActive { get; set; }
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