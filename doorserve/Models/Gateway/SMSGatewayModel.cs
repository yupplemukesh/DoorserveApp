using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class SMSGatewayModel:RegistrationModel
    {
        public SMSGatewayModel()
        {
            GatewayList = new SelectList(Enumerable.Empty<SelectListItem>());
            CompanyList= new SelectList(Enumerable.Empty<SelectListItem>());
        }
    
        public Int64 GatewayId { get; set; }
        [Required]
        [DisplayName("Gateway Name")]
        public string GatewayName { get; set; }
        [DisplayName("Gateway Type Id")]
        public Int64 GatewayTypeId { get; set; }
        [Required]
        [DisplayName("URL Setting")]
        public string URL { get; set; }
        [Required]
        [DisplayName("Trans Api key")]
        public string TransApikey { get; set; }
        [DisplayName("OTP Api key")]
        public string OTPApikey { get; set; }
        [Required]
        [DisplayName("Success Message")]
        public string SuccessMessage { get; set; }
        [DisplayName("OTP Sender")]
        public int? OTPSender { get; set; }
        public SelectList GatewayList { get; set; }
        public SelectList CompanyList { get; set; }

    }



    public class SMSGateWayMainModel
    {
        public List<SMSGatewayModel> mainModel { get; set; }
        public SMSGatewayModel Gateway { get; set; }

    }
}

