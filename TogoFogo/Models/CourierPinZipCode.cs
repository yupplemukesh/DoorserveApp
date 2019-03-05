using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public SelectList CountryList { get; set; }
        public SelectList CourierList { get; set; }
        public SelectList StateList { get; set; }
        public SelectList CityList { get; set; }
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


        [Required]
        public int Pin_ZIP_ID { get; set; }
        [Required]
        public int CountryID { get; set; }
        [Required]
        public int CourierID { get; set; }
        [Required]
        public int Pin_CountryID { get; set; }
        [Required]
        public int Pin_State { get; set; }
        [Required]
        public int Pin_City { get; set; }
        [Required]
        [DisplayName("Region")]      
        public string Pin_Region { get; set; }
        [Required]
        [DisplayName("Zone")]
        public string Pin_Zone { get; set; }
        [Required]
        [DisplayName("PIN/ZIP Code")]
        public string Pin_Code { get; set; }
        [DisplayName("Courier TAT in days")]
        public string Pin_TAT { get; set; }
        [Required]
        [DisplayName("Is COD?")]
        public Boolean Pin_Cod { get; set; }
        [Required]
        [DisplayName("Short Code")]
        public string ShortCode { get; set; }
        [Required]
        [DisplayName("Is Express?")]
        public Boolean ISExpress { get; set; }
        [Required]
        [DisplayName("Is Reverse Logistics?")]
        public Boolean ReverseLogistics { get; set; }
        [Required]
        [DisplayName("Is Allow Order Preference?")]
        public Boolean OrderPreference { get; set; }
        [Required]
        [DisplayName("Is Active?")]
        public Boolean IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public UserActionRights _UserActionRights { get; set; }
        public List<CourierPinZipCode> _CourierPinZipCodeList { get; set; }
    }

    public class GetData
    {
        public string SpareCode { get; set; }
        public string Part_Image { get; set; }
    }
}