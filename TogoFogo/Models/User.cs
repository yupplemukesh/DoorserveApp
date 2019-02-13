using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class User
    {
        public Int64 UserId { get; set; }
        public  string UserName { get; set; }
        public string Password { get; set; }
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }

    }
}