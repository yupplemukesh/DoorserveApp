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
        [DisplayName("Country")]
        public string CountryId { get; set; }
        [DisplayName("State")]
        public string StateId { get; set; }
        [DisplayName("Applicable Tax Type")]
        public string Applicable_Tax_Type { get; set; }
        [DisplayName("Gst Category")]
        public string GstCategoryId { get; set; }
        [DisplayName("Gst Chapter Heading")]
        public string GstHeading { get; set; }
        [Required]
        [DisplayName("Gst HSN Code")]
        public string Gst_HSN_Code { get; set; }
        [DisplayName("CTH Number")]
        public string CTH_Number { get; set; }
        [DisplayName("SAC (Services Accounting Code)")]
        public string SAC { get; set; }
        [DisplayName("Product Sale Range")]
        public string Product_Sale_Range { get; set; }
        [DisplayName("CGST Rate (%)")]
        public float CGST { get; set; }
        [DisplayName("SGST/UTGST Rate (%)")]
        public float SGST_UTGST { get; set; }
        [DisplayName("IGST Rate (%)")]
        public float IGST { get; set; }
        [DisplayName("GST Product Category")]
        public string GstProductCat { get; set; }
        [DisplayName("GST Product Sub Category")]
        public string GstProductSubCat { get; set; }
        [DisplayName("GST Description Of Goods")]
        public string Description_Of_Goods { get; set; }
        [Required]
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