using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace TogoFogo.Models
{
    public class ManageCountryModel:ContactPersonModel
    {   
        public long Cnty_Id { get; set; }
        [DisplayName("Country Name")]
        public string Cnty_Name { get; set; }
        public string Remarks { get; set; }
        //public bool IsActive { get; set; }
        public long AddedBy { get; set; }
        public string CBy { get; set; }
        public DateTime AddedOn { get; set; }
        public long ModifiedBy { get; set; }
        public string MBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

       
    }
}