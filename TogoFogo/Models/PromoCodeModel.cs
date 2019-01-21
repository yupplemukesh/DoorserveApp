using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class PromoCodeModel
    {
        public string PromoCode { get; set; }
        public string Amount { get; set; }
        
        public string FromDate { get; set; }        
        public string ToDate { get; set; }
    }
}