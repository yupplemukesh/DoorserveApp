﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace doorserve.Models.Company
{
    public class CompanyModel : RegistrationModel
    {
        public CompanyModel()
        {
            Organization = new OrganizationModel();
            Contacts = new List<OtherContactPersonModel>();
            BankDetails = new List<BankDetailModel>();
            Services = new List<ServiceModel>();
            Contact = new OtherContactPersonModel();
            BankDetail = new BankDetailModel();
            Agreement = new AgreementModel();
          
        }

        public string CompanyWebsiteDomainName { get; set; }
        [DataType(DataType.Date)]
        public string DomainExpiryDate { get; set; }
        public string CompanyLogo { get; set; }
        public HttpPostedFileBase CompanyPath { get; set; }
        public string CompanyFile { get; set; }
        public string AndroidAppName { get; set; }
        public string AndroidAppSetting { get; set; }
        public string IOSAppName { get; set; }
        public string IOSAppSetting { get; set; }
        public OrganizationModel Organization { get; set; }
        public List<OtherContactPersonModel> Contacts { get; set; }
        public OtherContactPersonModel Contact { get; set; }
        public List<BankDetailModel> BankDetails { get; set; }
        public BankDetailModel BankDetail { get; set; }
        public AgreementModel Agreement { get; set; }
        public List<ServiceModel> Services { get; set; }
        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [Required]
        [DisplayName("Company Code")]
        public string CompanyCode { get; set; }
        [Required]
        [DisplayName("Company Type")]
        public int CompanyTypeId { get; set; }
        public string CurrentCompanyName { get; set; }
        public int CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ? ModifyDate { get; set; }
        public SelectList CompanyTypeList { get; set; }
        public char Action { get; set; }
        public string ActiveTab{ get; set; }
        public string Path { get; set; }        
        public string CustomerCareNo { get; set; }
        [AllowHtml]
        public string CompanyAddress { get; set; }        
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
       ErrorMessage = "Please Enter Correct Email Address")]
        public string ContactEmail { get; set; }
        [AllowHtml]
        public string SocialLink { get; set; }
        
    }
}