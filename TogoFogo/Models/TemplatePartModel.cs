using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class TemplatePartModel
    {
        public int TemplatePartId { get; set; }
        [Required]
        public string TemplatePartName { get; set; }
        [Required]
        [AllowHtml]
        [DisplayName("HTML Part")]
        public string HTMLPart { get; set; }
        [Required]
        [DisplayName("Plain Text Part")]
        public string PlainTextPart { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedOn { get; set; }
        public int AddeddBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
       
    }



    public class TemplatePartMainModel
    {
        public List<TemplatePartModel> mainModel { get; set; }
        public TemplatePartModel TemplatePart { get; set; }
        public UserActionRights Rights { get; set; }


    }
}
