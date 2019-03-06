using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.Gateway
{
    public class SMTPGatewayList
    {
        public List<SMTPGatewayModel> SMTPGateway { get; set; }
        public UserActionRights Rights { get; set; }
    }
}