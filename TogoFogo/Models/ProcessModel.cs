using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models
{
    public class ProcessModel:ContactPersonModel
    {
        public int ProcessId {get; set;}
        [DisplayName("Process Name")]
        public string ProcessName {get; set;}
        public string ProcessCode {get; set;}
        [DisplayName("Process Owner")]
        public string ProcessOwner {get; set;}        
        public int CreatedBy {get; set;}
        public string CBy { get; set; }
        public int ModifyBy {get; set;}
        public string MBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ? ModifyDateTime { get; set; }
        public string Remark { get; set; }
        
    }
}