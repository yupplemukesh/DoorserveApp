using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models
{
    public class ManageBannerUploadModel:RegistrationModel
    {

        public Guid? BannerUploadId { get; set; }
        public Guid? RefKey { get; set; }
        public string HeaderTitle { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string BannerFileName { get; set; }
        public HttpPostedFileBase BannerFile { get; set; }
        public string BannerLinkURL { get; set; }
        public string BannerFilePath { get; set; }
        public string AltText { get; set; }      
        public int ? SortOrder { get; set; }
        public string FileName { get; set; }
    }
}