using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TogoFogo.Models.ClientData
{
    public class FileDetailModel
    {
        public int Id { get; set; }
        public int UploadedFileId { get; set; }
        public string UploadedFileName { get; set; }
        public int TotalRecords { get; set; }
        public int UploadedRecords { get; set; }
        public int FailedRecords { get; set; }    
        public DateTime DateAndTime { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDeliveryType { get; set; }
        public ClientModel _ClientModel { get; set; }
    }
}