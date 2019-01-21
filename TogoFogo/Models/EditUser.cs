using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class EditUser
    {
        [Required(ErrorMessage = "Name can't be blank")]
        [Display(Name = "Name")]
        public string CUSTOMER_NAME { get; set; }
        [Required(ErrorMessage = "Mobile can't be blank")]
        [Display(Name = "Mobile")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter only 10 digit mobile number!!")]
        public string MOBILE { get; set; }
        [Display(Name = "Email ID")]
        public string ALTERNATE_NUMBER { get; set; }
        [Display(Name = "Address")]
        public string ADDR { get; set; }
        [Display(Name = "Gender")]
        public string GENDER { get; set; }
        [Display(Name = "Region ")]
        public string OCCUPATION { get; set; }
        [Display(Name = "Sub Region")]
        public string REMARKS { get; set; }
        [Display(Name = "Reporting To")]
        public string ENTRY_USER { get; set; }

        public string[] ALL_PRODUCTS { get; set; }
    }
}