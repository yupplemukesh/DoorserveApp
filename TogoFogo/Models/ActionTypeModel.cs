using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TogoFogo.Repository;

namespace TogoFogo.Models
{
    public class ActionTypeModel
    {
        public int ActionTypeId { get; set; }
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Code")]
        public string Code { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        public DateTime AddedOn { get; set; }
        public int AddeddBy { get; set; }
        public string Comments { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string LastUpdateBy { get; set; }


    }
}