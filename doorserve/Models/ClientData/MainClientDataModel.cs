using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doorserve.Models.ServiceCenter;

namespace doorserve.Models.ClientData
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