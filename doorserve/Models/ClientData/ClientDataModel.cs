using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models
{
    public class ClientDataModel:AddressDetail
    {

       public ClientDataModel()
        {

            ClientList = new SelectList(Enumerable.Empty<SelectListItem>());
            ServiceTypeList= new SelectList(Enumerable.Empty<SelectListItem>());
            DeliveryTypeList= new SelectList(Enumerable.Empty<SelectListItem>());
        }

        public SelectList ClientList { get; set; }
        public SelectList ServiceTypeList { get; set; }
        public SelectList DeliveryTypeList { get; set; }
  
         
        public Guid? Id { get; set; }

        public Guid? ClientId { get; set; }
        [Required(ErrorMessage = "Please select Service Type")]
        [DisplayName("Service Type")]
        public int ServiceTypeId { get; set; }
        public string DataSource { get; set; }
        public string ProcessName { get; set; }
        public bool IsClientAddedBy { get; set; } 
        public string FileName { get; set; }

        [Required(ErrorMessage = "Please select Delivery Type")]
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