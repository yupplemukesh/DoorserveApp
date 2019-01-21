using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models

{
    public class SubcategoryModel
    {
        public string Device_Category { get; set; }
        public string CatName { get; set; }
        public int CatId { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCatName { get; set; }
        public int SubCatId { get; set; }
        [Required]
        [DisplayName("Sort Order")]
        public int SortOrder { get; set; }
        [DisplayName("Is IMEI-1 Required?")]
        public string IMEI1 { get; set; }
        [DisplayName("Is IMEI-2 Required?")]
        public string IMEI2 { get; set; }
        [DisplayName("IMEI Length")]
        public int IMEI_Length { get; set; }
        [DisplayName("Is Serial Number Required?")]
        public string Sr_no_req { get; set; }
        [DisplayName("Serial Number Length")]
        public int Sr_No_Length { get; set; }
        [Required]
        [DisplayName("Is Repair?")]
        public string Is_repair { get; set; }
        [Required]
        [DisplayName("Is Active?")]
        public string Is_Active { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public string Created_By { get; set; }
        public string Created_date { get; set; }
        public string Modify_by { get; set; }
        [DisplayName("Last update Details")]
        public string Modify_date { get; set; }
        public string delete_By { get; set; }
        public string delete_Date { get; set; }
        [Required]
        [DisplayName("Device Category")]
        public string DeviceCategory { get; set; }
    }
}