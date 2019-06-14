using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ProviderFileModel
    {
        public Guid? CompanyId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        [Required]
        [DisplayName("Upload file")]
        public HttpPostedFileBase DataFile { get; set; }
        public Guid? ServiceId { get; set; }
        public int TotalRecords { get; set; }
        public int UploadedRecords { get; set; }
        public int FailedRecords { get; set; }  
        public DateTime UploadedDate { get; set; }
    }
}