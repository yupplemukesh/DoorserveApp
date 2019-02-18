using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class UserPermission
    {
        public Int64 PermissionId { get; set; }
        public Int64 UserId { get; set; }
        public Int64 RoleId { get; set; }
        public Int64 UserLoginId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public User _User { get; set; }
        public UserRole _UserRole { get; set; }
        [NotMapped]
        public List<User>  UserList { get; set; }
        [NotMapped]
        public List<UserRole> UserRoleList { get; set; }

    }
}