using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Filters
{
    public class FilterModel
    {
        public Guid? ProviderId { get; set; }
        public Guid? CompId { get; set; }  
        public Guid? RefKey { get; set; }
        public Guid? ClientId { get; set; }
        public int UserId { get; set; }
        public char tabIndex { get; set; }
        public bool IsExport { get; set; }
    }
}