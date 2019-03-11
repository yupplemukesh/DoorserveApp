using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ServiceProviderList:Rights
    {
        public List<ServiceProviderModel> Providers { get; set; }
      
    }
}