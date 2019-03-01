using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ColorModel
    {
        public int SerialNo { get; set; }
        [DisplayName("Model Name")]
        public string[] Model{ get; set; }
        public string ModelId { get; set; }
        public SelectList getModelName { get; set; }
        public string ColorId { get; set; }
        [DisplayName("Color Name")]
        [Required(ErrorMessage ="Enter Color Name")]
        public string  ColorName { get; set; }
        public Boolean IsActive { get; set; }
        [Required(ErrorMessage = "Enter Comments")]
        public string Comments { get; set; }
        public string[] pd { get; set; }
        public string[] Brand { get; set; }
        public string BrandId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyDate { get; set; }
        public UserActionRights _UserActionRights { get; set; }
        public List<ColorModel> _ColorModelList { get; set; }

    }
}