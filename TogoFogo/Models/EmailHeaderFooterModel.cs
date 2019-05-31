using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class EmailHeaderFooterModel:RegistrationModel
    {
        public int EmailHeaderFooterId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Action Type Id")]
        public int ActionTypeId { get; set; }
        public string ActionTypeName { get; set; }
        public string ActionTypeIds { get; set; }
        [AllowHtml]
        [DisplayName("Header HTML")]
        public string HeaderHTML { get; set; }
        [AllowHtml]
        [DisplayName("Footer HTML")]
        public string FooterHTML { get; set; }     
        public SelectList ActionTypeList { get; set; }
    }



    public class EmailHeaderFooterMainModel
    {
        public List<EmailHeaderFooterModel> mainModel { get; set; }
        public EmailHeaderFooterModel EmailHeaderFooter { get; set; }


    }
}
