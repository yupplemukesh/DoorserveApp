using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace doorserve.Models.ClientData
{
    public class UploadedExcelModel : ClientDataModel
    {
        public string ServiceTypeName { get; set; }
        public string DeliveryTypeName { get; set; }
        public string ClientName { get; set; }
        [DisplayName("Uploaded Date")]
        public DateTime CreatedOn { get; set; }
        public string CustomerType { get; set; }
        [Required(ErrorMessage = "Please enter Customer Name")]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Please enter Contact Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        [DisplayName("Contact Number")]
        public string CustomerContactNumber { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number")]
        [DisplayName("Alternate Contact Number")]
        public string CustomerAltConNumber { get; set; }
        [DisplayName("Customer Email")]
        [Required(ErrorMessage = "Please enter Customer Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Invalid email format.")]
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
        public string DeviceSubCategory { get; set; }
        [DisplayName("Device Brand")]
        public string DeviceBrand { get; set; }
        [DisplayName("Device Modal")]
        public string DeviceModel { get; set; }
        public string DeviceModelNo { get; set; }
        [DisplayName("DEVICE SLN")]
        public string DeviceSn { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        [DisplayName("Date of Purchase")]
        [DataType(DataType.Date)]
        public string DOP { get; set; }
        [DisplayName("DEVICE PURCHASE FROM")]
        public string PurchaseFrom { get; set; }
       // [RequiredIf("Mobile == null",ErrorMessage = "At least email or phone should be provided.")]
        [DisplayName("DEVICE IMEI FIRST")]
        [NotEqual("DeviceIMEISecond")]
        public string DeviceIMEIOne { get; set; }
        [NotEqual("DeviceIMEIOne")]
        [DisplayName("DEVICE IMEI SECOND")]
        public string DeviceIMEISecond { get; set; }

        public string CRemark { get; set; }
        public string AspRemark { get; set; }

        public string Status { get; set; }

        public string BillNo { get; set; }
        public  decimal ? BillAmount { get; set; }
        [DisplayName("Call ID")]
        public string CRN { get; set; }
        public bool? IsRepeat { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsExistingCustomer { get; set; }
        [DisplayName("Customer Type")]
        [Required(ErrorMessage="Please select Customer Type")]
        public int CustomerTypeId { get; set; }
        public SelectList CustomerTypeList { get; set; }
        public string SubAppointmentStatus { get; set; }
        public int? AppointmentStatus { get; set; }
        public bool IsCancelCall { get; set; } 
        //public AddressDetail address { get; set; }
        [DisplayName("Device Brand")]
        [Required(ErrorMessage = "Please select Device Id")]
        public Guid DeviceId { get; set; }
        [Required(ErrorMessage = "Please select Device Brand")]
        public int DeviceBrandId { get; set; }
        [Required(ErrorMessage = "Please select Device Category")]
        [DisplayName("Device Category")]
        public int DeviceCategoryId { get; set; }

        [Required(ErrorMessage = "Please select Device Sub Category")]
        [DisplayName("Device Sub Category")]
        public int? DeviceSubCategoryId { get; set; }
        [Required(ErrorMessage = "Please enter Device Modal Number")]
        [DisplayName("Device Modal Number")]       
        public int DeviceModalId { get; set; }
        public decimal ServiceCharges { get; set;   }
        public bool IsCTAT { get; set; }
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

    public class NotEqualAttribute : ValidationAttribute
    {
        private string OtherProperty { get; set; }

        public NotEqualAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get other property value
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
            // verify values
            if (Convert.ToString(value).Equals(Convert.ToString(otherValue)))
                return new ValidationResult(string.Format("{0} should not be equal to {1}.", validationContext.MemberName, OtherProperty));
            else
                return ValidationResult.Success;
        }
    }

}