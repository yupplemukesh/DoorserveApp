using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ServiceOfferedModel:ServiceModel
    {
         
        
        public Guid? ServiceAreaId { get; set; }


        public string Country { get; set; }

        public string State { get; set; }

        [DisplayName("District")]
        public string District { get; set; }
        [Required]
        [DisplayName("Pin Code")]

        public string PinCode { get; set; }

        [Required]
        [DisplayName("Location Name")]
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public SelectList LocationList { get; set; }


    }
}