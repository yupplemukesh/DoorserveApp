using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.Customer_Support
{
    public class CallAllocatedToASCModel:CallAllocatedToASPModel
    {
        public string ServiceCenterName { get; set; }
        public int ServiceCenterId { get; set; }    
        public string ServiceCenterOrgName { get; set; }
        public string ServiceCenterProcessName { get; set; }
        public string ServiceCenterContactNumber { get; set; }
    }
}