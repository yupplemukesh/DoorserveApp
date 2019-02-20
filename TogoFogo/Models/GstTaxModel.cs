using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class GstTaxModel
    {
        //public string State { get; set; }
        public int GstTaxId {get;set;}
        [DisplayName("Country")]
        public int CountryId {get;set;}
        [Required]
        [DisplayName("Country")]
        public string Cnty_Name { get; set; }
        [DisplayName("State/Province/UT")]
        public int StateId {get;set;}
        [Required]
        [DisplayName("State")]
        public string St_Name { get; set; }
        [Required]
        [DisplayName("Applicable Tax Type")]
        public string Applicable_Tax {get;set;}
        [Required]
        [DisplayName("GST Category")]
        public int GstCategoryId {get;set;}
        [Required]
        [DisplayName("Device Category")]
        public int Device_Cat {get;set;}
        [Required]
        [DisplayName("Device Sub Category")]
        public int Device_SubCat {get;set;}
        [Required]
        [DisplayName("GST Chapter Heading")]
        public string GstHeading {get;set;}
        [Required]
        [DisplayName("GST HSN Code (Chapter Sub-heading)")]
        public string Gst_HSNCode_Id {get;set;}
        [Required]
        [DisplayName("CTH Number (GST Tariff item - HSN Code)")]
        public string CTH_NumberId {get;set;}
        [Required]
        [DisplayName("SAC (Services Accounting Code)")]
        public string SACId {get;set;}
        [Required]
        [DisplayName("Product Sale")]
        public string Product_Sale_Range {get;set;}
        [DisplayName("From")]
        public string Product_Sale_From { get; set; }
        [DisplayName("To")]
        public string Product_Sale_TO { get; set; }
        [Required]
        [DisplayName("CGST Rate (%)")]
        public float CGST {get;set;}
        [Required]
        [DisplayName("SGST/UTGST Rate (%)")]
        public float SGST {get;set;}
        [Required]
        [DisplayName("IGST Rate (%)")]
        public float IGST {get;set;}
        [Required]
        [DisplayName("GST Product Category")]
        public string Product_Cat {get;set;}
        [Required]
        [DisplayName("GST Product Sub Category")]       
        public string Product_SubCat {get;set;}
        [Required]
        [DisplayName("GST Description Of Goods")]
        public string Description_Goods {get;set;}
        [Required]
        [DisplayName("GST Applicable Date")]
        public DateTime Gst_Applicable_date {get;set;}
        [DisplayName("Is Active ?")]
        public Boolean IsActive{get;set;}        
        public string Comments {get;set;}
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string CBy { get; set; }
        public string MBy { get; set; }


        public SelectList CountryList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList GstcategoryList { get; set; }
        public SelectList DeviceCategoryList { get; set; }
        public SelectList DeviceSubCategoryList { get; set; }
        public SelectList GstHSNCodeList { get; set; }
        public SelectList ApplicableTaxTypeList { get; set; }
        public SelectList CTHNumberList { get; set; }
        public SelectList SACList { get; set; }

    }
}