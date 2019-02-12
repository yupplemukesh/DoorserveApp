using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageCourierApiModel
    {
        [Required]
        public string Country { get; set; }
        [Required]
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
        [Required]
        [DisplayName("Is Large Packet?")]
        public string IsLargePacket     {get;set;}
        [Required]
        [DisplayName("Is Active?")]
        public string IsActive    {get;set;}
        public string Comments    {get;set;}
        public string CreatedBy {get;set;}
        public string CreatedDate {get; set; }
        public string ModifyBy {get; set; }
        public string ModifyDate {get; set; }
        public string DeleteBy {get; set; }
        public string DeleteDate {get; set; }

    }
}