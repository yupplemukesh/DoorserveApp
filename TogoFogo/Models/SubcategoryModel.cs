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
        
        public int SerialNo { get; set; }
       
        public string Device_Category { get; set; }       
        public string CatName { get; set; }
        [Required]
        [DisplayName("Device Category")]
        public int CatId { get; set; }
        [Required]
        [DisplayName("Sub Category")]
        public string SubCatName { get; set; }
        public int SubCatId { get; set; }
        [Required]
        [DisplayName("Sort Order")]
        public int SortOrder { get; set; }
        [DisplayName("Is IMEI-1 Required?")]
        public Boolean IsRequiredIMEI1 { get; set; }
        [DisplayName("Is IMEI-2 Required?")]
        public Boolean IsRequiredIMEI2 { get; set; }
        [DisplayName("IMEI Length")]
        public int IMEILength { get; set; }
        [DisplayName("Is Serial Number Required?")]
        public Boolean IsRequiredSerialNo { get; set; }
        [DisplayName("Serial Number Length")]
        public int SRNOLength { get; set; }
        [Required]
        [DisplayName("Is Repair?")]
        public Boolean IsRepair { get; set; }
        [Required]
        [DisplayName("Is Active?")]
        public Boolean IsActive { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }        
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        
        public string DeviceCategory { get; set; }
        public List<SubcategoryModel> SubcategoryModelList { get; set; }

    }
}