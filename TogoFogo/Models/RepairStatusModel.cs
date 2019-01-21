using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    
    public class RepairStatusModel
    {
        
        [DisplayName("EngineerName")]
        public string EmployeeName { get; set; }
        [DisplayName("Mobile Number")]
        public string EmpMobileNo { get; set; }
        public string BikeModel { get; set; }
        public string BikeNumber { get; set; }
        public string CashInHand { get; set; }
        public string CashCollect { get; set; }
        public string EngineerPhoto { get; set; }
    }

    public class FindByCc
    {
        [DisplayName("Call Request Number")]
        public string CC_NO { get; set; }
    }
    public class GetProblem_Child_Order_problem
    {
        public string ProblemId { get; set; }
        public string ProblemName { get; set; }
        public string Estimated_Price { get; set; }
        public string ModelId { get; set; }
        public string BrandId { get; set; }

    }
    public class EditRepairStatus 
    {
        public string EntryDate { get; set; }
        public List<GetProblem_Child_Order_problem> Pro { get; set; }
     
        [DisplayName("Call Request Number")]
        public string CcNo { get; set; }
        [DisplayName("Call Request Number")]
          public string CC_NO { get; set; }
        [DisplayName("Name")]
        public string Customer_Name { get; set; }
        [DisplayName("Mobile Number")]
        public string Mobile_No { get; set; }
        [DisplayName("E-Mail Address")]
        public string Email_Id { get; set; }
        [DisplayName("Pin Code")]
        public string Pincode { get; set; }
        [DisplayName("State")]
        public string Cust_State { get; set; }
        [DisplayName("City / Location")]
        public string Cust_City { get; set; }
        [DisplayName("Address")]
        public string Cust_Add { get; set; }
        public string Brand { get; set; }
        [DisplayName("Model Name")]
        public string Model { get; set; }
        [DisplayName("Problem")]
        public string Problem { get; set; }

        public string CatName { get; set; }

        public string StatusName { get; set; }
        [DisplayName("Call Request Date")]
        public string Pickup_Date { get; set; }
        [DisplayName("TUPC")]
        public string CustomerId { get; set; }
        public string CurrentStatus { get; set; }
        [DisplayName("Device Type")]
        public string DeviceType { get; set; }
        [DisplayName("Alternate Number")]
        public string AltNo { get; set; }
        [DisplayName("Service Charge")]
        public string BillServiceCharge { get; set; }
        [DisplayName("Spare Cost")]
        public string BillSpareCost { get; set; }
        [DisplayName("Estimated Cost")]
        public string BillEstimatedCost { get; set; }
        [DisplayName("Estimated Cost Approved")]
        public string IsEstimatedCostApproved { get; set; }
        [DisplayName("Repair Status")]
        public string RepairStatus1 { get; set; }
        [DisplayName("Collectable Amount")]
        public string CollectableAmount { get; set; }
        [DisplayName("Payment Mode")]
        public string PaymentMode1 { get; set; }
        //Submit Data Model
        [DisplayName("Message To Customer")]
        public string MsgToCust { get; set; }
        public string SERemarks { get; set; }
        [DisplayName("Serial Number")]
        public string Serial_No { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        [DisplayName("Service Engineer Action")]
        public string SE_Action { get; set; }
        [DisplayName("Engineer Visit Date and Time")]
        public string VisitDatetime { get; set; }
        [DisplayName("Engineer Name")]
        public string Engg_Name { get; set; }
        [DisplayName("Reverse Pickup Date and Time")]
        public string Pickupdatetime { get; set; }
        [DisplayName("Courier Name")]
        public string CourierName { get; set; }
        [DisplayName("Physically Damaged?")]
        public string PhysicalDamage { get; set; }
        [DisplayName("Device Warranty Void?")]
        public string WarrantyVoid { get; set; }
        [DisplayName("Problem Observed")]
        public string[] PrblmObsrvd { get; set; }
        [DisplayName("Spare Type")]
        public string SpareType { get; set; }
        [DisplayName("Spare Name")]
        public string SpareName { get; set; }
        public string Quantity { get; set; }
        [DisplayName("Service Charge (INR)")]
        public string ServiceCharge { get; set; }
        [DisplayName("Spare Cost (INR)")]
        public string SpareCost { get; set; }
        [DisplayName("Estimated Cost (INR)")]
        public string EstimatedCost { get; set; }
        [DisplayName("Is Estimated Cost Approved?")]
        public string IsApproved { get; set; }
        [DisplayName("Repair Status")]
        public string RepairStatus { get; set; }
        [DisplayName("Collectable Amount (INR)")]
        public string CllectableAmt { get; set; }
        [DisplayName("Payment Mode")]
        public string PaymentMode { get; set; }
        [DisplayName("Cash Received (INR)")]
        public string CashRecvd { get; set; }
        [DisplayName("Balance Amount")]
        public string BalanceAmt { get; set; }
        [DisplayName("Transaction Amount (INR)")]
        public string TransAmt { get; set; }
        [DisplayName("Transaction Date and Time")]
        public string TransDateTime { get; set; }
        [DisplayName("Transaction Number")]
        public string TransNumber { get; set; }
        [DisplayName("Re-Visit Date and Time")]
        public string RevisitDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string CourierActive { get; set; }
        [DisplayName("Bike Make")]
        public string BikeMake { get; set; }
        [DisplayName("Message To Customer")]
        public string MessageCusto { get; set; }
        public string CourierLogo { get; set; }
        public string CourierContact { get; set; }
        public string BikeNumber { get; set; }
        public string Remarks { get; set; }
        public string EngineerVisitDate { get; set; }
        public string UploadedCourierFile { get; set; }
        public string MobileNumber { get; set; }
        public string DeviceWarranty { get; set; }

        public string CallStatus { get; set; }
        public string CallBackDatetime { get; set; }
    }

    public class CourierValuesModel
    {
        public string EstimatedPrice { get; set; }
        public string MarketPrice { get; set; }
        public string SpareTypeId { get; set; }
        public string SpareNameID { get; set; }
        public string SpareTypeName { get; set; }
        public string PartName { get; set; }
        public string Part_Image { get; set; }
        public string SpareCode { get; set; }
        public string ProblemId { get; set; }
        public string ProblemName { get; set; }
        public string Estimated_Price { get; set; }
        public string ModelId { get; set; }
        public string BrandId { get; set; }
    }
    public class Repi
    {
        public string CcNo { get; set; }
      
        public string Customer_Name { get; set; }
        public string Mobile_No { get; set; }
      
    }
}