using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ServiceChargeModel
    {

        public int ServiceChargeId {get;set;}
        [Required]
        [DisplayName("Device Category")]
        public int CategoryId {get;set;}
        [DisplayName("Device Category")]
        public string CatName { get; set; }
        [Required]
        [DisplayName("Device Sub Category")]
        public int SubCatId {get;set;}
        [DisplayName("Device Sub Category")]
        public string SubCatName { get; set; }
        [Required]
        [DisplayName("Brand")]
        public int BrandId {get;set;}
        [DisplayName("Barand")]
        public string BrandName { get; set; } 
        [Required]
        [DisplayName("Model Name")]
        public int ModalNameId {get;set;}
        [DisplayName("Model Name")]
        public String ModelName { get; set; }       
        [Required]        
        [DisplayName("HSN Code")]
        public int HSNCode {get;set;}          
        // [DisplayName("HSN Code")]
        //public string HsnCodeName { get; set; }
        [Required] 
        [DisplayName("SAC Code")]
        public int SAC {get;set;}
        //[DisplayName("SAC Code")]
       // public string SacName { get; set; }
        [Required]
        public string TRUPC  {get;set;}
        [Required]
        public string Form  {get;set;}
        [Required]
        public int MRP {get;set;}
        [Required]
        [DisplayName("Market Price")]
        public int MarketPrice {get;set;}
        [DisplayName("Service Charge")]
        public string ServiceCharge  {get;set;}
        public Boolean IsActive{get;set;}
        public int CreatedBy {get; set;}
        [DisplayName("Created By")]
        public string CBy {get; set; }
        public DateTime CreatedDate {get; set;}
        public int ModifyBy {get; set;}
        [DisplayName("Modify By")]
        public string MBy {get; set;}
        public DateTime ModifyDate {get; set;}
       // public string User {get;set;}
       // public string Action { get; set; }


        public SelectList DeviceCategoryList {get; set;}
        public SelectList DeviceSubCategoryList {get; set;}
        public SelectList BrandList {get; set;}
        public SelectList ModelNameList {get; set;}

       


    }
}