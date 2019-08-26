using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ProductModel
    {
        public int SerialNo { get; set; }
        public HttpPostedFileBase BulkProduct { get; set; }
        [Required]
        [DisplayName("Product Color")]
        public string[]  ProductColor { get; set; }
        public string Product_Color { get; set; }
        [DisplayName("Product Color")]
        public string Pro_Color { get; set; }
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }
        [DisplayName("Alternate Product Name")]
        public string AlternateProductName { get; set; }
        [DisplayName("Product Image")]
        public string ProductImage { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }
        [DisplayName("Category Id")]
        public int CategoryID { get; set; }
        [DisplayName("Sub category")]
        public string SubCatName { get; set; }
        public int BrandID { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }
        public string MRP { get; set; }
        [DisplayName("Market Price")]
        public string MarketPrice { get; set; }
        public string TUPC { get; set; }
        [DisplayName("Is Repair?")]
        //[Required]
        public Boolean IsRepair { get; set; }
        [DisplayName("Is Active?")]
        //[Required]
        public Boolean IsActive { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        [DisplayName("Created By")]
        public string CBy { get; set; }
        public long CreatedBy { get; set; }
        [DisplayName("Modify By")]
        public string MBy { get; set; }
        public long ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        [DisplayName("Delete By")]
        public string DeleteBy { get; set; }
        [DisplayName("Delete Date")]
        public string DeleteDate { get; set; }
        public HttpPostedFileBase ProductImg { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCategory { get; set; }
        public string Cat_Family { get; set; }
        public int SubCatId { get; set; }
        public string SubCategoryId { get; set; }
        public string User { get; set; }
        public string Action { get; set; }  
      

        public SelectList _BrandName { get; set; } 
        public SelectList _Category { get; set; }
        public SelectList _ProductColor { get; set; }
        public SelectList _SubCat { get; set; }
    }
}