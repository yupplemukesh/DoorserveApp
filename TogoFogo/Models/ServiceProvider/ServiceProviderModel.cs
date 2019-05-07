using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ServiceProviderModel
    {
       

        public ServiceProviderModel()
        {
            Activetab = "tab-1";
            Organization = new OrganizationModel();
            ContactPersons = new List<OtherContactPersonModel>();
            BankDetails = new List<BankDetailModel>();
            Bank = new BankDetailModel();
            Contact = new OtherContactPersonModel();

        }

        public string Path { get; set; }
        public string ServiceTypes
        {
            get;

            set;
        }
        public string ServiceDeliveryTypes
        {
            get;set;
        }
        public List<int> DeviceCategories { get; set; }
        public string _deviceCategories {
            get;
            set;
        }
        public string Activetab { get; set; }
        public char action { get; set; }
        public OrganizationModel Organization { get; set; }
        public List<OtherContactPersonModel> ContactPersons { get; set; }
        public OtherContactPersonModel Contact { get; set; }

        public List<BankDetailModel> BankDetails { get; set; }
        public BankDetailModel Bank { get; set; }
        public Guid ProviderId { get; set; }
        [Required]
        [DisplayName("Process Name")]
        public int ProcessId { get; set; }
           
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

        public bool IsUser { get; set; }
        [Required]
        [System.Web.Mvc.Remote("RemoteValidationforUserName", "Master", AdditionalFields= "CurrentUserName",  ErrorMessage = "UserName already exists!")]
        public string UserName { get; set; }
        [Required]
        public string CurrentUserName { get; set;}
        public string Password { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public Guid? CompanyId { get; set; }  
        public SelectList SupportedCategoryList { get; set; }

        [SkillValidation(ErrorMessage = "Select at least 1 Service Type")]
        public List<TogoFogo.CheckBox> ServiceList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Delivery Type")]
        public List<TogoFogo.CheckBox> DeliveryServiceList { get; set; }
        public SelectList ProcessList { get; set; }
     

    }

   
}