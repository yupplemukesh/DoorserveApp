using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class CategoryViewModel
    {
        public UserActionRights Rights { get; set; }
        public string AddCategory { get; set; }
        public string EditCategory { get; set; }
        public string AddSubCategory { get; set; }
        public string EditSubCategory { get; set; }
    }
}