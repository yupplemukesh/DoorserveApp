using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using doorserve.Models.ClientData;
using doorserve.Models.ServiceCenter;

namespace doorserve.Models
{
    public class CallAppointmentModel : UploadedExcelModel
    {
        // CallDetailsModel
        // public string AlternateContactNo { get; set; }
        public DateTime ? AppointmentDate { get; set; }
        public string Remark { get; set; }

    }
}