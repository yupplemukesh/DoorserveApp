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
        public int SerialNo { get; set; }
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
        public int SortOrder { get; set; }
        [DisplayName("Is Active?")]
        [Required]
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        [DisplayName("Modifyd Date")]
        public string ModifyDate { get; set; }
        public string DeleteBy { get; set; }
        public string DeleteDate { get; set; }
        public string SubCatName { get; set; }
        public string CatName { get; set; }
        public string User { get; set; }
        public UserActionRights _UserActionRights { get; set; }
        public  List<DeviceProblemModel> _DeviceProblemModelList { get; set; }

    }
}