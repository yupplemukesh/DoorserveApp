using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using TogoFogo.Models.Customer_Support;

namespace TogoFogo.Models.ServiceCenter
{
    public class CallDetailsModel: CallAllocatedToASCModel
    {
        public CallDetailsModel()
        {
            StatusList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        
        public int StatusId { get; set; }
       
        public SelectList StatusList { get; set; }
        [Required]
        public string RejectionReason { get; set; }

    }
}