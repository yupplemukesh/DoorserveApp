using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class CategoryModel
    {
        public int CatId { get; set; }
        public string CatName { get; set; }
        public string Cat_Family { get; set; }
        public string SortOrder { get; set; }
        [DisplayName("Is IMEI-1 Required?")]
        public string IMEI1 { get; set; }
        [DisplayName("Is IMEI-2 Required?")]
        public string IMEI2 { get; set; }
        public string IMEI_Length { get; set; }
        [DisplayName("Is Serial Number Required?")]
        public string Sr_no_req { get; set; }
        [DisplayName("Serial Number Length")]
        public string Sr_No_length { get; set; }
        [DisplayName("Is Repair ?")]
        [Required]
        public string Is_repair { get; set; }
        [DisplayName("Is Active ?")]
        [Required]
        public string Is_Active { get; set; }
        public string Comments { get; set; }
        public string Created_By { get; set; }
        public string Created_date { get; set; }
       
    }
}