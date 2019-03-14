using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class MainTableURSSE
    {
        public string PendingRepairCalls { get; set; }
        public string AverageRepairTAT { get; set; }
        public string MaximumOpenTAT { get; set; }
        public string Status { get; set; }
        public List<MainTableURSSE> _MainTableURSSEList { get; set; }
        public UserActionRights _UserActionRights { get; set; }
    }
    public class ReverseAWB_AllocationModel
    {
        public string Updatehistroy { get; set; }
        public string LastUpdateByEmailId { get; set; }
        public string LastUpdateByName { get; set; }
        public string IpAddress { get; set; }
        public string ReverseAWBNumber { get; set; }
        public string ReverseCourierName { get; set; }
        [DisplayName("Call Request Date")]
        public string CallRequestDate { get; set; }
        public string RRNO { get; set; }
        public string SE_Remarks { get; set; }
        public string[] ProblemArray { get; set; }
        [DisplayName("Call Request Number")]
        public string CC_NO { get; set; }
        [DisplayName("Name")]
        public string Customer_Name { get; set; }
        [DisplayName("Mobile Number")]
        public string Mobile_No { get; set; }
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        [DisplayName("Service Provider Name")]
        public string ServiceProviderName { get; set; }
        public string Pickup_Date { get; set; }
        public string StatusName { get; set; }
        [DisplayName("Current Status")]
        public string CurrentStatus { get; set; }
        [DisplayName("E-Mail Address")]
        public string Email_Id { get; set; }
        [DisplayName("Address")]
        public string Cust_Add { get; set; }
        [DisplayName("City / Location")]
        public string Cust_City { get; set; }
        [DisplayName("State")]
        public string Cust_State { get; set; }
        [DisplayName("Alternative Contact Number")]
        public string AltNo { get; set; }
        [DisplayName("Device Type")]
        public string DeviceType { get; set; }
        public string Brand { get; set; }
        [DisplayName("Model Name")]
        public string Model { get; set; }
        public string ModelColor { get; set; }
        public string TUPC { get; set; }
        public string CustomerId { get; set; }
        public string Problem { get; set; }
        public string Serial_No { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        [DisplayName("Call Status")]
        public string CallStatus { get; set; }
        public string CallBackDatetime { get; set; }
        [DisplayName("E-Mail/SMS Message To Customer")]
        public string EMail_SMS { get; set; }
        public string Remarks { get; set; }
        [DisplayName("Service Engineer Action")]
        public string SE_Action { get; set; }
        [DisplayName("Reverse Pickup Date and Time")]
        public string ReversePickupDate { get; set; }
        [DisplayName("Courier Name")]
        public string CourierName { get; set; }
        [DisplayName(" Is Courier Active?")]
        public string CourierActive { get; set; }
        [DisplayName("Bike Make")]
        public string BikeMake { get; set; }
        [DisplayName("Message To Customer")]
        public string MessageCusto { get; set; }
        [DisplayName("Courier Logo")]
        public string CourierLogo { get; set; }
        [DisplayName("Courier Contact")]
        public string CourierContact { get; set; }
        [DisplayName("Bike Number")]
        public string BikeNumber { get; set; }
        [DisplayName("AWB Number")]
        public string AWBNumber { get; set; }
        public string Pincode { get; set; }

    }
}