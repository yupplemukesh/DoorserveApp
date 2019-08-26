using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class IVRTemplate
    {
        public string TemplateID { get; set; }
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        [DisplayName("Action Name")]
        public string ActionName { get; set; }
        [DisplayName("Template Name")]
        public string TemplateName { get; set; }
        public string UsedIn { get; set; }
        [DisplayName("Message Type")]
        public string MsgType { get; set; }
        [DisplayName("Header/Footer Template Name")]
        public string HFTemplateName { get; set; }
        [DisplayName("Gateway")]
        public string Gateway { get; set; }

        public string SSLEnable { get; set; }
        [DisplayName("Is System E-Mail?")]
        public string IsSystemEmail { get; set; }
        [DisplayName("Is System SMS?")]
        public string IsSystemSMS { get; set; }
        [DisplayName("To E-Mail ID")]
        public string ToEmailID { get; set; }
        [DisplayName("To Mobile Number")]
        public string ToMobileNo { get; set; }
        [DisplayName("BCC E-Mail ID")]
        public string BCCEmailID { get; set; }
        [DisplayName("E-Mail Subject")]
        public string EmailSubject { get; set; }
        public string Remarks { get; set; }
        [DisplayName("Is Active?")]
        public string IsActive { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        [DisplayName("Remarks")]
        public string Comments { get; set; }
        [DisplayName("Message Contents")]
        public string MsgContent { get; set; }
        [DisplayName("Priority")]
        public string Priority { get; set; }
    }
}