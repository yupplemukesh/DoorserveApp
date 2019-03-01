using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.Template
{
    public class NotificationTemplateModel
    {
        public int TemplateId { get; set; }
        [Required]
        [DisplayName("Template Name")]
        public string TemplateName { get; set; }
        [Required]
        [DisplayName("Template Type Id")]
        public int TemplateTypeId { get; set; }
        [Required]
        [DisplayName("Message Type Id")]
        public int MessageTypeId { get; set; }
        [Required]
        [DisplayName("Priority Type")]
        public string PriorityType { get; set; }
        public int GatewayId { get; set; }
        public int EmailHeaderFooterId { get; set; }
        public int ActionTypeId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
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
    }
}