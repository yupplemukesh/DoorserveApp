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
        public string CustomerType { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Mobile No Required")]
        [DisplayName("Contact Number")]
        public string CustomerContactNumber { get; set; }
        [DisplayName("Alternate Contact Number")]
        public string CustomerAltConNumber { get; set; }
        [DisplayName("Customer Email")]
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Invalid email format.")]
        public string CustomerEmail { get; set; }
        [DisplayName("Customer Address Type")]
        [Required(ErrorMessage = "Address Type Required")]
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
        public string DeviceSubCategory { get; set; }
        [DisplayName("Device Brand")]
        public string DeviceBrand { get; set; }
        [DisplayName("Device Modal")]
        public string DeviceModel { get; set; }
        public string DeviceModelNo { get; set; }
        [DisplayName("DEVICE SLN")]
        public string DeviceSn { get; set; }
        [DisplayName("Date of Purchase")]
        [DataType(DataType.Date)]
        public string DOP { get; set; }
        [DisplayName("DEVICE PURCHASE FROM")]
        public string PurchaseFrom { get; set; }
        [DisplayName("DEVICE IMEI FIRST")]
        public string DeviceIMEIOne { get; set; }
        [DisplayName("DEVICE IMEI SECOND")]
        public string DeviceIMEISecond { get; set; }

        public string CRemark { get; set; }
        public string AspRemark { get; set; }

        public string Status { get; set; }

        public string BillNo { get; set; }
        public  double ? BillAmount { get; set; }
        [DisplayName("Call ID")]
        public string CRN { get; set; }
        public bool? IsRepeat { get; set; }
        public Guid CustomerId { get; set; }
        public bool IsExistingCustomer { get; set; }
        [DisplayName("Customer Type")]
        [Required(ErrorMessage="Customer Type Required")]
        public int CustomerTypeId { get; set; }
        public SelectList CustomerTypeList { get; set; }
        public string SubAppointmentStatus { get; set; }
        public int? AppointmentStatus { get; set; }

        //public AddressDetail address { get; set; }
        [DisplayName("Device Brand")]
        [Required]
        public Guid DeviceId { get; set; }
        [Required(ErrorMessage = "Device Brand is Required")]
        public int DeviceBrandId { get; set; }
        [Required(ErrorMessage = "Device Category is Required")]
        [DisplayName("Device Category")]
        public int DeviceCategoryId { get; set; }

        [Required(ErrorMessage = "Device Sub Category is Required")]
        [DisplayName("Device Sub Category")]
        public int? DeviceSubCategoryId { get; set; }
        [Required(ErrorMessage = "Device Modal Number is Required")]
        [DisplayName("Device Modal Number")]       
        public int DeviceModalId { get; set; }
        public decimal ServiceCharges { get; set;   }

        public decimal TotalCharges { get { return ServiceCharges + PartCharges; } }
        public decimal PartCharges { get; set; }
        [Required(ErrorMessage = "Please select Device Condition")]
        [DisplayName("Device Condition")]
        public int DeviceConditionId { get; set; }
        public string DeviceCondition { get; set; }
        public string Type { get; set; }
        // Previous Call Details 
        public DateTime? PrvCallDate { get; set; }
        public string PrvCallId { get; set; }
        public string PrvProblemDescription { get; set; }

        public string InvoiceFile { get; set; }
        public string JobSheetFile { get; set; }
        public Guid? PrvProviderId { get; set; }
        public Guid? PrvCenterId { get; set; }
        public Guid? PrvEmpId { get; set; }
        public DateTime? ProblemCloseDate { get; set; }
        public string WarranyStatus { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public SelectList ConditionList { get; set; }
        public SelectList CategoryList { get; set; }
        public SelectList SubCategoryList { get; set; }
        public SelectList BrandList { get; set; }
        public SelectList ProductList { get; set; }
        public SelectList StatusList { get; set; }
        public SelectList ProviderList { get; set; }


    }

}