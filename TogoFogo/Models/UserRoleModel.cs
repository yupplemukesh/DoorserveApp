using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class UserRolrightsModel
    {
        public string RoleId { get; set; }
        public string UserRole { get; set; }
        public string MenuId { get; set; }
        public string ParentMenu_id { get; set; }
    }
    public class UserRoleModel
    {
        public string Comments { get; set; }
        public int[] foo { get; set; }
        public List<submenuModel> TableSubMenu { get; set; }
        public string UserRole { get; set; }
        public string RoleId { get; set; }
        public string IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string UserType { get; set; }
        public string UserTypeId { get; set; }
        public string MenuMasters { get; set; }
        public List<UserRolrightsModel> TableData { get; set; }
    }
}