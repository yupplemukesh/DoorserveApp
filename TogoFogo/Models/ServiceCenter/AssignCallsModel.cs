using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TogoFogo.Models.Customer_Support;

namespace TogoFogo.Models.ServiceCenter
{
    public class AssignCallsModel
    {
        public int UserId { get; set; }
        public List<DeviceModel> SelectedDevices { get; set; }        
        public Guid EmpId { get; set; }
    }
}