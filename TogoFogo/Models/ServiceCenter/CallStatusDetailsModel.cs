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
        public int? AppointmentStatus { get; set; }
        public DateTime? AppointmentDate { get; set;  }
        public string Remarks { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? CenterId { get; set; }
        public Guid? EmpId { get; set; }
        public string Type { get; set; }
        public int? CStatus { get; set; }
    }
}