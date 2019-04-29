using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class OrganizationModel
    {
        public Guid? OrgId { get; set; } 
        public Guid? RefKey { get; set; }
        [DisplayName("Organisation Name")]
        [Required]
        public string OrgName { get; set; }
        [DisplayName("Organisation Code")]
        [Required]
        public string OrgCode { get; set; }
        [DisplayName("Organisation IEC Number")]
        public string OrgIECNumber { get; set; }
        [DisplayName("Organisation Statutory Type")]
        [Required]
        public int OrgStatutoryType { get; set; }
        [DisplayName("Organisation Application Tax Type")]
        [Required]
        public int OrgApplicationTaxType { get; set; }
        [DisplayName("Organisation GST Category")]
        [Required]
        public int OrgGSTCategory { get; set; }
        public string OrgGSTCat { get; set; }
        [DisplayName("Organisation GST Number")]
        [Required]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}", ErrorMessage = "Invalid GST Number")]
        public string OrgGSTNumber { get; set; }
        public HttpPostedFileBase OrgGSTNumberFilePath { get; set; }
        public string OrgGSTFileName { get; set; }
        public string OrgGSTFileUrl { get; set; }
        [DisplayName("Organisation PAN Number")]
        [Required]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Invalid PAN Number")]
        public string OrgPanNumber { get; set; }
        public HttpPostedFileBase OrgPanNumberFilePath { get; set; }
        public string OrgPanFileName { get; set; }
        public string OrgPanFileUrl { get; set; }
        public SelectList GstCategoryList { get; set; }
        public SelectList StatutoryList { get; set; }
        public SelectList AplicationTaxTypeList { get; set; }
        public char Action { get; set; }
        public int UserId { get; set; }
      
    }
}