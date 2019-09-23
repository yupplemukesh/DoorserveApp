using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace doorserve.Models.ClientData
{
    public class FileDetailModel:RegistrationModel
    {
        public FileDetailModel()
        {

            ClientList = new SelectList(Enumerable.Empty<SelectListItem>());
            ServiceTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
            DeliveryTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public string UploadedFileName { get; set; }
        public int? TotalRecords { get; set; }
        public int? UploadedRecords { get; set; }
        public int? FailedRecords { get; set; }    
        public DateTime UploadedDate { get; set; }
        public string ServiceType { get; set; }
        [Required]
        public HttpPostedFileBase DataFile { get; set; }
        public string ServiceDeliveryType { get; set; }
       

        public SelectList ClientList { get; set; }
        public SelectList ServiceTypeList { get; set; }
        public SelectList DeliveryTypeList { get; set; }

        public Guid? Id { get; set; }
        [Required]
        [DisplayName("Client")]
        public Guid? ClientId { get; set; }
        [Required]
        [DisplayName("Service Type")]
        public int ServiceTypeId { get; set; }
        public string DataSource { get; set; }
        public string ProcessName { get; set; }
        public bool IsClientAddedBy { get; set; }
        public string FileName { get; set; }

        [Required]
        [DisplayName("Delivery Type")]
        public int DeliveryTypeId { get; set; }
        public string UploadedBy { get; set; }
        // public int UserId { get; set; }
        public string GETAssignedCalls { get; set; }
        public bool IsClient { get; set; }
        //public Guid? CompanyId { get; set; }
        public int DataSourceId { get; set; }


    }
}