using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace doorserve.Models
{
    public class BrandModel
    {
        public int SerialNo { get; set; }
        public int BrandId { get; set; }
        [DisplayName("Brand Name")]
        [Required]
        public string BrandName { get; set; }
        [DisplayName("Brand Image")]
        public string BrandImage { get; set; }
        [DisplayName("Brand Description")]
        public string BrandDescription { get; set; }
        [DisplayName("Is Repair?")]
        [Required]
        public Boolean IsRepair { get; set; }
        [DisplayName("Is Active?")]
        [Required]
        public Boolean IsActive { get; set; }
        public string Comments { get; set; }
        [DisplayName("Meta Keyword")]
        public string MetaKeyword { get; set; }
        [DisplayName("Meta Description")]
        public string MetaDescription { get; set; }
        [DisplayName("Meta Title")]
        public string MetaTitle { get; set; }
        [DisplayName("Url Name")]
        public string UrlName { get; set; }
        [DisplayName("Header Description")]
        public string Header { get; set; }
        public string Footer { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifyBy { get; set; }
        [DisplayName("Created By")]
        public string CBy { get; set; }
        [DisplayName("Modify By")]
        public string MBy { get; set; }
        [DisplayName("Modify Date")]
        public string ModifyDate { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public HttpPostedFileBase BrandIMG { get; set; }        
    }
}