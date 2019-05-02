using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models.ClientData
{
    public class UploadedExcelModel : ClientDataModel
    {
       
        public string ServiceTypeName { get; set; }
        public string DeliveryTypeName { get; set; }
        [Required]
        [DisplayName("Client Name")]
        public string ClientName { get; set; }
        [DisplayName("Uploaded Date")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        [DisplayName("Customer Contact Number")]
        public string CustomerContactNumber { get; set; }
        public string CustomerAltConNumber { get; set; } 
        [DisplayName("Customer Email")]
        [Required]
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

        public string BillNo { get; set; }
        public decimal BillAmount { get; set; }
        [DisplayName("Call ID")]
        public string CRN { get; set; }
        public Guid CustomerId { get; set; }
        public bool IsExistingCustomer { get; set; }
        [DisplayName("Customer Type")]
        [Required]
        public int CustomerTypeId { get; set; }
        public SelectList CustomerTypeList { get; set; }
     
        //public AddressDetail address { get; set; }
        [DisplayName("Device Brand")]
        [Required]
        public Guid DeviceId { get; set; }
        public int DeviceBrandId { get; set; }
        [Required]
        [DisplayName("Device Category")]
        public int DeviceCategoryId { get; set; }
        [Required]
        [DisplayName("Device Modal Number")]
        public int DeviceModalId { get; set; }
        [Required]
        [DisplayName("Device Condition")]
        public int DeviceConditionId { get; set; }
        public SelectList ConditionList { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList BrandList { get; set; }
        public SelectList ProductList { get; set; }

       

    }
}