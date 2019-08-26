using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models.CustomerServiceRecord
{
    public class VisitorsDetailsModel
    {
        public DateTime VisitDate { get; set; }
        public string Engineer { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public int CrNo { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }

        public int VisitorNo { get; set; }
    }
}