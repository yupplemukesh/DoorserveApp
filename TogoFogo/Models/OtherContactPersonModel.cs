using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class OtherContactPersonModel:ContactPersonModel
    {
           [Required]
            public override int ? AddressTypeId { get; set; }
        [Required]
        public override string Address { get; set; }
        [Required]
        public override int? CountryId { get; set; }
        [Required]
        public override int? StateId { get; set; }
        [Required]
        public override int ?CityId { get; set; }
      
       
    }
}