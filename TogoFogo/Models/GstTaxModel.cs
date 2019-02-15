using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class GstTaxModel
    {
        public string State { get; set; }
        public string GstTaxId {get;set;}
        [DisplayName("Country")]
        public string CountryId {get;set;}
        [DisplayName("State/Province/UT")]
        public string StateId {get;set;}
        [DisplayName("Applicable Tax Type")]
        public string Applicable_Tax {get;set;}
        [DisplayName("GST Category")]
        public string GstCategoryId {get;set;}
        [DisplayName("Device Category")]
        public string Device_Cat {get;set;}
        [DisplayName("Device Sub Category")]
        public string Device_SubCat {get;set;}
        [DisplayName("GST Chapter Heading")]
        public string GstHeading {get;set;}
        [DisplayName("GST HSN Code (Chapter Sub-heading)")]
        public string Gst_HSNCode_Id {get;set;}
        [DisplayName("CTH Number (GST Tariff item - HSN Code)")]
        public string CTH_NumberId {get;set;}
        [DisplayName("SAC (Services Accounting Code)")]
        public string SACId {get;set;}
        [DisplayName("Product Sale")]
        public string Product_Sale_Range {get;set;}
        [DisplayName("From")]
        public string Product_Sale_From { get; set; }
        [DisplayName("To")]
        public string Product_Sale_TO { get; set; }
        [DisplayName("CGST Rate (%)")]
        public float CGST {get;set;}
        [DisplayName("SGST/UTGST Rate (%)")]
        public float SGST {get;set;}
        [DisplayName("IGST Rate (%)")]
        public float IGST {get;set;}
        [DisplayName("GST Product Category")]
        public string Product_Cat {get;set;}
        [DisplayName("GST Product Sub Category")]
        public string Product_SubCat {get;set;}
        [DisplayName("GST Description Of Goods")]
        public string Description_Goods {get;set;}
        [DisplayName("GST Applicable Date")]
        public string Gst_Applicable_date {get;set;}
        [DisplayName("Is Active ?")]
        public Boolean IsActive{get;set;}
        
        public string Comments {get;set;}
    }
}