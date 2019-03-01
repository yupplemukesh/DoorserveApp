using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class DeviceCategoryModel
    {
        [DisplayName("Serial No")]
        public Int32 SerialNo { get; set; }
        public int CatId { get; set; }   
        [DisplayName("Category Name")]
        [Required(ErrorMessage="Enter Category Name")]
        public string CatName { get; set; } 
       //[Required]
        [DisplayName("Is Repair")]
        public Boolean IsRepair { get; set; }
        //[Required]
        [DisplayName("Is Active")]
        public Boolean IsActive { get; set; }
        public string Comments { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public string DeleteBy { get; set; }
        public string Action { get; set; }
        [Required(ErrorMessage ="Enter Sort Order Number")]
        public int SortOrder { get; set; }
        public string ModifyDate { get; set; }
        public List<DeviceCategoryModel> _DeviceCategoryModelList { get; set; }
        public UserActionRights _UserActionRights { get; set; }
    }
}