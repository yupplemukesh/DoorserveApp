using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class HFTemplate
    {
        public string TemplateID { get; set; }
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        [DisplayName("Action Name")]
        public string ActionName { get; set; }
        [DisplayName("E-Mail Header/Footer Template Name")]
        public string TemplateName { get; set; }

        public string UsedIn { get; set; }
        [DisplayName("Is Active?")]
        public string IsActive { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string Comments { get; set; }
        [DisplayName("E-Mail Header Contents")]
        public string HeaderContent { get; set; }
        [DisplayName("E-Mail Footer Contents")]
        public string FooterContent { get; set; }
    }
}