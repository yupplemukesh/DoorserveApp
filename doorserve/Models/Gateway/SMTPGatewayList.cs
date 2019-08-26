using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models.Gateway
{
    public class SMTPGatewayList
    {
        public List<SMTPGatewayModel> SMTPGateway { get; set; }
        public UserActionRights Rights { get; set; }
    }
}