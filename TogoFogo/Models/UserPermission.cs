using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class UserPermission
    {
        public Int64 SerialNo { get; set; }
        public Int64 PermissionId { get; set; }
        [Required(ErrorMessage ="Select User Name")]
        public Int64 UserId { get; set; }
        [Required(ErrorMessage = "Select User Role")]
        public Int64 RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public Int64 UserLoginId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; } 
        public Boolean IsActive { get; set; }
        [NotMapped]
        public List<User>  UserList { get; set; }
        [NotMapped]
        public List<UserRole> UserRoleList { get; set; }        
        public List<MenuMasterModel> _MenuList { get; set; }
    }

    public  enum  Actions
    {
        View=1,Create=2,Edit=3,Delete=4,History=5, ExcelExport = 6
    }
   
}