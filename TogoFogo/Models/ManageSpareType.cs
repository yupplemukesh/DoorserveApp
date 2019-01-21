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
        public int SpareTypeId { get; set; }
        [Required]
        [DisplayName("Spare Type Name")]
        public string SpareTypeName { get; set; }
        public int ProductId { get; set; }
        public int SortOrder { get; set; }
        [Required]
        [DisplayName("Is Active")]
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string Created_date { get; set; }
        public string ModifyBy { get; set; }
        public string Modify_Date { get; set; }
        public string DeleteBy { get; set; }
        public string Delete_date { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }
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