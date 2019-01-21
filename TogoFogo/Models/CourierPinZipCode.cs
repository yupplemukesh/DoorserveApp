using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class Mass_Upload_Model
    {
        [Required]
        [DisplayName("Country")]
        public string CountryBulk { get; set; }
        [Required]
        [DisplayName("Courier Name")]
        public string CourierBulk { get; set; }
        [Required]
        public HttpPostedFileBase postedFile { get; set; }
        [Required]
        [DisplayName("Country")]
        public string PIN_CountryBulk { get; set; }
        [Required]
        [DisplayName("State/Province/Union Territory")]
        public string PIN_StateBulk { get; set; }
        [Required]
        [DisplayName("City/Location")]
        public string PIN_CityBulk { get; set; }
    }
    public class CourierPinZipCode
    {
        [Required]
        [DisplayName("Country")]
        public string Country { get; set; }
        [Required]
        [DisplayName("Courier Name")]
        public string Courier { get; set; }
        [Required]
        [DisplayName("Country")]
        public string PIN_Country1 { get; set; }
        [Required]
        [DisplayName("State/Province/Union Territory")]
        public string PIN_State1 { get; set; }
        [Required]
        [DisplayName("City/Location")]
        public string PIN_City1 { get; set; }



        public int Pin_ZIP_ID { get; set; }
        public int CountryID { get; set; }
        public int CourierID { get; set; }
        public int Pin_CountryID { get; set; }
        public int Pin_State { get; set; }
        public int Pin_City { get; set; }
        [Required]
        [DisplayName("Region")]
        public string Pin_Region { get; set; }
        [DisplayName("Zone")]
        public string Pin_Zone { get; set; }
        [DisplayName("PIN/ZIP Code")]
        public string Pin_Code { get; set; }
        [DisplayName("Courier TAT in days")]
        public string Pin_TAT { get; set; }
        [DisplayName("Is COD?")]
        public string Pin_Cod { get; set; }
        [DisplayName("Short Code")]
        public string ShortCode { get; set; }
        [DisplayName("Is Express?")]
        public string ISExpress { get; set; }
        [DisplayName("Is Reverse Logistics?")]
        public string ReverseLogistics { get; set; }
        [DisplayName("Is Allow Order Preference?")]
        public string OrderPreference { get; set; }
        [DisplayName("Is Active?")]
        public string IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
    }

    public class GetData
    {
        public string SpareCode { get; set; }
        public string Part_Image { get; set; }
    }
}