using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TogoFogo.Models.Company
{
    public class CompanyModel : RegistrationModel
    {
        public CompanyModel()
        {
            Organization = new OrganizationModel();
            Contacts = new List<ContactPersonModel>();
            BankDetails = new List<BankDetailModel>();
            Services = new List<ServiceModel>();
            Contact = new ContactPersonModel();
            BankDetail = new BankDetailModel();
            Agreement = new AgreementModel();
        }

        public string CompanyWebsiteDomainName { get; set; }
        public DateTime DomainExpiryDate { get; set; }
        public string CompanyLogo { get; set; }
        public HttpPostedFileBase CompanyPath { get; set; }
        public string CompanyFile { get; set; }
        public string AnroidAppName { get; set; }
        public string AnroidAppSetting { get; set; }
        public string IOSAppName { get; set; }
        public string IOSAppSetting { get; set; }
        public OrganizationModel Organization { get; set; }
        public List<ContactPersonModel> Contacts { get; set; }
        public ContactPersonModel Contact { get; set; }
        public List<BankDetailModel> BankDetails { get; set; }
        public BankDetailModel BankDetail { get; set; }
        public AgreementModel Agreement { get; set; }
        public List<ServiceModel> Services { get; set; }
        public Guid CompanyId { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public SelectList CompanyTypeList { get; set; }
        public string ActiveTab{ get; set; }
    }
}