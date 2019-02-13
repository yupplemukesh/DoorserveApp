using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.Gateway
{
    public class GatewayModel
    {
        public GatewayModel(){

            GatewayList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public Int64 GatewayId { get; set; }
        [DisplayName("Gateway Name")]
        public string GatewayName { get; set; }
        [DisplayName("Gateway Type Id")]
        public Int64 GatewayTypeId { get; set; }
        [DisplayName("URL Setting")]
        public string URL { get; set; }
        [DisplayName("Trans Api key")]
        public string TransApikey { get; set; }
        [DisplayName("OTP Api key")]
        public string OTPApikey { get; set; }
        [DisplayName("Success Message")]
        public string SuccessMessage { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        [DisplayName("Is Default ?")]
        public bool IsDefault { get; set; }
        [DisplayName("Is Process By AWS ?")]
        public bool IsProcessByAWS { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SmtpServerName { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string PortNumber { get; set; }
        public string SSLEnabled { get; set; }
        [DisplayName("OTP Sender")]
        public int? OTPSender { get; set; }
        public int AddeddBy { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string LastUpdateBy { get; set; }
        public SelectList GatewayList { get; set; }
    }
}