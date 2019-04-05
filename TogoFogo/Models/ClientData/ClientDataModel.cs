﻿using System;
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

       public ClientDataModel()
        {

            ClientList = new SelectList(Enumerable.Empty<SelectListItem>());
            ServiceTypeList= new SelectList(Enumerable.Empty<SelectListItem>());
            DeliveryTypeList= new SelectList(Enumerable.Empty<SelectListItem>());
        }

        public SelectList ClientList { get; set; }
        public SelectList ServiceTypeList { get; set; }
        public SelectList DeliveryTypeList { get; set; }
        [Required]
        [DisplayName("Upload file")]
        public HttpPostedFileBase DataFile { get; set; }       
        public Guid? Id { get; set; }
        [DisplayName("Client")]
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        [DisplayName("Service Type")]
        public int ServiceTypeId { get; set; }
        [Required]
        [DisplayName("Delivery Type")]
        public int DeliveryTypeId { get; set; }
        public int UserId { get; set; }
        
        
    }
}