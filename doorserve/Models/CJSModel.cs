using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class CJSModel
    {
        [DisplayName("Repair Warranty ")]
        public string WS { get; set; }
        [DisplayName("Customer Pickup Date ")]
        public string customerPickupdate { get; set; }
        public string CallRequestRejectReason { get; set; }


      
        public string AWBNumber { get; set; }
        public string ModelColor { get; set; }
        public string IP_Address { get; set; }
        public string Process_Name { get; set; }
      
        public string wipedevice { get; set; }

        [DisplayName("Device Type")]
        public string SubCatName { get; set; }
        public string EntryDate { get; set; }
        [DisplayName("AWB Number")]
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
        public bool DeviceWarranty { get; set; }



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
        [DisplayName("Engineer Remarks")]
        public string Repair_EngineerRemarks { get; set; }
        [DisplayName("Approved Spare Cost")]
        public string ApprovedSpareCost { get; set; }
        [DisplayName("Need Approval of Spare Cost")]
        public string NeedApprovalofSpareCost { get; set; }
        [DisplayName("Additional Spare Cost")]
        public string AdditionalSpareCost { get; set; }
        [DisplayName("Total Estimated Spare Cost")]
        public string TotalEstimatedSpareCost { get; set; }
        [DisplayName("Device Service Charge")]
        public string DeviceServiceCharge { get; set; }
        [DisplayName("Current Repair Cost Approved")]
        public string CurrentRepairCostApproved { get; set; }
        [DisplayName("Approved Repair Cost")]
        public string ApprovedRepairCost { get; set; }
        [DisplayName("Need Approval of Repair Cost")]
        public string NeedApprovalofRepairCost { get; set; }
        [DisplayName("Additional Repair Cost")]
        public string AdditionalRepairCost { get; set; }
        [DisplayName("Total Estimated Repair Cost")]
        public string TotalEstimatedRepairCost { get; set; }
        [DisplayName("Device Warranty Void?")]
        public string WarrantyVoid { get; set; }
        [DisplayName("Problem Found")]
        public string[] ProblemFound { get; set; }
        public string probF { get; set; }

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
        [DisplayName("Customer Support Remarks")]
        public string Customer_Support_Remarks { get; set; }
        [DisplayName("E-Mail/SMS Message To Customer")]
        public string EMail_SMS { get; set; }

        // PFR Page Fields
        [DisplayName("Advance Payment Amount")]
        public string AdvancePaymentAmount { get; set; }
        [DisplayName("Transaction Number")]
        public string TransactionNumber { get; set; }
        [DisplayName("Account Holder Name on Cheque")]
        public string AccountHolderNameonCheque { get; set; }
        [DisplayName("Cash Deposited By Name on Slip")]
        public string CashDepositedByNameonSlip { get; set; }
        [DisplayName("Transaction Date")]
        public string TransactionDate { get; set; }
        [DisplayName("Cheque Number")]
        public string ChequeNumber { get; set; }
        [DisplayName("Cash Deposited By Mobile Number on Slip")]
        public string CashDepositedByMobileNumberonSlip { get; set; }
        [DisplayName("Is Advance Payment Received")]
        public string IsAdvancePaymentReceived { get; set; }
        [DisplayName("Received Advance Payment Amount (INR)")]
        public string ReceivedAdvancePaymentAmount { get; set; }
        [DisplayName("Is Customer want to Reject Advance Payment?")]
        public string IsCustomerwanttoRejectAdvancePayment { get; set; }
        [DisplayName("Accounts Remarks")]
        public string AccountsRemarks { get; set; }
        [DisplayName("Is Approval Required for Additional Cost?")]
        public string IsApprovalRequiredforAdditionalCost { get; set; }
        [DisplayName("Is Device Functioning Normally?")]
        public string IsDeviceFunctioningNormally { get; set; }
        [DisplayName("Is Device is looking Equal To New?")]
        public string IsDeviceislookingEqualToNew { get; set; }
        [DisplayName("Final Repair Status")]
        public string FinalRepairStatus { get; set; }
        [DisplayName("QC Person Name")]
        public string QCPersonName { get; set; }
        [DisplayName("Pending JOBs of the QC Person")]
        public string PendingJOBsoftheQCPerson { get; set; }
        [DisplayName("Problem Observed")]
        public string[] PrblmObsrvdPoowrr { get; set; }
        [DisplayName("Box Condition")]
        public string BoxCondition { get; set; }
        public string ReceivingDocument { get; set; }
        [DisplayName("Number of Boxes Received")]
        public string NoOfBoxesReceived { get; set; }
        public string IsReturnable { get; set; }
        [DisplayName("Upload Proof of Advance Payment")]
        public string UploadProofofAdvancePayment { get; set; }
        [DisplayName("Is Repair Cost Approved")]
        public string IsRepairCostApproved { get; set; }
        [DisplayName("E-Mail/SMS Message To Customer")]
        public string Prcc_EMail_SMS { get; set; }
        [DisplayName("Advance Payment Amount")]
        public string Prcc_AdvancePaymentAmount { get; set; }
        [DisplayName("Send Company Bank Account Details")]
        public string Prcc_SendCompanyBankAccountDetails { get; set; }
        [DisplayName("Send Company Bank Account Details")]
        public string Prcc_IsCustomerwantstopayAdvance { get; set; }
        [DisplayName("Customer Support Remarks")]
        public string Prcc_CustomerSupportRemarks { get; set; }

        //PFQC
        [DisplayName("Advance Payment Mode")]
        public string AdvancePaymentMode { get; set; }
        [DisplayName("Is Device Tested by QUTrust?")]
        public string QUTrust { get; set; }
        [DisplayName("QUTrust Certificate Number")]
        public string QUTrustCertificateNumber { get; set; }
        [DisplayName("QUTrust Score")]
        public string QUTrustScore { get; set; }
        [DisplayName("QUTrust Certificate")]
        public string QUTrust_Certificate { get; set; }
        public HttpPostedFileBase QUTrust_Certificate1 { get; set; }
        [DisplayName("Functionality Test")]
        public string FunctionalityTest { get; set; }
        [DisplayName("QC Fail Reason")]
        public string[] QCFailReason { get; set; }
        [DisplayName("Is Device Functioning Normally")]
        public string QC_IsDeviceFunctioningNormally { get; set; }
        [DisplayName("Is Device is looking Equal To New")]
        public string QC_IsDeviceislookingEqualToNew { get; set; }
        [DisplayName("Final QC Status")]
        public string FinalQCStatus { get; set; }
        [DisplayName("QC Remarks")]
        public string QCRemarks { get; set; }
        public string Is_Device_Tested_by_QUTrust { get; set; }
        public string QUTrust_Certificate_Number { get; set; }
        public string QUTrust_Score { get; set; }
        public string Functionality_Test { get; set; }
        public string QC_Fail_Reason { get; set; }
        public string Is_Device_Functioning_Normally { get; set; }
        public string Is_Device_is_looking_Equal_To_New { get; set; }
        public string Final_QC_Status { get; set; }

        //Pending for Packing 
        [DisplayName("Materials to be packed")]
        public string Materials_packed { get; set; }
        [DisplayName("Is Repacking?")]
        public string Is_Repaking { get; set; }
        [DisplayName("Number of Times Re-Packed")]
        public int Number_of_Times_RePacked { get; set; }
        [DisplayName("Job Date")]
        public DateTime JOB_Date { get; set; }
        [DisplayName("Is QUTrust Certificate Printed ?")]
        public string Is_QUTrust_Certificate_Printed { get; set; }
        [DisplayName("Is Functionally Report Printed ?")]
        public string Is_Functionality_Report_Printed { get; set; }
        [DisplayName("Notice Number And Level")]
        public string Notice_Number_and_Level { get; set; }
        [DisplayName("Packaging Material")]
        public string Packaging_Material { get; set; }
        [DisplayName("Length (cm)")]
        public float Length { get; set; }
        [DisplayName("Height (cm)")]
        public float Height { get; set; }
        [DisplayName("Previous AWB Number")]
        public string Previous_AWB_Number { get; set; }
        [DisplayName("Previous Courier Name")]
        public string Previous_Courier_Name { get; set; }
        [DisplayName("Packer Name")]
        public string Packer_Name { get; set; }
        [DisplayName("Packer Id")]
        public string Packer_ID { get; set; }
        [DisplayName("Packaging Material Size")]
        public string Packaging_Material_Size { get; set; }
        [DisplayName("Width (cm)")]
        public float Width { get; set; }

        //Billing/Invoicing Information:
        [DisplayName("Device Shipping Attempt Number")]
        public int Device_Shipping_Attempt_Number { get; set; }
        [DisplayName("Previous Shipping Date")]
        public string Previous_Shipping_Date { get; set; }
        [DisplayName("Bill/Invoice Number")]
        public string Bill_Invoice_Number { get; set; }
        [DisplayName("Service Charge (INR)")]
        public int Service_Charge { get; set; }
        [DisplayName("Advance Amount Paid (INR)")]
        public int Advance_Amount_Paid { get; set; }
        [DisplayName("Due Amount (INR)")]
        public int Due_Amount { get; set; }
        [DisplayName("Bill/Invoice Amount (INR)")]
        public int Bill_Invoice_Amount { get; set; }
        [DisplayName("Material Type")]
        public string Material_Type { get; set; }
        [DisplayName("Notice Shipping Attempt Number")]
        public int Notice_Shipping_Attempt_Number { get; set; }
        [DisplayName("Previous AWB Status")]
        public string Previous_AWB_Status { get; set; }
        [DisplayName("Bill/Invoice Date")]

        public string Bill_Invoice_Date { get; set; }
        [DisplayName("Spare Parts Cost (INR)")]
        public int Spare_Parts_Cost { get; set; }
        [DisplayName("Advance Payment Date")]
        public string Advance_Payment_Date { get; set; }
        [DisplayName("Collectable Amount (INR)")]
        public int Collectable_Amount { get; set; }
        [DisplayName("Schedule Courier Pickup Date")]
        public string Schedule_Courier_Pickup_Date { get; set; }
        [DisplayName("Courier Type")]
        public string Courier_Type { get; set; }
        [DisplayName("Courier Name")]
        public string CourierID { get; set; }
        public string ImagePath { get; set; }
        public float Weight { get; set; }
        [DisplayName("Call Request Date")]
        public string CallRequestDate { get; set; }
        [DisplayName("Is Repair Cost is Less than Approved Cost?")]
        public string IsRepairCost_is_Less_than_Approved_Cost { get; set; }
        public string Prpose { get; set; }
        public string getServiceCharge { get; set; }

        // Testing Correction
        public string OnclickTRC_Name { get; set; }
        [DisplayName("Problem Observed")]
        public string Onclick_ProblemObserved { get; set; }
    }
}