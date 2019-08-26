using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class TableMenuModel
    {
        public string MenuCap_ID { get; set; }
        public string Menu_Name { get; set; }
        public string SubMenuName { get; set; }
        public string ParentMenuName { get; set; }
        public string ParentMenuID { get; set; }
        public bool isNewlyEnrolled { get; set; }
    }
    public class ViewStatusCheck
    {
        public string MenuId { get; set; }
    }
    public class UserTypeModel
    {
        public string Comments { get; set; }
        public int[] foo { get; set; }
        public List<submenuModel> TableSubMenu { get; set; }
        public List<TableMenuModel> TableMenu { get; set; }
        public string UserType { get; set; }
        public string UserTypeId { get; set; }
        public string IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string MenuMasters { get; set; }
    }
    public class UserRole1
    {
        public string Comments { get; set; }
        public int[] foo { get; set; }
       public List<MenuMasterModel> y { get; set; }
        public List<TableMenuModel> TableMenu { get; set; }
        public string UserType { get; set; }
        public string RoleId { get; set; }
        public string UserRole { get; set; }
        public string UserTypeId { get; set; }
        public string IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string MenuMasters { get; set; }
    }
    public class User1
    {
        public string Comments { get; set; }
        public int[] foo { get; set; }
        public List<MenuMasterModel> y { get; set; }
        public List<TableMenuModel> TableMenu { get; set; }
        public string UserType { get; set; }
        public string RoleId { get; set; }
        public string UserRole { get; set; }
        public string UserTypeId { get; set; }
        public string IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string MenuMasters { get; set; }
        public string EmployeeId { get; set; }
        public string AddressType { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Email_Address { get; set; }
        public string UserName { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string ServiceProvider { get; set; }
        public string ServiceProviderType { get; set; }
        public string TRC { get; set; }
    }

}