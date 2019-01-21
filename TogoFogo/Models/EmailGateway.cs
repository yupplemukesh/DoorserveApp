using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class EmailGateway
    {
        public string ID { get; set; }
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        [DisplayName("E-Mail Gateway Setting Name")]
        public string SettingName { get; set; }
        public string SettingID { get; set; }
        [DisplayName("From E-Mail ID")]
        public string FromID { get; set; }
        [DisplayName("From Name")]
        public string FromName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        public string Paswrd { get; set; }
        [DisplayName("Outgoing (SMTP) Server Name")]
        public string ServerName { get; set; }
        [DisplayName("Outgoing Mail (SMTP) Server Port Number")]
        public string PortNumber { get; set; }
        [DisplayName("Is SSL Enable?")]
        public string SSLEnable { get; set; }
        [DisplayName("Is Default?")]
        public string IsDefault { get; set; }
        [DisplayName("Is Active?")]
        public string IsActive { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        [DisplayName("Remarks")]
        public string Comments { get; set; }
    }
}