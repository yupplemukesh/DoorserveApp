using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ProcessModel:RegistrationModel
    {
        public int ProcessId {get; set;}
        [DisplayName("Process Name")]
        [Required]
        public string ProcessName {get; set;}
        public string ProcessCode {get; set;}
        [DisplayName("Process Owner")]
        public string ProcessOwner {get; set;} 
        public SelectList CompanyList { get; set; }
        
    }
}