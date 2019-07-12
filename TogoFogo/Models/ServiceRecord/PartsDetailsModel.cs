using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class PartsDetailsModel
    {
        public Guid? PartId { get; set; } 
        [Required]
        [DisplayName("Part Number")]
        public string PartNo { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public Guid? RefKey { get; set; }
        public Decimal UnitPrice{get;set;}
        public decimal? Total { get { return UnitPrice * Qty; } }
        public string FileName { get; set; }
        public HttpPostedFileBase PartFile
        {
            get; set;
        }
        public char? Action { get; set; }
        public string Defect { get; set; }
    }
}