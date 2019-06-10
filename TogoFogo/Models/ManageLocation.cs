using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ManageLocation
    {
        [DisplayName("Serial No")]
        public Int32 SerialNo { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }
        public string LocationId { get; set; }
        [Required]
        [DisplayName("Location Name")]
        public string LocationName { get; set; }
        [Required]
        [DisplayName("State Name")]
        public int StateId { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public int CountryId { get; set; }
        //[Required]
        [DisplayName("Is Active")]
        public Boolean IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public long CreatedBy { get; set; }
        [DisplayName("Created By")]
        public string CBy { get; set; }
        [DisplayName("Created Date")]
        public DateTime AddedOn { get; set; }
        public long ModifyBy { get; set; }
        [DisplayName("Modify By")]
        public string MBy { get; set; }
        [DisplayName("Modify Date")]
        public DateTime? ModifiedOn { get; set; }
        [DisplayName("Delete By")]
        public string DeleteBy { get; set; }
        [DisplayName("Delete Date")]
        public string DeleteDate { get; set; }
        [DisplayName("District Name")]
        public string DistrictName { get; set; }

        public long PinCode { get; set; }

        public SelectList _CountryList { get; set; }
        public SelectList _StateList { get; set; }



    }
}