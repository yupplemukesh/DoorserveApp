using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class SacCodesModel
    {
        public string Id { get; set; }
        public string SacCodesId { get; set; }
        [Required]
        [DisplayName("Country")]        
        public string CountryId { get; set; }
        [Required]
        [DisplayName("State")]        
        public string StateId { get; set; }
        [Required]
        [DisplayName("Applicable Tax Type")]        
        public string Applicable_Tax_Type { get; set; }
        [Required]
        [DisplayName("Gst Category")]        
        public string GstCategoryId { get; set; }
        [Required]
        [DisplayName("Gst Chapter Heading")]
        public string GstHeading { get; set; }
        [Required]
        [DisplayName("Gst HSN Code")]        
        public string Gst_HSN_Code { get; set; }
        [Required]
        [DisplayName("CTH Number")]        
        public string CTH_Number { get; set; }
        [Required]
        [DisplayName("SAC (Services Accounting Code)")]       
        public string SAC { get; set; }
        [Required]
        [DisplayName("Product Sale Range")]
        public string Product_Sale_Range { get; set; }
        [Required]
        [DisplayName("CGST Rate (%)")]        
        public float CGST { get; set; }
        [Required]
        [DisplayName("SGST/UTGST Rate (%)")]
        public float SGST_UTGST { get; set; }
        [Required]
        [DisplayName("IGST Rate (%)")]
        public float IGST { get; set; }
        [Required]
        [DisplayName("GST Product Category")]
        public string GstProductCat { get; set; }
        [Required]
        [DisplayName("GST Product Sub Category")]
        public string GstProductSubCat { get; set; }
        [Required]
        [DisplayName("GST Description Of Goods")]
        public string Description_Of_Goods { get; set; }        
        [DisplayName("Is Active")]
        public Boolean IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string Cby { get; set; }
        public string Mby { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteDate { get; set; }
    }
}