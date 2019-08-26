using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class POOWRRModel:AllData
    {
        public override string TRCFullAddr { get; set; }
        public override string ReversePickupDate { get; set; }
        public override string SelectTrc { get; set; }
        public string cALLBACK { get; set; }
        public string Reject { get; set; }
        public string Pending { get; set; }
        public string EngineerVisit { get; set; }
        public string SchedulePickup { get; set; }
        public string wipedevicedata { get; set; }
        public override string WarrantyStatus { get; set; }
        public override string WarrantyExpiryDate { get; set; }
        [DisplayName("Call Request Number")]
        public override string CcNo { get; set; }
        [DisplayName("Call Request Number")]
        public override string CC_NO { get; set; }
        [DisplayName("Name")]
        public override string Customer_Name { get; set; }
        public override string ServiceProviderName { get; set; }
        [DisplayName("Mobile Number")]
        public override string Mobile_No { get; set; }
        [DisplayName("E-Mail Address")]
        public override string Email_Id { get; set; }
        [DisplayName("Pin Code")]
        public override int Pincode { get; set; }
        [DisplayName("State")]
        public override string Cust_State { get; set; }
        [DisplayName("City / Location")]
        public override string Cust_City { get; set; }
        [DisplayName("Address")]
        public override string Cust_Add { get; set; }
        public override string Brand { get; set; }
        [DisplayName("Model Name")]
        public override string Model { get; set; }
        [DisplayName("Problem")]
        public override string Problem { get; set; }

        public override string CatName { get; set; }

        public override string StatusName { get; set; }
        [DisplayName("Call Request Date")]
        public override string Pickup_Date { get; set; }
        [DisplayName("TUPC")]
        public override string CustomerId { get; set; }
        public override string CurrentStatus { get; set; }
        [DisplayName("Device Type")]
        public override string DeviceType { get; set; }
        [DisplayName("Alternate Number")]
        public override string AltNo { get; set; }
        [DisplayName("Service Charge")]
        public override string BillServiceCharge { get; set; }
        [DisplayName("Spare Cost")]
        public override string BillSpareCost { get; set; }
        [DisplayName("Estimated Cost")]
        public override string BillEstimatedCost { get; set; }
        [DisplayName("Estimated Cost Approved")]
        public override bool IsEstimatedCostApproved { get; set; }
        [DisplayName("Repair Status")]
        public override string RepairStatus1 { get; set; }
        [DisplayName("Collectable Amount")]
        public override string CollectableAmount { get; set; }
        [DisplayName("Payment Mode")]
        public override string PaymentMode1 { get; set; }
        //Submit Data Model
        [DisplayName("Message To Customer")]
        public override string MsgToCust { get; set; }
        public override string SERemarks { get; set; }
        [DisplayName("Serial Number")]
        public override string Serial_No { get; set; }
        public override string IMEI1 { get; set; }
        public override string IMEI2 { get; set; }
        [DisplayName("Service Engineer Action")]
        public override string SE_Action { get; set; }
        [DisplayName("Engineer Visit Date and Time")]
        public override string VisitDatetime { get; set; }
        [DisplayName("Engineer Name")]
        public override string Engg_Name { get; set; }
        [DisplayName("Reverse Pickup Date and Time")]
        public override string Pickupdatetime { get; set; }
        [DisplayName("Courier Name")]
        public override string CourierName { get; set; }
        [DisplayName("Physically Damaged?")]
        public override bool PhysicalDamage { get; set; }
        [DisplayName("Device Warranty Void?")]
        public override string WarrantyVoid { get; set; }
        [DisplayName("Problem Observed")]
        public new string[] PrblmObsrvd { get; set; }
        [DisplayName("Spare Type")]
        public override string SpareType { get; set; }
        [DisplayName("Spare Name")]
        public override string SpareName { get; set; }
        public override string Quantity { get; set; }
        [DisplayName("Service Charge (INR)")]
        public override decimal ServiceCharge { get; set; }
        [DisplayName("Spare Cost (INR)")]
        public override decimal SpareCost { get; set; }
        [DisplayName("Estimated Cost (INR)")]
        public override decimal EstimatedCost { get; set; }
        [DisplayName("Is Estimated Cost Approved?")]
        public override string IsApproved { get; set; }
        [DisplayName("Repair Status")]
        public override string RepairStatus { get; set; }
        [DisplayName("Collectable Amount (INR)")]
        public override string CllectableAmt { get; set; }
        [DisplayName("Payment Mode")]
        public override string PaymentMode { get; set; }
        [DisplayName("Cash Received (INR)")]
        public override string CashRecvd { get; set; }
        [DisplayName("Balance Amount")]
        public override string BalanceAmt { get; set; }
        [DisplayName("Transaction Amount (INR)")]
        public override string TransAmt { get; set; }
        [DisplayName("Transaction Date and Time")]
        public override string TransDateTime { get; set; }
        [DisplayName("Transaction Number")]
        public override string TransNumber { get; set; }
        [DisplayName("Re-Visit Date and Time")]
        public override string RevisitDatetime { get; set; }
        public override string CreatedBy { get; set; }
        public override bool CourierActive { get; set; }
        [DisplayName("Bike Make")]
        public override string BikeMake { get; set; }
        [DisplayName("Message To Customer")]
        public override string MessageCusto { get; set; }
        public override string CourierLogo { get; set; }
        public override string CourierContact { get; set; }
        public override string BikeNumber { get; set; }
        public override string Remarks { get; set; }
        public override string EngineerVisitDate { get; set; }
        public override string UploadedCourierFile { get; set; }
        public override string MobileNumber { get; set; }
        public override bool DeviceWarranty { get; set; }

        public override string CallStatus { get; set; }
        public override string CallBackDatetime { get; set; }
    }
}