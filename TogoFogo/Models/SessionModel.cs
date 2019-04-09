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
        public DateTime CreatedBy { get; set; }
    }
}