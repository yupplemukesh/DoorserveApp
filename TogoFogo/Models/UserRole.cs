using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class UserRole
    {
        public Int64 SerialNo { get; set; }
        public Int64 RoleId { get; set; }
        [Required(ErrorMessage ="Enter Role Name")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Enter Comments")]
        public string Comments { get; set; }
        public Boolean IsActive { get; set; }
        public Int64 UserLoginId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public MenuMasterModel _Menu { get; set; }
        public List<MenuMasterModel> _MenuList { get; set; }
        public List<MenuMasterModel> _SubMenuList { get; set; }
    }
}