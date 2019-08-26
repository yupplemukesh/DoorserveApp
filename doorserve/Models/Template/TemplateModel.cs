using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models.Template
{
    public class TemplateModel:RegistrationModel
    {
        public TemplateModel()
        {
            TemplateTypeList = new SelectList(Enumerable.Empty<SelectListItem>());            
            ActionTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
            MessageTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
            PriorityTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
            EmailHeaderFooterList = new SelectList(Enumerable.Empty<SelectListItem>());
            GatewayList = new SelectList(Enumerable.Empty<SelectListItem>());  
            WildCardList= new SelectList(Enumerable.Empty<SelectListItem>());
        }
       
        [Required(ErrorMessage = "Select Mailer Gateway")]
        public Int64 GatewayId { get; set; }
        [Required(ErrorMessage = "Enter Subject")]
        public string Subject { get; set; }
        public Guid? GUID { get; set; }
        [Required(ErrorMessage = "Select Priority")]
        public int PriorityTypeId { get; set; }
        public DateTime DatePooled { get; set; }       
        public int TemplateId { get; set; }
        [Required(ErrorMessage = "Enter Template Name")]
        [DisplayName("Template Name")]
        public string TemplateName { get; set; }
        [Required]
        [DisplayName("Mailer Template Name")]
        public string MailerTemplateName { get; set; }
        [DisplayName("Message Type Name")]
        public string MessageTypeName { get; set; }
        [DisplayName("Action Type Name")]
        public string ActionTypeName { get; set; }
        public List<int> actionTypes { get; set; }
        public string ActionTypeIds { get; set; }
        [Required(ErrorMessage = "Select Mailer Template Type")]
        [DisplayName("Template Type Id")]
        public int TemplateTypeId { get; set; }
        [Required(ErrorMessage = "Select Message Type")]
        [DisplayName("Message Type Id")]
        public int MessageTypeId { get; set; }
        [DisplayName("Priority Type")]
        public string PriorityType { get; set; }
        [Required(ErrorMessage = "Select Email Header Footer Template")]
        public int? EmailHeaderFooterId { get; set; }
        [Required(ErrorMessage = "Select Action Type")]
        public int? ActionTypeId { get; set; }
        public string Content {get;set;}
        public string ContentMeta { get; set; }
        [DisplayName("Email To")]
        public string ToEmail { get; set; }
        [Required(ErrorMessage = "Enter BCC Email")]
        public string BccEmails { get; set; }
        [DisplayName("Is System Defined ?")]
        public bool IsSystemDefined { get; set; }
        [DisplayName("Is Deleted ?")]
        public bool? IsDeleted { get; set; }
      
        public DateTime AddedOn { get; set; }
        public int AddedBy { get; set; }
        //Email
        public Int64 EmailId { get; set; }
        public Int64 PixelId { get; set; }
        [DisplayName("Email From")]
        public string EmailFrom { get; set; }        
        [DisplayName("Email BCC")]
        public string EmailBCC { get; set; }
        [DisplayName("Email CC")]
        public string ToCCEmail { get; set; }
        [AllowHtml]
        public string EmailBody { get; set; }        
        //SMS
        public Int64 SmsId { get; set; }
        [DisplayName("Message Text")]
        public string MessageText { get; set; }
        [DisplayName("Sms From")]
        public string SmsFrom { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ToEmailFile { get; set; }
        public HttpPostedFileBase ToMobileNoFile { get; set; }
        public string UploadedEmail { get; set; }
        public string UploadedMobile { get; set; }
        public int TotalCount { get; set; }
        public int TotalTemplateSchedule { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string ScheduleDateTime { get; set; }
        public SelectList TemplateList { get; set; }
        public SelectList ActionTypeList { get; set; }
        public SelectList MessageTypeList { get; set; }
        public SelectList TemplateTypeList { get; set; }
        public SelectList PriorityTypeList { get; set; }
        public SelectList GatewayList { get; set; }
        public SelectList WildCardList { get; set; }
        public SelectList EmailHeaderFooterList { get; set; }
        public List<TemplateTracker> TemplateTrackers{ get; set; }
  
    }
    public class BindGateway
    {
        public Int64 GatewayId { get; set; }
        public string GatewayName { get; set; }
    }
    public class BindHeaderFooter
    {
        public int HeaderFooterId { get; set; }
        public string HeaderFooterName { get; set; }

    }
    public class TemplateTracker
    {
        public Guid? GUID { get; set; }
        public int TemplateId { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string ScheduleDateTime { get; set; }
        public DateTime? StartDate { get; set; }
        public string StatusCode { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
    }
}