using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ServiceChargeModel
    {

        public int ServiceChargeId {get;set;}
        [DisplayName("Device Category")]
        public int CategoryId {get;set;}
        [DisplayName("Device Sub Category")]
        public int SubCatId {get;set;}
        [DisplayName("Brand")]
        public int BrandId {get;set;}
        [DisplayName("Model Name")]
        public String ModelName { get; set; }
        [DisplayName("Model Name")]
        public int ModalNameId {get;set;}
        public int HSN { get; set; }
        [DisplayName("HSN Code")]
        public int HSNCode {get;set;}
        public int SAC {get;set;}
        public string TRUPC  {get;set;}
        public string Form  {get;set;}
        public int MRP {get;set;}
        [DisplayName("Market Price")]
        public int MarketPrice {get;set;}
        [DisplayName("Service Charge")]
        public string ServiceCharge  {get;set;}
        public string IsActive{get;set;}
        public string User {get;set;}
        public string Action { get; set; }


        //DISPLAY TABLE MODEL
        [DisplayName("Device Category")]
        public string CatName { get; set; }
        [DisplayName("Device Sub Category")]
        public string SubCatName { get; set; }
        [DisplayName("Brand")]
        public string BrandName { get; set; }

    }
}