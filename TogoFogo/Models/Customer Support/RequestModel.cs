using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models.ClientData;

namespace TogoFogo.Models.Customer_Support
{
    public class CallAllocatedToASPModel: UploadedExcelModel
    {
    public string ProviderProcessName { get; set; }
    public int providerId { get; set; }
    public string ProviderName { get; set; }
    public string providerOrgName { get; set; }
    public string ProviderContactNumber { get; set; }
    }
}