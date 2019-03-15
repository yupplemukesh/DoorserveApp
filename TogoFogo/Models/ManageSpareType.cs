using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ManageSpareType
    {
        public ManageSpareType()
        {
            CategoryList = new SelectList(Enumerable.Empty<SelectListItem>());
            SubCategoryList = new SelectList(Enumerable.Empty<SelectListItem>());

        }
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
        public SelectList CategoryList { get; set; }
        public SelectList SubCategoryList { get; set; }
    }
}