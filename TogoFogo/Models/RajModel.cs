using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class RajModel
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string ContactNo { get; set; }
        public string Contact_Person_Name { get; set; }
        public string CreatedBy { get; set; }
       
        public string CreatedDate { get; set; }
        public string LAT { get; set; }
        public string LON { get; set; }
        public string Location { get; set; }
    }
}