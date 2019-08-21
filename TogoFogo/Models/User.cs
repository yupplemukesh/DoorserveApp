﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace TogoFogo.Models
{
    public class User
    {
        public Int64 SerialNo { get; set; }
        public Int64 UserId { get; set; }
        [Required(ErrorMessage = "Enter User Name")]
        [System.Web.Mvc.Remote("RemoteValidationforUserName", "Master", AdditionalFields = "UserId", ErrorMessage = "User Name already exists!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        //[Range(5, 20, ErrorMessage = "Enter password between 5 to 20")]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password is not matched")]
        public string ConfirmPassword { get; set; }
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int UserLoginId { get; set; }
        public string RoleName { get; set; }
        public int UserTypeId { get; set;}
        public ContactPersonModel _ContactPerson { get;set;} 
        public AddressDetail _AddressDetail { get; set; }
        public UserRole _UserRole { get; set; }
        public string RefName { get; set; }
        public ClientModel _ClientModel { get; set; }
        public OrganizationModel _OrganizationModel { get; set; }
        [DisplayName("Email Address")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
       ErrorMessage = "Please Enter Correct Email Address")]
        [Required(ErrorMessage = "Enter Email")]
        [System.Web.Mvc.Remote("RemoteValidationConEmailAddress", "Master", AdditionalFields = "CurrentEmail", ErrorMessage = "Email already exists!")]
        public string ConEmailAddress { get; set; }
        public string CurrentEmail { get; set; }


    }
 }