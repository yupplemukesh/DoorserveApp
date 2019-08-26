using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace doorserve.Models.ClientData
{
    public class FileDetailModel:ClientDataModel
    {
        public string UploadedFileName { get; set; }
        public int TotalRecords { get; set; }
        public int UploadedRecords { get; set; }
        public int FailedRecords { get; set; }    
        public DateTime UploadedDate { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDeliveryType { get; set; }

    }
}