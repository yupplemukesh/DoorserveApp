using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public  static class SessionModel
    {
        public static int UserId { get; set; }
        public static string UserRole { get; set; }
        public static string UserName { get; set; }
        public static int UserTypeId { get; set; }
        public static string UserTypeName { get; set; }
        public static Guid? CompanyId { get; set; }
        public static Guid? RefKey { get; set; }
        public static MenuMasterModel Menues { get; set; }
        public static string LogoUrl { get; set; }
        public static string RefName { get; set; }
    }
}