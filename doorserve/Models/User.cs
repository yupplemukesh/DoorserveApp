using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.ComponentModel;

namespace doorserve.Models
{
    public class User:ContactPersonModel
    {
        public User()
        {
            _UserRole = new UserRole();           
        }
        public Int64 SerialNo { get; set; }
        [Required(ErrorMessage = "Enter User Name")]
        [System.Web.Mvc.Remote("RemoteValidationforUserName", "Master", AdditionalFields = "UserId", ErrorMessage = "User Name already exists!")]
        public string UserName { get; set; }
        //[Range(5, 20, ErrorMessage = "Enter password between 5 to 20")]
    
        public string ConfirmPassword { get; set; }
        public int UserLoginId { get; set; }
        public string RoleName { get; set; }


        public List<Guid> SelectedRegions { get; set; }
        public string Regions { get; set; }   
        public UserRole _UserRole { get; set; }
        public string RefName { get; set; }
        public SelectList RegionList { get; set; }
        public OrganizationModel _OrganizationModel { get; set; }
        public string CurrentPassword { get; set; }


    }
 }