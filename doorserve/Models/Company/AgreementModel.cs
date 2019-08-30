﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace doorserve.Models.Company
{
    public class AgreementModel:RegistrationModel
    {
        public int CompanyTypeId { get; set; }
        [DisplayName("Company Type")]
        public string CompanyType { get; set; }
        public int PayableTypeId { get; set; }
        [DataType(DataType.Date)]
        public string AgreementStartDate { get; set; }
        public string AgreementPeriod { get; set; }
        public string AgreementNumber { get; set; }
        public string AgreementFile { get; set; }
        public string AgreementFileUrl { get; set; }
        public HttpPostedFileBase AgreementPath { get; set; }
        public HttpPostedFileBase CancelledChequePath { get; set; }
        public string CancelledChequeFile { get; set; }
        public string CancelledChequeFileUrl { get; set; }
        public List<CheckBox> PayableTypeList { get; set; }
        public string ServiceTypes { get; set; }
        public string  DeliveryTypes{ get; set; }
        public int CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ? ModifyDate { get; set; }
        public string CompanyTypeName { get; set; }
        public Guid? RefKey { get; set; }
        public Guid? AGRId { get; set; }
        public char Action { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Type")]
        public List<doorserve.CheckBox> ServiceList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Delivery Type")]
        public List<doorserve.CheckBox> DeliveryServiceList { get; set; }

    }
}