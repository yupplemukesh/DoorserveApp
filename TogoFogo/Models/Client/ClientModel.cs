using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ClientModel
    {
       

        public ClientModel()
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
        public Guid ClientId { get; set; }
        [Required]
        [DisplayName("Process Name")]
        public int ProcessId { get; set; }
           
        public string ProcessName { get; set; }

        [Required]
        [DisplayName("Client Code")]
        public string ClientCode { get; set; }
        [Required]
        [DisplayName("Client Name")]
        public string ClientName { get; set; }
        [DisplayName("Organization Name")]
        public string ORGNAME { get; set; }


       
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        
        public SelectList SupportedCategoryList { get; set; }

        [SkillValidation(ErrorMessage = "Select at least 1 Service Type")]
        public List<TogoFogo.CheckBox> ServiceList { get; set; }
        [SkillValidation(ErrorMessage = "Select at least 1 Service Delivery Type")]
        public List<TogoFogo.CheckBox> DeliveryServiceList { get; set; }
        public SelectList ProcessList { get; set; }
     

    }

    public class SkillValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<TogoFogo.CheckBox> instance = value as List<TogoFogo.CheckBox>;
            int count = instance == null ? 0 : (from p in instance
                                                where p.IsChecked == true
                                                select p).Count();
            if (count >= 1)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}