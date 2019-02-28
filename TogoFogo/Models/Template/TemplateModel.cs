using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.Template
{
    public class TemplateModel
    {
        public TemplateModel()
        {

            TemplateList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public int GatewayId { get; set; }
        public string Subject { get; set; }
        public Guid GUID { get; set; }
        public Int64 PriorityTypeId { get; set; }
        public DateTime DatePooled { get; set; }

        //NotificationTemplate

        public int TemplateId { get; set; }
        [DisplayName("Template Name")]
        public string TemplateName { get; set; }
        [DisplayName("Template Type")]
        public string TemplateType { get; set; }
        [DisplayName("Message Type")]
        public string MessageType { get; set; }
        [DisplayName("Priority Type")]
        public string PriorityType { get; set; }
        public int EmailHeaderFooterId { get; set; }
        public int ActionTypeId { get; set; }
        public string Content {get;set;}
        public string ContentMeta { get; set; }
        public string ToEmail { get; set; }
        public string BccEmails { get; set; }
        [DisplayName("Is System Defined ?")]
        public bool IsSystemDefined { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        [DisplayName("Is Deleted ?")]
        public bool IsDeleted { get; set; }
        public DateTime AddedOn { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 ModifiedBy { get; set; }

        //Email

        public Int64 EmailId { get; set; }
        public Int64 PixelId { get; set; }
        [DisplayName("Email From")]
        public string EmailFrom { get; set; }
        [DisplayName("Email To")]
        public string EmailTo { get; set; }
        [DisplayName("Email BCC")]
        public string EmailBCC { get; set; }
        public string EmailBody { get; set; }
        
        //SMS

        public Int64 SmsId { get; set; }
        [DisplayName("Message Text")]
        public string MessageText { get; set; }
        [DisplayName("Sms From")]
        public string SmsFrom { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public SelectList TemplateList { get; set; }


    }
}