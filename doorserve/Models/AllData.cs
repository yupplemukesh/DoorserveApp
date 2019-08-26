using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace doorserve.Models
{
    public class QCtableData
    {
        public string QCId { get; set; }
        public string QCProblem { get; set; }
    }
    public class Get_Spare_Type
    {
        public string SpareName { get; set; }
       
    }
    public class PassFailTable
    {
        public string TablePassFailCC_No { get; set; }
        public string TablePassFail_ProblemId { get; set; }
        public string TablePassFail_Pass { get; set; }
        public string TablePassFail_Fail { get; set; }
    }
    public class spareTestPFELSForm1
    {
        //public string spareCcNoField { get; set; }
        public string TablespaceCC_NOField1 { get; set; }
        public string TablespaceJobNumberField1 { get; set; }
        public string TablespareTypeField1 { get; set; }
        public string TablespareCodeField1 { get; set; }
        public string TablesparePartPhoto1 { get; set; }
        public string TablespareNameField1 { get; set; }
        public string TablespareQuantityField1 { get; set; }
        public float TablesparePriceField1 { get; set; }
        public string TablespareTotalField1 { get; set; }
    }
    public class New_Auto_Fill_Table
    {
        public string CC_NO { get; set; }
        public string SpareType { get; set; }
        public string SpareCode { get; set; }
        public string SpareName { get; set; }
        public string SpareQuantity { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
    }
    public class AllData
    {
        
        public List<New_Auto_Fill_Table> New_Auto_Table { get; set; }
        [DisplayName("TUPC")]
        public string TUPCModel { get; set; }
        public string SchedulePickupDateandTime { get; set; }
        [DisplayName("Repair Warranty ")]
        public virtual string WS { get; set; }
        [DisplayName("Customer Pickup Date ")]
        public string customerPickupdate { get; set; }
        public string CallRequestRejectReason { get; set; }



        public List<CourierValuesModel> NewGetTableData { get; set; }
        public string AWBNumber { get; set; }
        public string ModelColor { get; set; }
        public string IP_Address { get; set; }
        public string Process_Name { get; set; }
        public List<GetProblem_Child_Order_problem> ChildtableDataProblem { get; set; }
        public List<Get_Spare_Type> getSpareName { get; set; }
        public List<PassFailTable> Table_Pass_Fail { get; set; }
        public List<QCtableData> QC_Data { get; set; }
        public List<spareTestPFELSForm1> TableData1 { get; set; }
        public string wipedevice { get; set; }

        [DisplayName("Device Type")]
        public string SubCatName { get; set; }
        public string EntryDate { get; set; }
        [DisplayName("AWB Number")]
        public string AWBNo { get; set; }
        public string ProcessName { get; set; }
        [DisplayName("Call Request Number")]
        public virtual string CcNo { get; set; }
        [DisplayName("Call Request Number")]
        public virtual string CC_NO { get; set; }
        [DisplayName("Name")]
        public virtual string Customer_Name { get; set; }
        [DisplayName("Mobile Number")]
        public virtual string Mobile_No { get; set; }
        [DisplayName("E-Mail Address")]
        public virtual string Email_Id { get; set; }
        [DisplayName("Pin Code")]
        public virtual int Pincode { get; set; }
        [DisplayName("State")]
        public virtual string Cust_State { get; set; }
        [DisplayName("City / Location")]
        public virtual string Cust_City { get; set; }
        [DisplayName("Address")]
        public virtual string Cust_Add { get; set; }
        public virtual string Brand { get; set; }
        [DisplayName("Model Name")]
        public virtual string Model { get; set; }
        [DisplayName("Problem")]
        public virtual string Problem { get; set; }

        public virtual string CatName { get; set; }        
        [DisplayName("Call Request Date")]
        public virtual string Pickup_Date { get; set; }

        public virtual string CustomerId { get; set; }
        public string Current_Status { get; set; }

        [DisplayName("Device Type")]
        public virtual string DeviceType { get; set; }
        [DisplayName("Alternate Number")]
        public virtual string AltNo { get; set; }
        [DisplayName("Service Charge")]
        public virtual string BillServiceCharge { get; set; }
        [DisplayName("Spare Cost")]
        public virtual string BillSpareCost { get; set; }
        [DisplayName("Estimated Cost")]
        public virtual string BillEstimatedCost { get; set; }
        [DisplayName("Estimated Cost Approved")]
        public virtual bool IsEstimatedCostApproved { get; set; }
        [DisplayName("Repair Status")]
        public virtual string RepairStatus1 { get; set; }
        [DisplayName("Collectable Amount")]
        public virtual string CollectableAmount { get; set; }
        [DisplayName("Payment Mode")]
        public virtual string PaymentMode1 { get; set; }
        //Submit Data Model
        [DisplayName("Message To Customer")]
        public virtual string MsgToCust { get; set; }
        public virtual string SERemarks { get; set; }
        [DisplayName("Serial Number")]
        public virtual string Serial_No { get; set; }
        public virtual string IMEI1 { get; set; }
        public virtual string IMEI2 { get; set; }
        [DisplayName("Service Engineer Action")]
        public virtual string SE_Action { get; set; }
        [DisplayName("Engineer Visit Date and Time")]
        public virtual string VisitDatetime { get; set; }
        [DisplayName("Engineer Name")]
        public virtual string Engg_Name { get; set; }
        [DisplayName("Reverse Pickup Date and Time")]
        public virtual string Pickupdatetime { get; set; }
        [DisplayName("Courier Name")]
        public virtual  string CourierName { get; set; }
        [DisplayName("Physically Damaged?")]
        public virtual bool PhysicalDamage { get; set; }

        [DisplayName("Problem Observed")]
        public virtual string PrblmObsrvd { get; set; }

        [DisplayName("Service Charge (INR)")]
        public virtual decimal ServiceCharge { get; set; }
        [DisplayName("Spare Cost (INR)")]
        public virtual decimal SpareCost { get; set; }
        [DisplayName("Estimated Cost (INR)")]
        public virtual decimal EstimatedCost { get; set; }
        [DisplayName("Is Estimated Cost Approved?")]
        public virtual string IsApproved { get; set; }
        [DisplayName("Repair Status")]
        public virtual string RepairStatus { get; set; }
        [DisplayName("Collectable Amount (INR)")]
        public virtual string CllectableAmt { get; set; }
        [DisplayName("Payment Mode")]
        public virtual string PaymentMode { get; set; }
        [DisplayName("Cash Received (INR)")]
        public virtual string CashRecvd { get; set; }
        [DisplayName("Balance Amount")]
        public virtual string BalanceAmt { get; set; }
        [DisplayName("Transaction Amount (INR)")]
        public virtual string TransAmt { get; set; }
        [DisplayName("Transaction Date and Time")]
        public virtual string TransDateTime { get; set; }
        [DisplayName("Transaction Number")]
        public virtual string TransNumber { get; set; }
        [DisplayName("Re-Visit Date and Time")]
        public virtual string RevisitDatetime { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual bool CourierActive { get; set; }
        [DisplayName("Bike Make")]
        public virtual string BikeMake { get; set; }
        [DisplayName("Message To Customer")]
        public virtual string MessageCusto { get; set; }
        public virtual string CourierLogo { get; set; }
        public virtual string CourierContact { get; set; }
        public virtual string BikeNumber { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string EngineerVisitDate { get; set; }
        public virtual string UploadedCourierFile { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual bool DeviceWarranty { get; set; }



        public virtual string TRCFullAddr { get; set; }
        public virtual string ReversePickupDate { get; set; }
        public string TrcProcess_Name { get; set; }
        public string TrcName { get; set; }
        public string TrcMobile { get; set; }
        public string TrcEmail { get; set; }
        public string TrcAddress { get; set; }
        public string TrcPinCode { get; set; }
        public string TrcLocality { get; set; }
        public string TrcNear_By_Location { get; set; }
        public virtual string WarrantyStatus { get; set; }
        public virtual string WarrantyExpiryDate { get; set; }
        public  string EstimatedCostApproved { get; set; }
        [DisplayName("TRC City/Location")]
        public string TRCCity { get; set; }

        // SubmitPart
        [DisplayName("Is Battery Removable?")]
        public bool BtryRemovable { get; set; }
        [DisplayName("Is Battery Required?")]
        public bool BtryReq { get; set; }
        [DisplayName("Is Battery Received?")]
        public bool BtryReceived { get; set; }
        [DisplayName("Battery Brand and Model")]
        public string BtryBrandAndModel { get; set; }
        [DisplayName("Is Different Device Received?")]
        public bool IsDiffDeviceRecvd { get; set; }
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
        public bool IsPhyDamage { get; set; }
        [DisplayName("Upload Photo of Damaged Device")]
        public string PhotoOfDevice { get; set; }
        [DisplayName("Receive Date")]
        public string ReceiveDate { get; set; }
        [DisplayName("Is Need Receive Approval?")]
        public bool ReceiveApprovalNeeded { get; set; }
        [DisplayName("Receiver Remarks")]
        public string RecvRemarks { get; set; }
        [DisplayName("Is Approve for Receiving Device")]
        public bool IsApproveforReceivingDevice { get; set; }
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
        public virtual string CallStatus { get; set; }
        [DisplayName("Warranty Sticker Tempered")]
        public bool WarrantyStickerTempered { get; set; }
        [DisplayName("Current Status")]
        public virtual string CurrentStatus { get; set; }
        public virtual string CallBackDatetime { get; set; }
        [DisplayName("Job Number")]
        public string JobNumber { get; set; }
        [DisplayName("Device Water Damaged")]
        public bool DeviceWaterDamaged { get; set; }
        [DisplayName("Spare Type")]
        public virtual string SpareType { get; set; }
        [DisplayName("Spare Name")]
        public virtual string SpareName { get; set; }
        public virtual string Quantity { get; set; }
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
        public virtual string WarrantyVoid { get; set; }
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
        public virtual string ServiceProviderName { get; set; }
        [DisplayName("Call Request Date and Time")]
        public string CallRequestDateTime { get; set; }
        [DisplayName("Call Status")]
        public string CallStatusRepairInfo { get; set; }
        [DisplayName("Current Status")]
        public string CurrentStatusRepairInfo { get; set; }
        public string PfelsEngineerName { get; set; }
        public string PfelsProblemFound { get; set; }
        [DisplayName("Is OS/Software Reinstall Required? ")]
        public bool OS_Software_Reinstall { get; set; }
        [DisplayName("Is Customer Data Backup Done?")]
        public bool Customer_Data_Backup { get; set; }
        [DisplayName("Current OS/Software Name and Version")]
        public string Current_OS_Software_Name { get; set; }
        [DisplayName("Installed OS/Software Name and Version")]
        public string Installed_OS { get; set; }
        public bool DeviceWarrantyVoid { get; set; }

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
        public virtual string ChequeNumber { get; set; }
        [DisplayName("Cash Deposited By Mobile Number on Slip")]
        public string CashDepositedByMobileNumberonSlip { get; set; }
        [DisplayName("Is Advance Payment Received")]
        public string IsAdvancePaymentReceived { get; set; }
        [DisplayName("Received Advance Payment Amount (INR)")]
        public string ReceivedAdvancePaymentAmount { get; set; }
        [DisplayName("Is Customer want to Reject Advance Payment?")]
        public string IsCustomerwanttoRejectAdvancePayment { get; set; }
        [DisplayName("Accounts Remarks")]
        public virtual string AccountsRemarks { get; set; }
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
        public virtual string PrblmObsrvdPoowrr { get; set; }
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
        public bool Prcc_IsCustomerwantstopayAdvance { get; set; }
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
        public int CourierID { get; set; }
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


        //POWRR SelectList
        public virtual  string SelectTrc { get; set; }
        public virtual SelectList SpareTypeList { get; set; }
        public virtual SelectList SpareNameList { get; set; }
        public virtual SelectList SelectTrcList { get; set; }
        public virtual SelectList CourierNameList { get; set; }
        public virtual SelectList CallStatusList { get; set; }
        public virtual SelectList ServiceProviderNameList { get; set; }
        public virtual SelectList ProblemList { get; set; }
        public virtual SelectList WSList { get; set; }

       
        //PIWRRSelectList
        public  virtual string ProblemObserved { get; set; }
        public  SelectList ProblemObservedList { get; set; }
        //PRCC()
        public virtual  string ReceivedDevice { get; set; }
        public SelectList ReceivedDeviceList { get; set; }
        public SelectList Engg_NameList { get; set; }
        public SelectList RecvdModelList { get; set; }
        public virtual SelectList RecvdBrandlList { get; set; }
        public SelectList ProblemFoundList { get; set; }

        //FindPOOWRR

        public virtual string StatusName { get; set; }

    }

}