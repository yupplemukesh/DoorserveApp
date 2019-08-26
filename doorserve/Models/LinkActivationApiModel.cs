using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class LinkActivationApiModel
    {
        public partial class LinkActivation
        {
           public string Schema { get; set; }
           public string Host { get; set; }
            public string Port { get; set; }

        }
    }
}