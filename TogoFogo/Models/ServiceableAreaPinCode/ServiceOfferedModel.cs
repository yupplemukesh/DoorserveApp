using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ServiceOfferedModel:ServiceModel
    {

        public Guid? ServiceAreaId { get; set; }
        public int CountPin { get; set;}
        [Required]
        [DisplayName("Country")]
        public int CountryId { get; set;}
        public string Country { get; set; }
        [Required]
        [DisplayName("State")]
        public int StateId { get; set; }
        public string State { get; set; }
        [Required]
        [DisplayName("City")]
        public int CityId { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }

        public SelectList CountryList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList CityList { get; set; }
        public SelectList PinCodeList { get; set; }

    }
}