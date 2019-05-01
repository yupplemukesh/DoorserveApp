using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class AddressDetail:RegistrationModel
    {
        public Guid AddresssId { get; set; }
        [DisplayName("Address Type")]
        [Required]
        public int AddressTypeId { get; set; }
        [DisplayName("Country")]
        [Required(ErrorMessage = "Enter Country")]
        public  int CountryId { get; set; }
        [DisplayName("State")]
        [Required(ErrorMessage = "Enter State")]
        public  int StateId { get; set; }
        [DisplayName("City")]
        [Required(ErrorMessage = "Enter City")]
        public int CityId { get; set; }      
        public string City { get; set; }
        public string State { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string Address { get; set; } 
        public string Locality { get; set; }
        [DisplayName("Near By Location")]
        public string NearLocation { get; set; }
        [DisplayName("Pin Code")]
        [Required(ErrorMessage = "Enter Pin Code")]
        public string PinNumber { get; set; }
        public SelectList AddressTypelist { get; set; }
        public SelectList CityList { get; set;}
        public SelectList StateList { get; set;}
        public SelectList CountryList { get; set;}
    }
}