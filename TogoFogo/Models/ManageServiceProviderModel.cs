using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageServiceProviderModel
    {
        public int SerialNo { get; set; }
        public string GstCategoryId { get; set; }
        [DisplayName("Gst Category")]
        public string GstCategory { get; set; }
        [DisplayName("State")]
        public string St_Name { get; set; }
        [DisplayName("Location")]
        public string LocationName { get; set; }
        public string DeviceCategory { get; set; }
        public string ProviderId { get; set; }
        [Required]
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        [Required]
        [DisplayName("Provider Code")]
        public string ProviderCode { get; set; }
        [Required]
        [DisplayName("Provider Name")]
        public string ProviderName { get; set; }
        [DisplayName("Organisation Name")]
        public string OrganisationName { get; set; }
        [DisplayName("Number Of Center")]
        public string NumberOfCenter { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }
        public string Country { get; set; }
        [DisplayName("Provider State")]
        public string ProviderState { get; set; }
        [DisplayName("Provider City")]
        public string ProviderCity { get; set; }
        public string Address { get; set; }
        public string Locality { get; set; }
        [DisplayName("Near By Location")]
        public string NearByLoc { get; set; }
        public string Pincode { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Mobile No")]
        public string Mobile_No { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string Email { get; set; }
        [DisplayName("Supported Category")]
        public string SupportedCategory { get; set; }
        public string[] Supported_Category { get; set; }
        [DisplayName("Organization Code (CIN)")]
        public string Org_Code { get; set; }
        [DisplayName("Organization IEC Number")]
        public string Org_IEC_No { get; set; }
        [DisplayName("Statutory Type")]
        public string StatutoryType { get; set; }
        [DisplayName("Applicable Tax Type")]
        public string TaxType { get; set; }

        [DisplayName("GST Number")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}", ErrorMessage = "Invalid GST Number")]
        public string GST_No { get; set; }
        [DisplayName(" Upload GST Number")]
        public HttpPostedFileBase GST_No_File1 { get; set; }
        [DisplayName(" Upload GST Number")]
        public string GST_No_File { get; set; }
        [DisplayName(" PAN Card Number")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN Number")]
        public string PancardNo { get; set; }
        [DisplayName("Upload PAN Card Number")]
        public HttpPostedFileBase Pancardno_File1 { get; set; }
        [DisplayName("Upload PAN Card Number")]
        public string Pancardno_File { get; set; }
        [DisplayName("Is User?")]
        public Boolean IsUser { get; set; }
        [DisplayName("User PAN Card Number")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN Number")]
        public string UserPanNo { get; set; }
        [DisplayName("Upload Pan Card")]
        public HttpPostedFileBase UserPanNo_File1 { get; set; }
        [DisplayName("Upload Pan Card")]
        public string UserPanNo_File { get; set; }
        [DisplayName("Voter ID Card Number")]
        public string VoterIdCard { get; set; }
        [DisplayName("Upload Voter ID Card Number")]
        public HttpPostedFileBase VoterIdCard_File1 { get; set; }
        [DisplayName("Upload Voter ID Card Number")]
        public string VoterIdCard_File { get; set; }
        [DisplayName("Aadhaar Number")]
        
        //[RegularExpression(@"^\d{4}\s\d{4}\s\d{4}", ErrorMessage = "* Invalid Aadhaar Number")]
        public string AadhaarNo { get; set; }
        [DisplayName("Upload Aadhaar Number")]
        public HttpPostedFileBase AadharCard_File1 { get; set; }
        [DisplayName("Upload Aadhaar Number")]
        public string AadharCard_File { get; set; }
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [DisplayName("Bank Account Number")]
        public string BankAccNo { get; set; }
        [DisplayName("Company Name at Bank Account")]
        public string CompanyNameAtBank { get; set; }
        [DisplayName("IFSC Code")]
        public string IFSC_Code { get; set; }
        [DisplayName("Bank Branch")]
        public string BankBranch { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public HttpPostedFileBase CancelledChequeFile1 { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public string CancelledChequeFile { get; set; }
        [DisplayName("Is Active ?")]
        [Required]
        public Boolean IsActive { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public int  User { get; set; }
        public UserActionRights _UserActionRights { get; set; }
        public List<ManageServiceProviderModel> ManageServiceProviderModelList { get; set; }
    }
}