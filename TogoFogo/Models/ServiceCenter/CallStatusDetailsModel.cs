using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.ServiceCenter
{
    public class CallStatusDetailsModel
    {
        public int UserId { get; set; }
        public Guid DeviceId { get; set; }
        public string RejectionReason { get; set; }
        public int CStatus { get; set; }
        public Guid EmpId { get; set; }
    }
}