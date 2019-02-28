using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.TempleteModel
{
    public class SMSModel
    {
        public Int64 SmsId { get; set; }
        public Guid GUID { get; set; }
        [Required]
        [DisplayName("Message Text")]
        public string MessageText { get; set; }
        public Int64 PriorityTypeId { get; set; }
        public int GatewayId { get; set; }
        [DisplayName("Sms From")]
        public string SmsFrom { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime DatePooled { get; set; }
    }
}