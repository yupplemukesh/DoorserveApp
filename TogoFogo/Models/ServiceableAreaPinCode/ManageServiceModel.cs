using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ManageServiceModel
    {
        public List<ServiceOfferedModel> Services { get; set; }
        public ServiceOfferedModel Service { get; set; }
        public ProviderFileModel ImportModel { get; set; }
        public List<ProviderFileModel> Files { get; set; }
        public string BaseUrl { get; set; }
    }
}