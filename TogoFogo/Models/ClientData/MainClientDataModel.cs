using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models.ServiceCenter;

namespace TogoFogo.Models.ClientData
{
    public class MainClientDataModel:ReportedProblemModel
    {
      public ClientDataModel Client { get; set; }
      public List<UploadedExcelModel> UploadedData { get; set; }
      public List<FileDetailModel> UploadedFiles { get; set; }
      public CallDetailsModel  NewCallLog { get; set; }
      public CallsViewModel Calls { get; set; }

    }  
}