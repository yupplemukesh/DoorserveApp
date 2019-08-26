using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using doorserve.Models;

namespace doorserve.Models
{
    public class ManageStateModel:ContactPersonModel
    {
        public long St_ID { get; set; }
        [DisplayName("State Name")]
        public string St_Name { get; set; }
        [DisplayName("State Code")]
        public string St_Code { get; set; }
        [Required]
        [DisplayName("Country Name")]
        public long St_CntyID { get; set; }
        public long AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string CBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ? ModifiedOn { get; set; }
        public string MBy { get; set; }        
        public string Cnty_Name { get; set; }        
        public string Remarks { get; set; }
        public System.Web.Mvc.SelectList _CountryList { get; set; }
    }
}