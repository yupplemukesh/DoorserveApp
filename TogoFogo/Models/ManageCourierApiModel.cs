using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ManageCourierApiModel
    {
        public SelectList CountryList { get; set; }
        public SelectList CourierList { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public string Country { get; set; }
        [Required]
        [DisplayName("Courier Name")]
        public string Courier { get; set; }

        public string CourierImage { get; set; }

        public int ID {get; set; } 
        public int API_ID      {get;set;}
        public int CourierID   {get;set;}
        public int CountryID   {get;set;}
        [Required(ErrorMessage = "Username is required")]
        public string Username    {get;set;}
        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        public string Passwrd     {get;set;}
        [Required(ErrorMessage = "Version is required")]
        [DisplayName("Version")]
        public string AppVersion  {get;set;}
        [Required]        
        [DisplayName("Account Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public string AccountNo   {get;set;}
        [Required]
        [DisplayName("Account Pin")]
        public string AccountPIN  {get;set;}
        [Required]
        [DisplayName("Account Entity")]
        public string AccountEntity     {get;set;}
        [Required]
        [DisplayName("Account Country Code")]
        public string AccountCountryCode      {get;set;}
       
        [DisplayName("Is Large Packet?")]
        public Boolean IsLargePacket     {get;set;}
     
        [DisplayName("Is Active?")]
        public Boolean IsActive    {get;set;}
        public string Comments    {get;set;}
        public long CreatedBy {get;set;}
        [DisplayName("CreatedBy")]
        public string CBy { get; set; }
        public DateTime CreatedDate {get; set; }
        public long ModifyBy {get; set; }
        [DisplayName("ModifyBy")]
        public string MBy { get; set; }
        public string ModifyDate {get; set; }
        public string DeleteBy {get; set; }
        public string DeleteDate {get; set; }
        

    }
}