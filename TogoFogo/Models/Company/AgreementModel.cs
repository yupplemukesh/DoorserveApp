using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models.Company
{
    public class AgreementModel
    {
        public int CompanyTypeId { get; set; }
        [DisplayName("Company Type")]
        public string CompanyType { get; set; }
        public int PaymentMethodTypeId { get; set; }
        public bool PayableByInstitution { get; set; }
        public bool PayableByCorporate { get; set; }
        public DateTime AgreementStartDate { get; set; }
        public string AgreementPeriod { get; set; }
        public string AgreementNumber { get; set; }
        public string AgreementFile { get; set; }
        //public HttpPostedFileBase AgreementPath { get; set; }
        public HttpPostedFileBase CancelledCheckPath { get; set; }
        public string CancelledCheckFile { get; set; }
        public bool IsActive { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
       
    }
}