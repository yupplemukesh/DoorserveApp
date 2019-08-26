using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class test
    {

        [DisplayName("Is customer agree to wipe device data?")]
        public string wipedevicedata { get; set; }
        public List<spareTestPFELSForm> TableData { get; set; }
        [DisplayName("Receiving Document")]
        public string ReceivingDoc { get; set; }
        [DisplayName("Schedule Pickup Date and Time")]
        public string ScheduleDate { get; set; }
        [DisplayName("Customer Full Information")]
        public string CustoFullInfo { get; set; }

        [DisplayName("Inward Type")]
        public string InwardType { get; set; }
        [DisplayName("Number of Boxes")]
        public string NoOfBoxes { get; set; }
        [DisplayName("Box Condition")]
        public string BoxCondition { get; set; }
        //[DisplayName("Service Provider Name")]
        //public string ServiceProviderName { get; set; }
        [DisplayName("Purpose?")]

        public string PurPose { get; set; }
        [DisplayName("Actual Pickup Date and Time")]
        public string ActualDate { get; set; }
        [DisplayName("(TRC Information)")]
        public string TRCInfoFull { get; set; }
        [DisplayName("Is Returnable?")]
        public string Returnable { get; set; }
        [DisplayName("Number of Boxes Received")]
        public string NoOfBoxesReceived { get; set; }
        [DisplayName("Upload Photo of Damaged Box")]
        public string DamagedBox { get; set; }

        [DisplayName("Problem Reported")]
        public string ProblemReported { get; set; }
        [DisplayName("Problem Observed")]
        public string ProblemObserved { get; set; }
        [DisplayName("Warrenty Expiry Date")]
        public string WarrentyExpiryDate { get; set; }
        [DisplayName("Warrenty Status")]
        public string WarrentyStatus { get; set; }
        [DisplayName("Is Device Physically Damage Reported?")]
        public string PhysicallyDamageReported { get; set; }
        [DisplayName("Is customer agree to wipe device data?")]
        public string wipedevice { get; set; }
        [DisplayName("Is IW Call Converted to OOW Call?")]
        public string OOWCall { get; set; }
        [DisplayName("Is Estimated Cost Approved?")]
        public string CostApproved { get; set; }
        [DisplayName("Customer Support  Remarks")]
        public string CustomerSupport { get; set; }

        public HttpPostedFileBase PhotoOfDevice1 { get; set; }
        public HttpPostedFileBase DamagedBox1 { get; set; }

        //old

        public string CallRequest { get; set; }
        public string CallRequestDate { get; set; }

        public string Device { get; set; }

        public string TUPC { get; set; }
        public string SerialNo { get; set; }


        public string BatteryRemovable { get; set; }
        public string BatteryRequired { get; set; }
        public string DiffDeviceReceived { get; set; }
        [DisplayName("Received Device")]
        public string ReceivedDevice { get; set; }
        public string ReceivedDeviceBrand { get; set; }
        public string ReceivedDeviceModel { get; set; }
        public string ReceivedDeviceDamaged { get; set; }

        public string ReceiverRemarks { get; set; }
        public string IssuetoEngineer { get; set; }
        public string BatteryReceived { get; set; }
        public string BatteryBrandandModel { get; set; }
        public string ReceivedTUPC { get; set; }
        public string ReceivedSerialNumber { get; set; }
        public string ReceivedIMEI1 { get; set; }
        public string ReceivedIMEI2 { get; set; }
        public string UploadDamagedDevice { get; set; }
        public string NeedReceiveApproval { get; set; }
        public string ReceiveApprovalStatus { get; set; }
        public string ReceiveApprovedBy { get; set; }
        public string ReceiveApprovalDate { get; set; }

        // for PRCC FOrm
        public string SelectTrc { get; set; }
        [DisplayName("Call Request Reject Reason")]
        public string Reject { get; set; }
        public string Pending { get; set; }
        public string EngineerVisit { get; set; }
        public string SchedulePickup { get; set; }
        [DisplayName("Device Type")]
        public string SubCatName { get; set; }
        public string EntryDate { get; set; }
        public string AWBNo { get; set; }
        public string ProcessName { get; set; }
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
        public string Current_Status { get; set; }

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

        [DisplayName("Problem Observed")]
        public string PrblmObsrvd { get; set; }

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



        public string TRCFullAddr { get; set; }
        public string ReversePickupDate { get; set; }
        public string TrcProcess_Name { get; set; }
        public string TrcName { get; set; }
        public string TrcMobile { get; set; }
        public string TrcEmail { get; set; }
        public string TrcAddress { get; set; }
        public string TrcPinCode { get; set; }
        public string TrcLocality { get; set; }
        public string TrcNear_By_Location { get; set; }
        public string WarrantyStatus { get; set; }
        public string WarrantyExpiryDate { get; set; }
        public string WipeDeviceData { get; set; }
        public string EstimatedCostApproved { get; set; }
        [DisplayName("TRC City/Location")]
        public string TRCCity { get; set; }

        // SubmitPart
        [DisplayName("Is Battery Removable?")]
        public string BtryRemovable { get; set; }
        [DisplayName("Is Battery Required?")]
        public string BtryReq { get; set; }
        [DisplayName("Is Battery Received?")]
        public string BtryReceived { get; set; }
        [DisplayName("Battery Brand and Model")]
        public string BtryBrandAndModel { get; set; }
        [DisplayName("Is Different Device Received?")]
        public string IsDiffDeviceRecvd { get; set; }
        [DisplayName("Received Device TUPC")]
        public string DeviceTUPC { get; set; }
        [DisplayName("Received Device")]
        public string RecvdDevice { get; set; }
        [DisplayName("Received Device Brand")]
        public string RecvdBrand { get; set; }
        [DisplayName("Received Device Model")]
        public string RecvdModel { get; set; }
        [DisplayName("Received Device Serial Number")]
        public string RecvdSerialNo { get; set; }
        [DisplayName("Received Device IMEI 1")]
        public string RecvdIMEI1 { get; set; }
        [DisplayName("Received Device IMEI 2")]
        public string RecvdIMEI2 { get; set; }
        [DisplayName("Is Received Device Physically Damaged?")]
        public string IsPhyDamage { get; set; }
        [DisplayName("Upload Photo of Damaged Device")]
        public string PhotoOfDevice { get; set; }
        [DisplayName("Receive Date")]
        public string ReceiveDate { get; set; }
        [DisplayName("Is Need Receive Approval?")]
        public string ReceiveApprovalNeeded { get; set; }
        [DisplayName("Receiver Remarks")]
        public string RecvRemarks { get; set; }
        [DisplayName("Is Approve for Receiving Device")]
        public string IsApproveforReceivingDevice { get; set; }
        [DisplayName("Approver Remarks")]
        public string ApproverRemarks { get; set; }
        [DisplayName("Issue to Engineer")]
        public string EnggName { get; set; }
        [DisplayName("Receive Approval Status")]
        public string RecvApprovalStatus { get; set; }
        [DisplayName("Receive Approval Date")]
        public string RecvApprovalDate { get; set; }
        [DisplayName("Receive Approved By")]
        public string RecvApprovedBy { get; set; }

        public string DamagedBoxImage { get; set; }

        //Repair Information
        [DisplayName("Job Date")]
        public string JOBDate { get; set; }
        [DisplayName("Call Status")]
        public string CallStatus { get; set; }
        [DisplayName("Warranty Sticker Tempered")]
        public string WarrantyStickerTempered { get; set; }
        [DisplayName("Current Status")]
        public string CurrentStatus { get; set; }
        public string CallBackDatetime { get; set; }
        [DisplayName("Job Number")]
        public string JobNumber { get; set; }
        [DisplayName("Device Water Damaged")]
        public string DeviceWaterDamaged { get; set; }
        [DisplayName("Spare Type")]
        public string SpareType { get; set; }
        [DisplayName("Spare Name")]
        public string SpareName { get; set; }
        public string Quantity { get; set; }
        [DisplayName("Engineer Remarks")]
        public string EngineerRemarks { get; set; }
        [DisplayName("Approved Spare Cost")]
        public string ApprovedSpareCost { get; set; }
        public string NeedApprovalofSpareCost { get; set; }
        public string AdditionalSpareCost { get; set; }
        public string TotalEstimatedSpareCost { get; set; }
        [DisplayName("Device Service Charge")]
        public string DeviceServiceCharge { get; set; }
        public string CurrentRepairCostApproved { get; set; }
        public string ApprovedRepairCost { get; set; }
        public string NeedApprovalofRepairCost { get; set; }
        public string AdditionalRepairCost { get; set; }
        public string TotalEstimatedRepairCost { get; set; }
        [DisplayName("Device Warranty Void?")]
        public string WarrantyVoid { get; set; }
        [DisplayName("Problem Found")]
        public string[] ProblemFound { get; set; }

        [DisplayName("Is OS/Software Reinstall Required? ")]
        public string OS_SoftwareReinstall { get; set; }
        [DisplayName("Is Customer Data Backup Done?")]
        public string CustomerDataBackup { get; set; }
        [DisplayName("Current OS/Software Name and Version")]
        public string CurrentOS_Software { get; set; }
        [DisplayName("Installed OS/Software Name and Version")]
        public string InstalledOS_Software { get; set; }
        [DisplayName("Service Provider Name")]
        public string ServiceProviderName { get; set; }
        [DisplayName("Call Request Date and Time")]
        public string CallRequestDateTime { get; set; }
        [DisplayName("Call Status")]
        public string CallStatusRepairInfo { get; set; }
        [DisplayName("Current Status")]
        public string CurrentStatusRepairInfo { get; set; }
        public string PfelsEngineerName { get; set; }
        public string PfelsProblemFound { get; set; }
        [DisplayName("Is OS/Software Reinstall Required? ")]
        public string OS_Software_Reinstall { get; set; }
        [DisplayName("Is Customer Data Backup Done?")]
        public string Customer_Data_Backup { get; set; }
        [DisplayName("Current OS/Software Name and Version")]
        public string Current_OS_Software_Name { get; set; }
        [DisplayName("Installed OS/Software Name and Version")]
        public string Installed_OS { get; set; }
        public string DeviceWarrantyVoid { get; set; }

        [DisplayName("Is Repair Cost Approved?")]
        public string Prcc_Is_Repair_Cost_Approved { get; set; }
        [DisplayName("Callback Date and Time")]
        public string Prcc_Callback_Date_and_Time { get; set; }
        [DisplayName("E-Mail/SMS Message To Customer")]
        public string Prcc_EMail_SMS_Message_To_Customer { get; set; }
        [DisplayName("Advance Payment Amount (INR)")]
        public string Prcc_Advance_Payment_Amount { get; set; }
        [DisplayName("Send Company Bank Account Details via")]
        public string Prcc_Send_Company_Bank_Account_Details { get; set; }
        [DisplayName("Is Customer wants to pay Advance?")]
        public string Prcc_Is_Customer_wants_to_pay_Advance { get; set; }
        [DisplayName("Customer Support Remarks")]
        public string Prcc_Customer_Support_Remarks { get; set; }
        [DisplayName("E-Mail/SMS Message To Customer")]
        public string EMail_SMS { get; set; }
    }
}