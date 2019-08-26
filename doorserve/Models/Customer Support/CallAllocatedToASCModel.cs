using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models.Customer_Support
{
    public class CallAllocatedToASCModel:CallAllocatedToASPModel
    {
        public string ServiceCenterName { get; set; }
        public Guid?  ServiceCenterId { get; set; }    
     
    }
}