using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ServiceCenterModel
    {
       

        public ServiceCenterModel()
        {
            Activetab = "tab-1";

        }
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
        public List<ContactPersonModel> ContactPersons { get; set; }
        public List<BankDetailModel> BankDetails { get; set; }
        public Guid? CenterId { get; set; }
        [Required]
        [DisplayName("Service Provider")]
        public Guid? ProviderId { get; set; }
        [Required]
        [DisplayName("Process Name")]
        public int ProcessId { get; set; }
           
        public string ProcessName { get; set; }

        [Required]
        [DisplayName("Service Center Code")]
        public string CenterCode { get; set; }
        [Required]
        [DisplayName("Service Center Name")]
        [System.Web.Mvc.Remote("RemoteValidationCenterName", "Master", AdditionalFields =  "CurrentCenterName", ErrorMessage = "Service Center Name already exists!")]
        public string CenterName { get; set; }
        public string CurrentCenterName { get; set; }
        [DisplayName("Organization Name")]
        public string ORGNAME { get; set; }

        public bool IsUser { get; set; }
        [Required]
        [System.Web.Mvc.Remote("RemoteValidationforUserName", "Master", AdditionalFields= "CurrentUserName",  ErrorMessage = "User Name already exists!")]
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
        
        public SelectList SupportedCategoryList { get; set; }
        public SelectList ProviderList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Type")]
        public List<TogoFogo.CheckBox> ServiceList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Delivery Type")]
        public List<TogoFogo.CheckBox> DeliveryServiceList { get; set; }
        public SelectList ProcessList { get; set; }
     

    }

   
}