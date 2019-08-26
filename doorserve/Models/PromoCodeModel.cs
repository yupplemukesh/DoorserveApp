using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class PromoCodeModel:UserActionRights
    {
        public string PromoCode { get; set; }
        public string Amount { get; set; }
        
        public string FromDate { get; set; }        
        public string ToDate { get; set; }
        public List<PromoCodeModel> _PromoCodeList { get; set; }
        public UserActionRights _UserActionRights { get; set; }
    }
}