using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageEngineerModel
    {
        [Required]
        public string Trc { get; set; }
        public int EngineerId { get; set; }
        [Required]
        [DisplayName("Engineer Code")]
        public int EngineerCode { get; set; }
        public string EngineerPhoto { get; set; }
        [DisplayName("Engineer Photo")]
        public HttpPostedFileBase EngineerPhoto1 { get; set; }
        public string TrcCode { get; set; }
        public int TrcId { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        [Required]
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }
        [DisplayName("Mobile Number")]
        public string EmpMobileNo { get; set; }
        [DisplayName("Alternate Number")]
        public string EmpAltNo { get; set; }
        [DisplayName("Email-Id")]
        public string EmpEmailId { get; set; }
        public string EmpAddress { get; set; }
        [DisplayName("Joining Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy}")]  
        public DateTime EmpJoiningDate { get; set; }
        [DisplayName("Can Pick Up ?")]
        public string EmpPicUp { get; set; }
        [DisplayName("Bike Model")]
        public string BikeModel { get; set; }
        [DisplayName("Bike Number")]
        public string BikeNumber { get; set; }
        [Required]
        [DisplayName("Is Active")]
        public string IsActive { get; set; }
    }
}