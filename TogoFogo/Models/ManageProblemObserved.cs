using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageProblemObserved
    {
        public int SerialNo { get; set; }
        [Required]
        public string Device_Category { get; set; }
        [Required]
        [DisplayName("Device Category")]
        public int DeviceCategory { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCategory { get; set; }
        [Required]
        [DisplayName("Problem Code")]
        public string ProblemCode { get; set; }
        public string ProblemId { get; set; }
        [DisplayName("Problem Observed")]
        public string ProblemObserved { get; set; }
        [DisplayName("Sort Order")]
        public int SortOrder { get; set; }
        [Required]
        [DisplayName("Is Active?")]
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public string CatName { get; set; }
        public string SubCatName { get; set; }
    }
}