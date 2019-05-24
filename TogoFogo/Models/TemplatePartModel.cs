using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class TemplatePartModel:RegistrationModel
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
            
    }



    public class TemplatePartMainModel
    {
        public List<TemplatePartModel> mainModel { get; set; }
        public TemplatePartModel TemplatePart { get; set; }
    


    }
}
