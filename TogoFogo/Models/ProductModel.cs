using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ProductModel
    {
        public HttpPostedFileBase BulkProduct { get; set; }
        [DisplayName("Product Color")]
        public string[] ProductColor { get; set; }
        public string Product_Color { get; set; }
        [DisplayName("Product Color")]
        public string Pro_Color { get; set; }
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Alternate Product Name")]
        public string Alt_ProductName { get; set; }
        [DisplayName("Product Image")]
        public string ProductImage { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }
        [DisplayName("Category Id")]
        public int Category_ID { get; set; }
        [DisplayName("Sub category")]
        public string SubCatName { get; set; }
        public int Brand_ID { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }
        public string MRP { get; set; }
        [DisplayName("Market Price")]
        public string Market_Price { get; set; }
        public string TUPC { get; set; }
        [DisplayName("Is Repair?")]
        [Required]
        public string Is_repair { get; set; }
        [DisplayName("Is Active?")]
        [Required]
        public string Is_Active { get; set; }
        public string Comments { get; set; }
        public string Created_date { get; set; }
        [DisplayName("Modify By")]
        public string Modify_By { get; set; }
        [DisplayName("Delete By")]
        public string delete_By { get; set; }
        [DisplayName("Delete Date")]
        public string delete_Date { get; set; }
        public HttpPostedFileBase ProductImg { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCategory { get; set; }
        public string Cat_Family { get; set; }
        public int SubCatId { get; set; }
        public string Sub_Cat_Id { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
    }
}