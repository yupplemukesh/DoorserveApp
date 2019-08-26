using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doorserve.Models.Customer_Support;

namespace doorserve.Models.ServiceCenter
{
    public class CallStatusModel
    {
        public int UserId { get; set; }
        public List<DeviceModel> SelectedDevices { get; set; }
        public string RejectionReason { get; set; }
        public int StatusId { get; set; }
        public Guid? CompId { get; set; }
        public Guid? RefKey { get; set; }
    }
}