using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class WildCardModel:RegistrationModel
    {
        public int WildCardId { get; set; }
        [Required]
        [DisplayName("Wild Card")]
        public string WildCard { get; set; }
        [Required]
        public List<int> actionTypes { get; set; }
        public string ActionTypeIds { get; set; }

        public SelectList ActionTypeList{ get; set; }

    }

}



