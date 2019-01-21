using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class DynamicCrouselModel
    {
        [DisplayName("Webview Image1")]
        public HttpPostedFileBase Image1 { get; set; }
        [DisplayName("Webview Image2")]
        public HttpPostedFileBase Image2 { get; set; }
        [DisplayName("Webview Image3")]
        public HttpPostedFileBase Image3 { get; set; }
        [DisplayName("Mobileview Image1")]
        public HttpPostedFileBase MobileImageUpload1 { get; set; }
        [DisplayName("Mobileview Image2")]
        public HttpPostedFileBase MobileImageUpload2 { get; set; }
        [DisplayName("Mobileview Image3")]
        public HttpPostedFileBase MobileImageUpload3 { get; set; }
        public string FirstImg { get; set; }
        public string SecondImg { get; set; }
        public string ThirdImg { get; set; }
        public string MobileImage1 { get; set; }
        public string MobileImage2 { get; set; }
        public string MobileImage3 { get; set; }
    }
}