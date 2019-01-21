using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class submenuModel
    {
        public string Menu_Name { get; set; }
        public string ViewStatus { get; set; }
        public string UserID { get; set; }
        public string MenuId { get; set; }
        public string ID { get; set; }

    }
    public class UserrightsModel
    {
        public string RoleId { get; set; }
        public string UserRole { get; set; }
        public string MenuId { get; set; }
        public string ParentMenu_id { get; set; }
    }
    public class UserrightstableModel
    {
        public string RoleId { get; set; }
        public string UserRole { get; set; }
        public string MenuId { get; set; }
        public string ParentMenu_id { get; set; }
    }
    public class CreateUserModel
    {
        public string EmployeeId { get; set; }
        public string Comments { get; set; }
        public string CreatedDate { get; set; }
        public List<UserrightstableModel> TableData { get; set; }
        public string RoleId { get; set; }
        public int[] foo { get; set; }
        public List<submenuModel> TableSubMenu { get; set; }
        public string ID { get; set; }
        [DisplayName("User Type")]
        public string UserType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Mobile { get; set; }
        [DisplayName("Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email_Address { get; set; }
        [Remote("RemoteValidationforUserName", "Master", ErrorMessage = "UserName already exists!")]
        public string UserName { get; set; }
        [DisplayName("Service Provider")]
        public string ServiceProvider { get; set; }
        [DisplayName("Service Provider Type")]
        public string ServiceProviderType { get; set; }
        public string TRC { get; set; }
        public string TRCId { get; set; }
        public string Password { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string MenuMasters { get; set; }
        public string UserRole { get; set; }
    }
}