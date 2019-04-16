using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.CustomerServiceRecord
{
    public class PartsDetailsModel
    {
        public string PartNo { get; set; }
        public string Description { get; set; }
        public string Qty { get; set; }
        public Decimal UnitPrice{get;set;}
        public int Total { get; set; }
        public string Defect { get; set; }
        public string Verifiedby { get; set; }
    }
}