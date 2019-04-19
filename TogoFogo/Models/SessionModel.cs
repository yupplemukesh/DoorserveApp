using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class SessionModel
    {
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public string UserName { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public Guid? RefKey { get; set; }
        public MenuMasterModel Menues { get; set; }
    }
}