using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class SMSGatewayModel
    {
        public int GatewayId { get; set; }
        [Required]
        [DisplayName("Gateway Name")]
        public string GatewayName { get; set; }
        [Required]
        [DisplayName("Configuration Setting")]
        public string ConfigurationSetting { get; set; }
        [DisplayName("Trans Api key")]
        public string TransApikey { get; set; }
        [DisplayName("OTP Api key")]
        public string OTPApikey { get; set; }
        [DisplayName("Success Message")]
        public string SuccessMessage { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        public string Header { get; set; }
        [DisplayName("OTP Sender")]
        public string OTPSender { get; set; }
        [DisplayName("Is Default ?")]
        public bool IsDefault { get; set; }
        public int AddeddBy { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string LastUpdateBy { get; set; }
    }
}