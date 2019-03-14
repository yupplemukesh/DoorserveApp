using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class EmailHeaderFooterModel
    {
        public int EmailHeaderFooterId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Action Type Id")]
        public List<int> ActionTypeId { get; set; }
        public string ActionTypeIds { get; set; }
        [Required]
        [AllowHtml]
        [DisplayName("Header HTML")]
        public string HeaderHTML { get; set; }
        [Required]
        [AllowHtml]
        [DisplayName("Footer HTML")]
        public string FooterHTML { get; set; }
        public bool ISACTIVE { get; set; }
        public DateTime AddedOn { get; set; }
        public int AddeddBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public SelectList ActionTypeList { get; set; }
    }



    public class EmailHeaderFooterMainModel
    {
        public List<EmailHeaderFooterModel> mainModel { get; set; }
        public EmailHeaderFooterModel EmailHeaderFooter { get; set; }


    }
}
