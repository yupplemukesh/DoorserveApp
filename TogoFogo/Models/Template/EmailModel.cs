using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.TempleteModel
{
    public class EmailModel
    {
        public Int64 EmailId { get; set; }
        public Guid GUID { get; set; }
        public Int64 PriorityTypeId { get; set; }
        public int GatewayId { get; set; }
        public Int64 PixelId { get; set; }
        [DisplayName("Email From")]
        public string EmailFrom { get; set; }
        [DisplayName("Email To")]
        public string EmailTo { get; set; }
        [DisplayName("Email BCC")]
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public DateTime DatePooled { get; set; }
    }
}