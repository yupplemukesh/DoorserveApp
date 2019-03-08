using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageLocation
    {
        [DisplayName("Serial No")]
        public Int32 SerialNo { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public string CountryName { get; set; }
        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }
        public string LocationId {get;set;}
        [Required]
        [DisplayName("Location Name")]
        public string LocationName {get;set;}
        public string StateId {get;set;}
        public string CountryId {get;set;}
        //[Required]
        [DisplayName("Is Active")]
        public Boolean IsActive {get;set;}
        [DisplayName("Comments")]
        public string Comments {get;set;}
        public long CreatedBy { get; set; }
        [DisplayName("Created By")]
        public string CBy {get;set;}
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }
        public long ModifyBy { get; set; }
        [DisplayName("Modify By")]
        public string MBy {get;set;}
        [DisplayName("Modify Date")]
        public string ModifyDate { get; set; }
        [DisplayName("Delete By")]
        public string DeleteBy { get; set; }
        [DisplayName("Delete Date")]
        public string DeleteDate {get;set;}
        public List<ManageLocation> ManageLocationList { get; set; }
        public UserActionRights _UserActionRights { get; set; }

    }
}