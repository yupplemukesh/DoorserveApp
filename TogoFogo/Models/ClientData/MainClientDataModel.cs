using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.ClientData
{
    public class MainClientDataModel
    {
      public ClientDataModel client { get; set; }
      public List<UploadedExcelModel> uploadedData { get; set; }
    }
}