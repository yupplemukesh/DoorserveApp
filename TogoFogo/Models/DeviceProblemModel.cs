using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class DeviceProblemModel
    {
        [Required]
        public string Category { get; set; }
        [Required]
        [DisplayName("Sub Category")] 
        public string SubCategory { get; set; }
        public int Id { get; set; }
        public string CatId { get; set; }
        public string SubCatId { get; set; }
        public string ProblemID { get; set; }
        [Required]
        public string Problem { get; set; }
        public string SortOrder { get; set; }
        [DisplayName("Is Active?")]
        [Required]
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string Createddate { get; set; }
        public string Modifyby { get; set; }
        public string Modifydate { get; set; }
        public string deleteBy { get; set; }
        public string deleteDate { get; set; }
        public string SubCatName { get; set; }
        public string CatName { get; set; }
    }
}