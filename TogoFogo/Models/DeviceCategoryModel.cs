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
        public int CatId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string CatName { get; set; }
        [Required]
        [DisplayName("Is Repair")]
        public string Is_repair { get; set; }
        [Required]
        [DisplayName("Is Active")]
        public string Is_Active { get; set; }
        public string Comments { get; set; }
        public string Created_By { get; set; }
        public string Modify_By { get; set; }
        public string Delete_By { get; set; }
        public string Action { get; set; }
        public int SortOrder { get; set; }
        public string Modify_Date { get; set; }

    }
}