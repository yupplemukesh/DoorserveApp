using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class WarrantyModel
    {
        public string SNCTwo { get; set; }
        
    }
    public class BindGatewayModel
    {
        public string SettingID { get; set; }
        public string SettingName { get; set; }
    }
    public class GetCallStatusModel
    {
        public string CallStatusId { get; set; }
        public string CallStatusName { get; set; }
    }
    public class GstHsnCodeDropdown
    {
        public string SacCodesId { get; set; }
        public string Gst_HSN_Code { get; set; }
    }
    public class ManageSparePart
    {
        public int SerialNo { get; set; }
        [DisplayName("Device Model Name")]
        public string ProductName { get; set; }
        public string CatName { get; set; }
        public string SubCatName { get; set; }
        [Required]
        [DisplayName("Category")]
        public string Category { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        [Required]
        [DisplayName("Device Model Name")]
        public string DeviceModelName { get; set; }
        [Required]
        [DisplayName("Spare Type")]
        public string SpareTypeName { get; set; }
        public int SpareTypeId {get;set;}
        public string CategoryId {get;set;}
        public int BrandId {get;set;}
        public int ProductId {get;set;}
        public int PartId {get;set;}
        [Required]
        [DisplayName("Spare Part Name")]
        public string PartName {get;set;}
        [DisplayName("TRUPC")]
        public string TUPC {get;set;}
        public string TGFGCode {get;set;}
        [DisplayName("Spare Code")]
        [Required]
        public string SpareCode {get;set;}
        [DisplayName("CTH Number (GST Tariff item - HSN Code)")]
        public string CTHNo {get;set;}
        [DisplayName("Spare Part Photo")]
        public string Part_Image {get;set;}
        public string SortOrder {get;set;}
        [DisplayName("Is Active")]
        [Required]
        public Boolean IsActive {get;set;}
        public string CreatedBy {get;set;}
        public string CreatedDate {get;set;}
        public string ModifyBy {get;set;}
        public string ModifyDate {get;set;}
        public string DeleteBy {get;set;}
        public string DeleteDate { get; set; }
        public HttpPostedFileBase PartImage1 { get; set; }
    }
}