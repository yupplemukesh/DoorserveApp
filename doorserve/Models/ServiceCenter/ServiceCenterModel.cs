using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ServiceCenterModel
    {



        public ServiceCenterModel()
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
        public string ServiceTypes
        {
            get;

            set;
        }
        public string ServiceDeliveryTypes
        {
            get; set;
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
        public List<BankDetailModel> BankDetails { get; set; }
        public OtherContactPersonModel Contact { get; set; }
        public BankDetailModel Bank { get; set; }
        public List<ServiceOfferedModel> Services { get; set; }
        public ServiceOfferedModel Service { get; set; }
        public Guid CenterId { get; set; }
        [Required]
        [DisplayName("Service Provider")]
        public Guid? ProviderId { get; set; }

        [DisplayName("Process Name")]
        public int? ProcessId { get; set; }

        public string ProcessName { get; set; }

        [Required]
        [DisplayName("Service Center Code")]
        public string CenterCode { get; set; }
        [Required]
        [DisplayName("Service Center Name")]
        [System.Web.Mvc.Remote("RemoteValidationCenterName", "Master", AdditionalFields = "CurrentCenterName", ErrorMessage = "Service Center Name already exists!")]
        public string CenterName { get; set; }
        public string CurrentCenterName { get; set; }
        [DisplayName("Organization Name")]
        public string ORGNAME { get; set; }

        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        public Guid? CompanyId { get; set; }
        public bool IsCompany { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ? ModifyDate { get; set; }
        public bool IsProvider { get; set; }
        public SelectList SupportedCategoryList { get; set; }
        public SelectList ProviderList { get; set; }
        public SelectList CompanyList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Type")]
        public List<doorserve.CheckBox> ServiceList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Delivery Type")]
        public List<doorserve.CheckBox> DeliveryServiceList { get; set; }
        public SelectList ProcessList { get; set; }
     

    }

   
}