﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class SMTPGatewayModel
    {
      
        public Int64 GatewayId { get; set; }
        [Required]
        [DisplayName("Gateway Name")]
        public string GatewayName { get; set; }
        [DisplayName("Gateway Type Id")]
        public Int64 GatewayTypeId { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Is Default ?")]
        public bool IsDefault { get; set; }
        [DisplayName("Is Process By AWS ?")]
        public bool IsProcessByAWS { get; set; }
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string SmtpServerName { get; set; }
        [Required]
        public string SmtpUserName { get; set; }
        [Required]
        public string SmtpPassword { get; set; }
        [Required]
        public string PortNumber { get; set; }
        public bool SSLEnabled { get; set; }
        public int AddeddBy { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string LastUpdateBy { get; set; }
    }
}