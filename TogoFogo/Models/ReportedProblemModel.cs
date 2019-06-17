using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models.ServiceCenter;

namespace TogoFogo.Models
{
    public class ReportedProblemModel:CallDetailsModel
    {
       // public string ProblemDescription { get; set; }
       // public DateTime? IssueOcurringSinceDate { get; set; }
        public Guid? RefKey { get; set; }
       // public bool IsRepeat { get; set; }
    }
}