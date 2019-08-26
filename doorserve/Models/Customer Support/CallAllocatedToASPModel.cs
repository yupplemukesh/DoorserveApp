using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doorserve.Models.ClientData;

namespace doorserve.Models.Customer_Support
{
    public class CallAllocatedToASPModel: UploadedExcelModel
    {
    public Guid? providerId { get; set; }
    public string ProviderName { get; set; }
    }
}