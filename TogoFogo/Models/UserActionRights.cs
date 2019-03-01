using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class UserActionRights
    {
        public Boolean View { get; set; }
        public Boolean Create { get; set; }
        public Boolean Edit { get; set; }
        public Boolean Delete { get; set; }
        public Boolean History { get; set; }
        public Boolean ExcelExport { get; set; }
    }
}