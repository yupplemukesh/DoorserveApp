using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class EmployeeModel
    {
        public Guid EmpId { get; set; }
        [Required]
        [DisplayName("Engineer Code")]
        public int EmpCode { get; set; }
        public string EMPPhoto { get; set; }
        public string EMPPhotoUrl { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsPickUp { get; set; }
        public DateTime EMPDOJ { get; set; }
        public DateTime EMPDOB { get; set; }
        public int UserID { get; set; }
        public string LastUpdateBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        [DisplayName("Department Name")]
        [Required]
        public string EmployeeName { get; set; } 
        public char Action { get; set; }
        public int DepartmentId { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        [Required]
        [DisplayName("Center Name")]
        public Guid RefKey { get; set; }
        public Guid ProviderId { get; set; }
        [Required]
        [DisplayName("Designation Name")]
        public int DesignationId { get; set; }
        [DisplayName("Engineer Photo")]
        public HttpPostedFileBase EMPPhoto1
        {
            get; set;
        }
        public ContactPersonModel Contact { get; set; }
        public VehicleModel Vehicle { get; set; }
        public SelectList DepartmentList  { get; set; }
        public SelectList DeginationList { get; set; }
        public SelectList CenterList { get; set; }
        public SelectList ProviderList { get; set; }


    }
}