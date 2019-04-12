using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models.Company
{
    public class AgreementModel:RegistrationModel
    {
        public int CompanyTypeId { get; set; }
        [DisplayName("Company Type")]
        public string CompanyType { get; set; }
        public int PayableTypeId { get; set; }
        public DateTime AgreementStartDate { get; set; }
        public string AgreementPeriod { get; set; }
        public string AgreementNumber { get; set; }
        public string AgreementFile { get; set; }
        //public HttpPostedFileBase AgreementPath { get; set; }
        public HttpPostedFileBase CancelledCheckPath { get; set; }
        public string CancelledCheckFile { get; set; } 
        public List<CheckBox> PayableTypeList { get; set; }
        public string ServiceTypes { get; set; }
        public string  DeliveryTypes{ get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CompanyTypeName { get; set; }
        public Guid RefKey { get; set; }
        public Guid ? AGRId { get; set; }
        public char Action { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Type")]
        public List<TogoFogo.CheckBox> ServiceList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Delivery Type")]
        public List<TogoFogo.CheckBox> DeliveryServiceList { get; set; }

    }
}