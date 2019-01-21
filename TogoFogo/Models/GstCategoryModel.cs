using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class GstCategoryModel
    {
        public int Id { get; set; }
        public int GstCategoryId { get; set; }
        [Required]
        [DisplayName("Gst Category")]
        public string GSTCategory { get; set; }
        [DisplayName("Is Active")]
        public string IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
    }
}