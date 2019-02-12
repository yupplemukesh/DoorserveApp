using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class ClientDataModel
    {

        public ClientDataModel()
        {

        }
            

        public SelectList ClientList { get; set; }

        public SelectList ServiceTypeList { get; set; }
        public HttpPostedFileBase DataFile { get; set; }
        [DisplayName("Select Client")]
        public Guid ClientId { get; set; }
        [DisplayName("Select ServiceType")]
        public int ServiceTypeId { get; set; }

    }
}