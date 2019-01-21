using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageTRCModel
    {
        public string SupportedCategory { get; set; }
        public string[] Supported_Category { get; set; }
        [DisplayName("Process Name")]
        [Required]
        public string PROCESS_NAME { get; set; }
        public string TRC_ID { get; set; }

        [DisplayName("TRC Code")]
        [Required]
        public string TRC_CODE { get; set; }
        [DisplayName("TRC Name")]
        [Required]
        public string TRC_NAME { get; set; }

        [DisplayName("Organization/Service Provider/Partner Name")]
        [Required]
        public string ORG_NAME { get; set; }
        [DisplayName("Organization Code (CIN)")]
        public string CIN { get; set; }
        [DisplayName("Organization IEC Number")]
        public string IEC_NO { get; set; }
        [DisplayName("Statutory Type")]
        public string STATUTORY_TYPE { get; set; }
        [DisplayName("Applicable Tax Type")]
        public string APPL_TAX_TYPE { get; set; }
        [DisplayName("GST Number")]
        public string GST_N0 { get; set; }
        public HttpPostedFileBase UPLOAD_GST_NO1 { get; set; }
        [DisplayName("Upload GST Number")]
        public string UPLOAD_GST_NO { get; set; }
        [DisplayName("PAN Card Number")]
        //[RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "* Invalid PAN Number")]
        public string PAN_NO { get; set; }

        public HttpPostedFileBase UploadPanCardNO { get; set; }
        [DisplayName("Upload PAN Card Number")]
        public string UPLOAD_PAN_NO { get; set; }
        [DisplayName("Address type")]
        public string ADDRESS_TYPE { get; set; }
        [DisplayName("Country")]
        public string COUNTRY { get; set; }
        [DisplayName("State/Province/Union Territory")]
        public string STATE_TERRITORY { get; set; }
        [DisplayName("City/Location")]
        public string CITY { get; set; }
        [DisplayName("Address")]
        public string ADDRESS { get; set; }
        [DisplayName("Locality/PS/PO")]
        public string LOCALITY { get; set; }
        [DisplayName("Near By Location")]
        public string NEAR_BY_LOCATION { get; set; }
        [DisplayName("PIN/ZIP")]
       
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "Pincode must be a digit")]
        public int PIN_CODE { get; set; }
        [DisplayName("First Name")]
        public string FIRST_NAME { get; set; }
        [DisplayName("Last Name")]
        public string LAST_NAME { get; set; }
        [DisplayName("Mobile Number")]
        
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered Mobile format is not valid.")]
        public string MOBILE_NO { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string EMAIL { get; set; }
        [DisplayName("Is User?")]
        public string IS_USER { get; set; }
        [DisplayName("PAN Card Number")]
        
        //[RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "* Invalid PAN Number")]
        public string CONTACT_PERSON_PANNO { get; set; }
        public HttpPostedFileBase PERSON_UPLOAD_PANNO1 { get; set; }
        [DisplayName("Upload PAN Card Number")]
        public string PERSON_UPLOAD_PANNO { get; set; }
        [DisplayName("Voter ID Card Number")]
        public string VOTER_CARD_N0 { get; set; }
        public HttpPostedFileBase UPLOAD_VOTER_CARD_NO1 { get; set; }
        [DisplayName("Upload Voter ID Card Number")]
        public string UPLOAD_VOTER_CARD_NO { get; set; }
        [DisplayName("Aadhar Card Number")]

        //[RegularExpression(@"^[0-9]*$", ErrorMessage = "* Aadhar Card Number")]
        [MaxLength(12)]
        public string AADHAR_CARD_NO { get; set; }

        public HttpPostedFileBase UPLOAD_AADHAR_CARD_NO1 { get; set; }
        [DisplayName("Upload Aadhaar Card Number")]
        public HttpPostedFileBase Upload_Aadhaar_Card_Number { get; set; }
        [DisplayName("Upload Aadhaar Card Number")]
        public string UPLOAD_AADHAR_CARD_NO { get; set; }
        [DisplayName("Bank Name")]
        public string BANK_NAME { get; set; }
        [DisplayName("Bank Account Number")]
        public string BANK_ACCOUNT_NO { get; set; }
        [DisplayName("Company Name at Bank Account")]
        public string COMPANY_NAME_BANK_ACCNT { get; set; }
        [DisplayName("IFSC Code")]
        public string IFSC_CODE { get; set; }
        [DisplayName("Bank Branch")]
        public string BANK_BRANCH { get; set; }
        public HttpPostedFileBase UPLOAD_CANCELLED_CHEQUE1 { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public string UPLOAD_CANCELLED_CHEQUE { get; set; }
        [DisplayName("Is Active")]
        [Required]
        public string IS_ACTIVE { get; set; }
        [DisplayName("Comments")]
        public string COMMENTS { get; set; }
        [DisplayName("Last Update Date and Time")]
        public string LAST_UPDATE_DATETIME { get; set; }
        [DisplayName("Last update Details")]
        public string LAST_UPDATE_DETAILS { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATE_DATE { get; set; }
        public string MODIFY_BY { get; set; }
        public string MODIFY_DATE { get; set; }
        public int DELETE_BY { get; set; }
        public string DELETE_DATE { get; set; }
       
        
    }
}