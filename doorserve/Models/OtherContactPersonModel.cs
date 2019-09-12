using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class OtherContactPersonModel:ContactPersonModel
    {
       
        [Required(ErrorMessage = "Select address type")]
        public override int ? AddressTypeId { get; set; }
        [Required(ErrorMessage = "Enter address")]
        public override string Address { get; set; }      
       
    }
}