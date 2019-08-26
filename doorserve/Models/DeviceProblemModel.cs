using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class DeviceProblemModel
    {
        public int SerialNo { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [DisplayName("Sub Category")] 
        public string SubCategory { get; set; }
        public int Id { get; set; }
        public int CatId { get; set; }
        public int SubCatId { get; set; }
        public int ProblemID { get; set; }
        [Required]
        public string Problem { get; set; }
        public int SortOrder { get; set; }
        [DisplayName("Is Active?")]
        [Required]
        public Boolean IsActive { get; set; }
        [DisplayName("Created By")]
        public string CBy { get; set; }
        public long CreatedBy { get; set; }
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Modify By")]
        public string MBy { get; set; }
        public long ModifyBy { get; set; }
        [DisplayName("Modifyd Date")]
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        [DisplayName("Sub Category Name")]
        public string SubCatName { get; set; }
        [DisplayName("Category Name")]
        public string CatName { get; set; }
        public string User { get; set; }      
         public SelectList CatIdList { get; set; }
        public SelectList SubCatIdList { get; set; }
        public SelectList ProblemList { get; set; }
    }
}