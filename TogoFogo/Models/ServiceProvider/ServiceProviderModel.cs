using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ServiceProviderModel:RegistrationModel
    {
       

        public ServiceProviderModel()
        {
            Activetab = "tab-1";
            Organization = new OrganizationModel();
            ContactPersons = new List<OtherContactPersonModel>();
            BankDetails = new List<BankDetailModel>();
            Bank = new BankDetailModel();
            Contact = new OtherContactPersonModel();
            Services = new List<ServiceOfferedModel>();
            Service = new ServiceOfferedModel();


        }

        public string Path { get; set; }
        
        public string Activetab { get; set; }
        public char action { get; set; }
        public OrganizationModel Organization { get; set; }
        public List<OtherContactPersonModel> ContactPersons { get; set; }
        public OtherContactPersonModel Contact { get; set; }

        public List<BankDetailModel> BankDetails { get; set; }
        public List<ServiceOfferedModel> Services { get; set; }
        public ServiceOfferedModel Service { get; set; }
        public BankDetailModel Bank { get; set; }
        public Guid ProviderId { get; set; }

        [DisplayName("Process Name")]
        public int? ProcessId { get; set; }
           
        public string ProcessName { get; set; }

        [Required]
        [DisplayName("Service Provider Code")]
        public string ProviderCode { get; set; }
        [Required]
        [DisplayName("Service Provider Name")]
        [System.Web.Mvc.Remote("RemoteValidationProviderName", "Master", AdditionalFields =  "CurrentProviderName", ErrorMessage = "Provider Name already exists!")]
        public string ProviderName { get; set; }
        public string CurrentProviderName { get; set; }
        [DisplayName("Organization Name")]
        public string ORGNAME { get; set; }

        public bool IsSuperAdmin { get; set; }
        public SelectList CompanyList { get; set; }

        public int CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
     
       
     

    }

   
}