using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class URSSEModel
    {
        public string Process_Name { get; set; }
        public string ServiceProviderName { get; set; }
        public string CcNo { get; set; }
        public string EntryDate { get; set; }
        public string CallStatus { get; set; }
        public string CurrentStatus { get; set; }
        public string ServiceEnggName { get; set; }
        public string TrcName { get; set; }
        public string Mobile_No { get; set; }
        public string BikeMake { get; set; }
        public string BikeNumber { get; set; }
        public string CashInHand { get; set; }
        public string CashNeedToCollect { get; set; }
        public string Customer_Name { get; set; }
        public string AltNo { get; set; }
        public string Email_Id { get; set; }
        public string Cust_Add { get; set; }
        public string Pincode { get; set; }
        public string Cust_City { get; set; }
        public string Cust_State { get; set; }
        public string DeviceType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string ModelColor { get; set; }
        public string TUPCModel { get; set; }
        public string Serial_No { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public string Problem { get; set; }
        public string WS { get; set; }
        public string WarrantyStatus { get; set; }
        public string WarrantyExpiryDate { get; set; }
        public string SE_Action { get; set; }
        public string ReversePickupDate { get; set; }
        public string CourierName { get; set; }
        public string CourierActive { get; set; }
        public string MessageCusto { get; set; }
        public string CourierLogo { get; set; }
        public string CourierContact { get; set; }
        public string SE_Remarks { get; set; }
        public string EngineerVisitDateTime { get; set; }
        public string EngineerName { get; set; }
        public string ServiceCharge { get; set; }
        public string SpareCost { get; set; }
        public string EstimatedCost { get; set; }
        public string IsEstimatedCostApproved { get; set; }
        public string RepairStatus { get; set; }
        public string TotalCost { get; set; }
        public string CollectableAmount { get; set; }
        public string PaymentMode { get; set; }
        public string CashReceived { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionDateTime { get; set; }
        public string TransactionNumber { get; set; }
        public string ReVisitDateTime { get; set; }
        public List<GetProblem_Child_Order_problem> ChildtableDataProblem { get; set; }
        public List<URSSE_Page_Model> URSSE_Table { get; set; }
    }
    public class URSSE_Page_Model
    {
        public string RR_NO { get; set; }
        public string CC_NO { get; set; }
        public string CallStatus { get; set; }
        public string Entry_date { get; set; }
        public string CallBackDate { get; set; }
        public string ServiceEngineerAction { get; set; }
        public string Customer_Name { get; set; }
        public string Mobile_No { get; set; }
        public string AltNumber { get; set; }
        public string EMail_Id { get; set; }
        public string Cust_Add { get; set; }
        public string Cust_State { get; set; }
        public string Cust_City { get; set; }
        public string Pincode { get; set; }
        public string DeviceType { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
    }
}