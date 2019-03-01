using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.Template
{
    public class CommonFilterModel
    {
        public int? ActionTypeId { get; set; }
        public int? MessageTypeId { get; set; }
        public int? TemplateTypeId { get; set; }
    }
}