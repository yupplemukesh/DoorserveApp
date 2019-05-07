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
        public virtual  int? AddressTypeId { get; set; }
        [DisplayName("Country")]
        public virtual int? CountryId { get; set; }
        [DisplayName("State")]
        public virtual int? StateId { get; set; }
        [DisplayName("City")]
        public virtual int? CityId { get; set; }      
        public string City { get; set; }
        public string State { get; set; }
        public virtual string Address { get; set; } 
        public string Locality { get; set; }
        [DisplayName("Near By Location")]
        public string NearLocation { get; set; }
        [DisplayName("Pin Code")]
        public string PinNumber { get; set; }
        public SelectList AddressTypelist { get; set; }
        public SelectList CityList { get; set;}
        public SelectList StateList { get; set;}
        public SelectList CountryList { get; set;}
    }
}