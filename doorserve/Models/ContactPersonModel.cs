using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ContactPersonModel:AddressDetail
    {
        
        
        public Guid? ContactId { get; set; }
        public Guid? RefKey { get; set; }       
        [DisplayName("First Name")]
        [Required( ErrorMessage ="Enter Name")]
        public string ConFirstName { get; set; }
        [DisplayName("Last Name")]
        public string ConLastName { get; set; }       
        public string TechnicianName { get { return ConFirstName + " " + ConLastName; } }
        [DisplayName("Mobile No")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "Enter Mobile No")]
        public string ConMobileNumber { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        [Required(ErrorMessage = "Enter Email")]
        [System.Web.Mvc.Remote("RemoteValidationConEmailAddress", "Master", AdditionalFields = "CurrentEmail", ErrorMessage = "Email already exists!")]
        public string ConEmailAddress { get; set; }
        public string CurrentEmail { get; set; }
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN Number")]
        [DisplayName("Client PAN Card Number")]
        public string ConPanNumber { get; set; }
        [DisplayName("Upload Pan Card")]
        public HttpPostedFileBase ConPanNumberFilePath { get; set; }
        public string ConPanFileName { get; set; }
        public string ConPanFileUrl { get; set; }
        [DisplayName("Voter ID Card Number")]
        [RegularExpression(@"[A-Z]{3}\d{7}", ErrorMessage = "Invalid Voter ID Card Number")]
        public string ConVoterId { get; set; }
        [DisplayName("Upload Voter ID Card Number")]
        public HttpPostedFileBase ConVoterIdFilePath { get; set; }

        public string ConVoterIdFileName { get; set; }
        public string ConVoterIdFileUrl { get; set; }       
        [DisplayName("Aadhaar Number")]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Aadhaar Number")]
        public string ConAdhaarNumber { get; set; }
        [DisplayName("Upload Aadhaar Number")]
        public HttpPostedFileBase ConAdhaarNumberFilePath { get; set; }      
        [DisplayName("Upload Aadhaar Number")]
        public string ConAdhaarFileName { get; set; }
        public string ConAdhaarFileUrl { get; set; }
        public int UserTypeId { get; set; }
        public bool IsSingleCenter { get; set; }



    }

  
}