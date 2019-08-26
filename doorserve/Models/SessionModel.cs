using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public   class SessionModel
    {
        public  int UserId { get; set; }
        public  string UserRole { get; set; }
        public  string UserName { get; set; }
        public  int UserTypeId { get; set; }
        public  string UserTypeName { get; set; }
        public  Guid? CompanyId { get; set; }
        public  Guid? RefKey { get; set; }
        public  MenuMasterModel Menues { get; set; }
        public  string LogoUrl { get; set; }
        public  string RefName { get; set; }
        public  string Email { get; set; }
        public  string Mobile { get; set; }
        public string Password { get; set; }
    }
}