﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doorserve.Models.ClientData;
using doorserve.Models.ServiceCenter;

namespace doorserve.Models.Customer_Support
{
    public class CallToASPModel:MainClientDataModel
    {       
        public List<CallAllocatedToASPModel> AllocatedCalls { get; set; }
        public List<UploadedExcelModel> PendingCalls { get; set; }
       // public SelectList ClientList { get; set; }
       // public SelectList ServiceTypeList { get; set; }
        public AllocateCallModel CallAllocate { get; set; }
        //public Guid ClientId { get; set; }
        //public int ServiceTypeId  { get; set; }
        public Guid AClientId { get; set; }
        public int AServiceTypeId { get; set; }
        


    }
}