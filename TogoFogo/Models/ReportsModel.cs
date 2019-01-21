using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class ReportsModel
    {
        [DisplayName("From Date")]
        public string Fromdate { get; set; }
        [DisplayName("To Date")]
        public string ToDate { get; set; }
        [DisplayName("Process Name")]
        public string ProcessName { get; set; }
        [DisplayName("SP Name")]
        public string ServiceProviderName { get; set; }
        [DisplayName("TRC")]
        public string TrcName { get; set; }

    }
    public class ReportsModelTable
    {
        public string CourierName { get; set; }
        public string ProviderName { get; set; }
        public string TRC_NAME { get; set; }
        public string ServiceCharge { get; set; }
        public string SpareCost { get; set; }
        public string EstimatedCost { get; set; }
        public string ServiceEngineerAction { get; set; }
        public string RAWB_ReversePickupDate { get; set; }
        public string RAWB_MessageToCustomer { get; set; }
        public string RAWB_Remarks { get; set; }
        public string Email_SMS_To_Customer { get; set; }
        public string Remarks { get; set; }
        public string SchedulePickupDate { get; set; }
        public string ReversePickUpDate { get; set; }
        public string EngineerVistDate { get; set; }
        public string CallRequestRejectReason { get; set; }
        [DisplayName("Call Status")]
        public string CallStatusName { get; set; }
        public string AddressType { get; set; }
        public string ModelColor { get; set; }
        public string Customer_Name { get; set; }
        public string Mobile_No { get; set; }
        public string Email_Id { get; set; }
        public string PinCode { get; set; }
        public string Entry_Date { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
         public string Process_Name { get; set; }
        public string Cust_Add { get; set; }
        public string Cust_City { get; set; }
        public string Cust_State { get; set; }
        public string CurrentStatus { get; set; }
        public string Device { get; set; }
        public string CC_NO { get; set; }
        public string ProblemReported { get; set; }
    }
}