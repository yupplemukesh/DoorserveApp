using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class SMSGateway
    {
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        public string SettingID { get; set; }
        [DisplayName("SMS Gateway Setting Name")]
        public string SettingName { get; set; }
        [DisplayName("SMS Service Provider Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("SMS Service Provider Login URL")]
        public string LoginURL { get; set; }
        [DisplayName("User ID")]
        public string UserID { get; set; }
        [DisplayName("Password")]
        public string Paswrd { get; set; }
        [DisplayName("SMS Setting URL")]
        public string URLSetting { get; set; }
        [DisplayName("Success Message")]
        public string SuccessMsg { get; set; }
        public string AvailCredit { get; set; }
        [DisplayName("Is Active?")]
        public string IsActive { get; set; }
        [DisplayName("Remarks")]
        public string Comments { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
    }
}