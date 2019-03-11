using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.Customer_Support
{
    public class CallToASCModel:Rights
    {
        public List<CallAllocatedToASCModel> AllocatedCalls { get; set; }
        public List<CallAllocatedToASPModel> PendingCalls { get; set; }
        public SelectList ClientList { get; set; }
        public SelectList ServiceTypeList { get; set; }
        public SelectList ServiceProviderList { get; set; }
        public AllocateCallModel CallAllocate { get; set; }
        public Guid ClientId { get; set; }
        public int ServiceTypeId { get; set; }
        public Guid ServiceProviderId { get; set; }
        public Guid AClientId { get; set; }
        public int AServiceTypeId { get; set; }
        public Guid AServiceProviderId { get; set; }
    }
}