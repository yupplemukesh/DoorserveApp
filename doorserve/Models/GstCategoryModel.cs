using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class GstCategoryModel
    {
        public int Id { get; set; }
        public int GstCategoryId { get; set; }
        [Required]
        [DisplayName("Gst Category")]
        public string GSTCategory { get; set; }
        [DisplayName("Is Active")]
        public Boolean IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public int DeleteBy { get; set; }
        public string CRBY { get; set; }
        public string MODBY { get; set; }
        public string DeleteDate { get; set; }
        
    }
}