using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace doorserve.Models
{
    public class ProcessModel:RegistrationModel
    {
        public int ProcessId {get; set;}
        [DisplayName("Process Name")]
        public string ProcessName {get; set;}
        public string ProcessCode {get; set;}
        [DisplayName("Process Owner")]
        public string ProcessOwner {get; set;}        
        
    }
}