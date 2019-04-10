using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models.Company
{
    public class ServiceModel
    {
        public int CompanyTypeId { get; set; }
        [DisplayName("Company Type")]
        public string CompanyType { get; set; }
        [DisplayName("Service Name")]
        public string ServiceName { get; set; }
        public string Note { get; set; }
        public decimal RatePerUser { get; set; }
        public bool IsActive { get; set; }        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}