using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class SacCodesModel
    {

        public int SacCodesId { get; set; }
        [Required]
        [DisplayName("Country")]
        public int CountryId { get; set; }
        [DisplayName("Country")]
        public string Cnty_Name { get; set; }
        [Required]
        [DisplayName("State")]
        public int StateId { get; set; }
        [DisplayName("State")]
        public string St_Name { get; set; }
        [Required]
        [DisplayName("Applicable Tax Type")]        
        public int Applicable_Tax { get; set; }
        [DisplayName("Applicable Tax Type")]
        public string Applicabletax { get; set; }
        [Required]
        [DisplayName("Gst Category")]        
        public string GstCategoryId { get; set; }
        [DisplayName("Gst Category")]
        public string Gstcategory { get; set; }
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
        [DisplayName("Product Sale Range")]
        public string Product_Sale_Range { get; set; }
        [Required]
        [DisplayName("From")]
        public string Product_Sale_From { get; set; }
        [Required]
        [DisplayName("To")]
        public string Product_Sale_TO { get; set; }
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
       // public int DeleteBy { get; set; }
      //  public string DeleteDate { get; set; }

        public SelectList CountryList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList GstList { get; set; }
        public SelectList GstHsnCodeList { get; set; }
        public SelectList AplicationTaxTypeList { get; set; }
        

    }
}