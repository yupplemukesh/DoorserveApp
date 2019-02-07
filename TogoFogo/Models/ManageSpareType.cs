using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageSpareType
    {
        public int SerialNo { get; set; }
        public int SpareTypeId { get; set; }
        [Required]
        [DisplayName("Spare Type Name")]
        public string SpareTypeName { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int SortOrder { get; set; }
        [Required]
        [DisplayName("Is Active")]
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCategory { get; set; }
        public int SubCategoryId { get; set; }
        [DisplayName("Category Name")]
        public string CatName { get; set; }
        [DisplayName("Sub Category")]
        public string SubCatName { get; set; }
    }
}