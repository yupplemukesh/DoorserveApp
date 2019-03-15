using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ManageProblemObserved
    {
        public ManageProblemObserved()
        {
            CategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
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
        public long CreatedBy { get; set; }
        [DisplayName("Created By")]
        public string CBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ModifyBy { get; set; }
        [DisplayName("Modify By")]
        public string MBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public string CatName { get; set; }
        public string SubCatName { get; set; }       
        public SelectList CategoryList { get; set; }
        public SelectList SubCategoryList { get; set; }
    }
}