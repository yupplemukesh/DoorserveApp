using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ServiceCenterList:Rights
    {
       public List<ServiceCenterModel> serviceCenters { get; set; } 
    }
}