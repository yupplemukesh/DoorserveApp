using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TogoFogo.Models
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
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        [DisplayName("Last Update By")]
        public string CBy { get; set; }
        public string MBy { get; set; }
        [DisplayName("Last Update Date and Time")]
        public string ModifyDate { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public HttpPostedFileBase BrandIMG { get; set; }
        public List<BrandModel> ListBrandModel { get; set; }
        public UserActionRights _UserActionRights { get; set; }
    }
}