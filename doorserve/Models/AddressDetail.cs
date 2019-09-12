using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class AddressDetail:RegistrationModel
    {
        public Guid? AddresssId { get; set; }
        [DisplayName("Address Type")]

        public virtual  int? AddressTypeId { get; set; }
     

        [DisplayName("District")]
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Required(ErrorMessage ="Select Location")]
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public virtual string Address { get; set; } 
        public string Locality { get; set; }
        [DisplayName("Near By Location")]
        public string NearLocation { get; set; }
        [DisplayName("Pin Code")]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code")]
        [Required(ErrorMessage ="Enter Pincode")]
        public string PinNumber { get; set; }
       
        public SelectList AddressTypelist { get; set; }
        public SelectList CityList { get; set;}
        public SelectList StateList { get; set;}
        public SelectList CountryList { get; set;}
        public SelectList LocationList { get; set; }
    }
}