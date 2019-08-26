using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace doorserve.Models
{
    public class ManagePageContentsModel : RegistrationModel
    {
        [Required(ErrorMessage ="Please Select Page Name")]        
        public int PageId { get; set; }
        public string PageName { get; set; }
        [Required(ErrorMessage = "Please Select Section Name")]
        public Guid SectionId { get; set; }
        public string SectionName { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public Guid ContentId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaNameDescription { get; set; }
        public List<ManagePageContentsModel> MainContent { get; set; }
        public ManagePageContentsModel DynamicContent { get; set; }
        public SelectList PageNameList { get; set; }
        public SelectList SectionNameList { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int SerialNo { get; set; }
        public char Type { get; set; }
    }

   
}