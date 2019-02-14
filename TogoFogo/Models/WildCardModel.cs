﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class WildCardModel
    {
        public int WildCardId { get; set; }
        [Required]
        [DisplayName("Wild Card")]
        public string WildCard { get; set; }
        [DisplayName("Is Active ?")]
        public bool IsActive { get; set; }
        public int AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}


