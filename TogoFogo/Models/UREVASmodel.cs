using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class UREVASTable
    {
        [DisplayName("Courier Name")]
        public string CourierName { get; set; }
        [DisplayName("Reverse AWB Number")]
        public string ReverseAWBNumber { get; set; }
        [DisplayName("Reverse AWB Number Used Type")]
        public string ReverseAWBNumberUsedType { get; set; }
        [DisplayName("Open TAT (Days)")]
        public string OpenTatDays { get; set; }
        [DisplayName("Shipment Date and Time")]
        public string shipdateandTime { get; set; }
        [DisplayName("Pickup Date and Time")]
        public string pickupdateandTime { get; set; }
        [DisplayName("Pickup Code")]
        public string PickupCode { get; set; }
        [DisplayName("Status Type")]
        public string StatusType { get; set; }
        [DisplayName("Reverse AWB Status")]
        public string ReverseAWBStatus { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        [DisplayName("Update Type")]
        public string UpdateType { get; set; }
        [DisplayName("Last Update Date and Time")]
        public string LastUpdateDateandTime { get; set; }
       

    }
    public class UREVASmodel
    {
        public string ReverseAWBNumber { get; set; }
        public string Remarks { get; set; }
        public List<UREVASTable> AksTable { get; set; }
        [DisplayName("Call Request Number")]
        public string CC_NO { get; set; }
        [DisplayName("CourierLogo")]
        public string CourierLogo { get; set; }
        [DisplayName("Courier Name")]
        public string CourierName { get; set; }
        [DisplayName("Reverse AWB Status")]
        public string ReverseAWBStatus { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AWBNumber { get; set; }
        public string AWBStatus { get; set; }
        public string OpenReverseAWBStatus { get; set; }
        public string AvgOPenTAT { get; set; }
        public string Status { get; set; }
        public string CourierId { get; set; }
    }
}