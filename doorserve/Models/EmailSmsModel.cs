using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class EmailSmsModel
    {
        public int UserId { get; set; }
        public int PriorityTypeId { get; set; }
        public long GatewayId { get; set; }
        public string SmsFrom { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public string MessageText { get; set; }
        public string Status { get; set; }
        public string ErrorText { get; set; }
    }
}