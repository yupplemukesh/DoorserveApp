using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class CallHistory
    {
        public string Date { get; set; }
        public string AppointmentDate { get; set; }
        public string Status { get; set; }
        public string CStatus { get; set; }
        public string ASCStatus { get; set; }
        public string Remarks { get; set; }       
    }
}