using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ColorModel
    {
        [DisplayName("Model Name")]
        public string[] Model{ get; set; }
        public string ModelId { get; set; }
        public SelectList getModelName { get; set; }
        public string ColorId { get; set; }
        [DisplayName("Color Name")]
        public string ColorName { get; set; }
        public string IsActive { get; set; }
        public string Comments { get; set; }
        public string[] pd { get; set; }
        public string[] Brand { get; set; }
        public string BrandId { get; set; }
    }
}