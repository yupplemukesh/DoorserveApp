using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models
{
    public class ManageBannersModel: ManageBannerUploadModel
    {
       
        public Guid? BannerId { get; set; }              
       // public string BannerLinkURL { get; set; }         
        public string ModifiedBy { get; set; }
        public DateTime ? ModifiedOn { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        [DisplayName("Page Name")]
        public int PageId { get; set; }
        public string PageName { get; set; }
        [DisplayName("Section Name")]
        public Guid? SectionId { get; set; }
        public string SectionName { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }       
        public SelectList PageNameList { get; set; }
        public SelectList SectionNameList { get; set; }
        public SelectList ImageList { get; set; }
        public char Type { get; set; }
       
        public List<ManageBannerUploadModel> ImgDetails { get; set; }
      
       
        


    }
}