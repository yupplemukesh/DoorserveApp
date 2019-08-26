using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class PreviousCallModel:WarrantyInformationModel
    {
       public DateTime? CallDate { get; set; }
       public string CallId { get; set; }
        public Guid? RefKey{ get; set; }

        public string ProblemDescription { get; set; }
       public DateTime? ProblemCloseDate{ get; set; }

    }
}