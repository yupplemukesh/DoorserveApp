using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.ClientData
{
    public class CallsViewModel
    {
        public List<UploadedExcelModel> OpenCalls { get; set; }
        public List<UploadedExcelModel> CloseCalls { get; set; }
    }
}