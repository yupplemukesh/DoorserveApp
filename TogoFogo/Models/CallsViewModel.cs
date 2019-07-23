using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models.ClientData;

namespace TogoFogo.Models
{
    public class PendingCallsViewModel
    {
        public char Type { get; set; }
        public List<UploadedExcelModel> Calls { get; set;    }
    }
}