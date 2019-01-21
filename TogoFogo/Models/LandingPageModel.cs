using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class LandingPageModel
    {
        
        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
       
    }
    public class LandingPageExcelModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string city { get; set; }
        public string ProblemDescription { get; set; }
        public string CreatedDate { get; set; }
    }
}