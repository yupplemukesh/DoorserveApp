﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.ClientData
{
    public class MainClientDataModel
    {
      public ClientDataModel client { get; set; }
      public List<UploadedExcelModel> UploadedData { get; set; }
      public List<FileDetailModel> UploadedFiles { get; set; }
      public UploadedExcelModel NewCallLog { get; set; }
    }

    public class CallsModel
    {
        public List<UploadedExcelModel> OpenedCalls { get; set; }
        public List<FileDetailModel> ClosedCalls { get; set; }

    }
}