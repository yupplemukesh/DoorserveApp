using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Filters
{
    public class FilterModel
    {
        public Guid? ProviderId { get; set; }
        public Guid? CompId { get; set; }
        public Guid? CenterId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? RefKey { get; set; }
        public Guid? ClientId { get; set; }
        public int UserId { get; set; }
        public char tabIndex { get; set; }
        public bool IsExport { get; set; }
        public string DeviceSN { get; set; }
        public int? CategoryId { get; set; }
        public string IMEI { get; set; }          
        public int? GatewayTypeId { get; set; }
        public Guid? ServiceAreaId { get; set;  }
        public char Type { get; set; } 
        public Guid? FileId { get; set; }
    }
}