using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ContactPersonModel
    {
      
        public Guid ContactId { get; set; }
        public Guid ClientId { get; set; }
        [DisplayName("Address Type")]
        public int ConAddressType{ get; set; }
        [DisplayName("Country")]
        public int ConCountry { get; set; }
        [DisplayName("State")]
        public int ConState { get; set; }
        [DisplayName("City")]
        public int ConCity { get; set; }
        [DisplayName("Address")]
        public string ConAddress { get; set; }
        [DisplayName("Locality")]
        public string ConLocality { get; set; }
        [DisplayName("Near By Location")]
        public string ConNearByLocation { get; set; }
        [DisplayName("Pin Code")]
        public string ConPinNumber { get; set; }
        [DisplayName("First Name")]
        public string ConFirstName { get; set; }
        [DisplayName("Last Name")]
        public string ConLastName { get; set; }
        [DisplayName("Mobile No")]
        public string ConMobileNumber { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string ConEmailAddress { get; set; }
        public bool IsUser { get; set; }
        [DisplayName("Client PAN Card Number")]
        public string ConPanNumber { get; set; }
        [DisplayName("Upload Pan Card")]
        public HttpPostedFileBase ConPanNumberFilePath { get; set; }
        public string ConPanFileName { get; set; }
        [DisplayName("Voter ID Card Number")]
        public string ConVoterId { get; set; }
        [DisplayName("Upload Voter ID Card Number")]
        public HttpPostedFileBase ConVoterIdFilePath { get; set; }
        public int UserID { get; set; }
        public char Action { get; set; }

        public string ConVoterIdFileName { get; set; }

        [System.Web.Mvc.Remote("RemoteValidationforUserName", "Master", ErrorMessage = "UserName already exists!")]
        public string UserName { get; set; }
        public string Password { get; set; }
        //[RegularExpression(@"^\d{4}\s\d{4}\s\d{4}", ErrorMessage = "* Invalid Aadhaar Number")]
        [DisplayName("Aadhaar Number")]
        public string ConAdhaarNumber { get; set; }
        [DisplayName("Upload Aadhaar Number")]
        public HttpPostedFileBase ConAdhaarNumberFilePath { get; set; }
        public string ConAdhaarFileName { get; set; }
        [DisplayName("Upload Aadhaar Number")]
        public string ConCityName { get; set; }
        public SelectList AddressTypelist { get; set; }
        public SelectList CityList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList CountryList { get; set; }
    }
}