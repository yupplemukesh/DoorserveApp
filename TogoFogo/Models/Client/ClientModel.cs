using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ClientModel:RegistrationModel
    {
       

        public ClientModel()
        {
            Activetab = "tab-1";
            Organization = new OrganizationModel();
            ContactPersons = new List<OtherContactPersonModel>();
            BankDetails = new List<BankDetailModel>();
            Bank = new BankDetailModel();
            Contact = new OtherContactPersonModel();
            Services = new List<ServiceModel>();
            Service = new ServiceModel();
        }
        public string Path { get; set; }
      
       
        public string Activetab { get; set; }
        public char action { get; set; }
        public OrganizationModel Organization { get; set; }
        public List<OtherContactPersonModel> ContactPersons { get; set; }
        public OtherContactPersonModel Contact { get; set; }
        public List<BankDetailModel> BankDetails { get; set; }
        public List<ServiceModel> Services { get; set; }
        public ServiceModel Service { get; set; }
        public BankDetailModel Bank { get; set; }
        public Guid? ClientId { get; set; }
        [Required]
        [DisplayName("Process Name")]
        public int ProcessId { get; set; }           
        public string ProcessName { get; set; }
        [Required]
        [DisplayName("Client Code")]
        public string ClientCode { get; set; }
        [Required]
        [DisplayName("Client Name")]
        [System.Web.Mvc.Remote("RemoteValidationClientName", "Master", AdditionalFields =  "CurrentClientName", ErrorMessage = "Client Name already exists!")]
        public string ClientName { get; set; }
        public string CurrentClientName { get; set; }
        [DisplayName("Organization Name")]
        public string ORGNAME { get; set; }
        [Required]
        [System.Web.Mvc.Remote("RemoteValidationforUserName", "Master", AdditionalFields= "CurrentUserName",  ErrorMessage = "UserName already exists!")]
        public string UserName { get; set; }
        [Required]
        public string CurrentUserName { get; set;}
        [DisplayName("Is Active ?")]
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }

       
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