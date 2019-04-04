using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ClientDataModel
    {

                  
        public SelectList ClientList { get; set; }
        public SelectList ServiceTypeList { get; set; }
        [Required]
        [DisplayName("Upload file")]
        public HttpPostedFileBase DataFile { get; set; }
        [DisplayName("Client")]
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        [DisplayName("Service Type")]
        public int ServiceTypeId { get; set; }
        public int UserId { get; set; }
        public ClientModel _ClientModel { get; set; }
        public ContactPersonModel _ContactPersonModel { get; set; }
        public ClientData.UploadedExcelModel _UploadedExcelModel { get; set; }
      

    }
}