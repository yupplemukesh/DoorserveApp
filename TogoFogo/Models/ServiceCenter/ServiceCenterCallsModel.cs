using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models.Customer_Support;

namespace TogoFogo.Models.ServiceCenter
{
    public class ServiceCenterCallsModel
    {
        public List<CallDetailsModel> PendingCalls { get; set; }
        public List<CallDetailsModel> AcceptedCalls { get; set; }
        public List<CallDetailsModel> RejectedCalls { get; set; }
        public CallDetailsModel CallDetails { get; set; }
        public EmployeeModel employee { get; set; }
    }
}