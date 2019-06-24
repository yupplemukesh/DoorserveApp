using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class PreviousCallModel
    {
       public DateTime CallDate { get; set; }
       public string CallId { get; set; }
       public string ProblemDescription { get; set; }
       public DateTime ProblemCloseDate{ get; set; }

    }
}