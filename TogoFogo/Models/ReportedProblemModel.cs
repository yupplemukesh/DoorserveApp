using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ReportedProblemModel
    {
        public string ProblemDescription { get; set; }
        public DateTime? IssueOcurringSinceDate { get; set; }
        public Guid? RefKey { get; set; }
    }
}