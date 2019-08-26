using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class UserModel: User
    {
        [Required]
        //[Range(5, 20, ErrorMessage = "Enter password between 5 to 20")]
        [DataType(DataType.Password)]
        public override string Password { get; set; }


    }
}