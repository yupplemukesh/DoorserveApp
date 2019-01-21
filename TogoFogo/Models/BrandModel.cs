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
        public string Is_repair { get; set; }
        [DisplayName("Is Active?")]
        [Required]
        public string Is_Active { get; set; }
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
        public string Created_By { get; set; }
        public string Created_Date { get; set; }
        public string Modify_by { get; set; }
        public string Modify_date { get; set; }
        public string delete_By { get; set; }
        public string delete_Date { get; set; }
        public HttpPostedFileBase BrandIMG { get; set; }
    }
}