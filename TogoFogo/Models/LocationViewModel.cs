using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class LocationViewModel
    {
        public UserActionRights Rights { get; set; }
        public string AddLocation { get; set; }
        public  string EditLocation {get;set; }
    }
}