using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class VehicleModel
    {
        public Guid? VHId { get; set; }       
        [DisplayName("Vehicle Model")]
        public string VHModel { get; set; }       
        [DisplayName("Vehicle Number")]
        public string VHNumber { get; set; }
        public int? VHTypeId { get; set; }
        public SelectList VehicleTypeList { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleBrand { get; set; }
        public string RcNumber { get; set; }
        public string DrivingLicense { get; set; }
        public string InsuranceExpairyDate { get; set; }
    }
}