using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.ClientData
{
    public class UploadedExcelModel
    {
        public Guid ClientId { get; set; }
        public Guid DeviceId { get; set; }
        public int SERVICETYPEID  { get; set; }
        [DisplayName("Client Name")]
        public string ClientName { get; set; }
        [DisplayName("Uploaded Date")]
        public DateTime  CreatedOn { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Customer Contact Number")]
        public string CustomerContactNumber { get; set; }
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }
        [DisplayName("Customer Address Type")]
        public string CustomerAddressType { get; set; }
        [DisplayName("Customer Address")]
        public string CustomerAddress { get; set; }
        [DisplayName("Customer Country")]
        public string CustomerCountry { get; set; }
        [DisplayName("Customer State")]
        public string CustomerState { get; set; }
        [DisplayName("Customer City")]
        public string CustomerCity { get; set; }
        [DisplayName("Customer Pincode")]
        public string CustomerPincode { get; set; }
        [DisplayName("Device Category")]
        public string DeviceCategory { get; set; }
        [DisplayName("Device Brand")]
        public string DeviceBrand { get; set; }
        [DisplayName("Device Modal")]
        public string DeviceModel { get; set; }
        [DisplayName("DEVICE SLN")]
        public string DeviceSN { get; set; }
        [DisplayName("DEVICE DOP")]
        public DateTime DOP { get; set; }
        [DisplayName("DEVICE PURCHASE FROM")]
        public string PurchaseFrom { get; set; }
        [DisplayName("DEVICE IMEI FIRST")]
        public string DeviceIMEIOne { get; set; }
        [DisplayName("DEVICE IMEI SECOND")]
        public string DeviceIMEISecond { get; set; }
        [DisplayName("Call ID")]
        public string CRN { get; set; }
    }
}