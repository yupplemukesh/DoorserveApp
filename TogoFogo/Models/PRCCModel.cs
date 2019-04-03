using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class PRCCModel
    {
        public PRCCModel()
        {
            ReceivedDeviceList = new SelectList(Enumerable.Empty<SelectListItem>());
            Engg_NameList = new SelectList(Enumerable.Empty<SelectListItem>());
            RecvdModelList = new SelectList(Enumerable.Empty<SelectListItem>());
            RecvdBrandlList = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            ProblemFoundList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public string PfelsProblemFound { get; set; }
        [DisplayName("Upload Photo of Damaged Device")]
        public string PhotoOfDevice { get; set; }
        public List<spareTestPFELSForm> TableData { get; set; }
        [DisplayName("Job Date")]
        public string JOBDate { get; set; }
        [DisplayName("Job Number")]
        public string JobNumber { get; set; }
        [DisplayName("Issue to Engineer")]
        public string EnggName { get; set; }
        [DisplayName("Call Request Date")]
        public string CallRequestDate { get; set; }
        [DisplayName("Call Request Number")]
        public string CC_NO { get; set; }
        [DisplayName("Courier Name")]
        public string CourierName { get; set; }
        public string TrcAddress { get; set; }
        public string TrcName { get; set; }
        [DisplayName("Number of Boxes Received")]
        public string NoOfBoxesReceived { get; set; }
        public string CatName { get; set; }
        public string Brand { get; set; }
        [DisplayName("Model Name")]
        public string Model { get; set; }
        [DisplayName("Problem")]
        public string Problem { get; set; }
        [DisplayName("Problem Observed")]
        public string PrblmObsrvd { get; set; }
        [DisplayName("Estimated Cost (INR)")]
        public decimal EstimatedCost { get; set; }
        public string EstimatedCostApproved { get; set; }
        [DisplayName("Physically Damaged?")]
        public bool PhysicalDamage { get; set; }
        public bool DeviceWarrantyVoid { get; set; }
        public string wipedevice { get; set; }
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
        [DisplayName("Received Device")]
        public string RecvdDevice { get; set; }
        [DisplayName("Received Device Brand")]
        public string RecvdBrand { get; set; }
        [DisplayName("Received Device Model")]
        public string RecvdModel { get; set; }
        [DisplayName("Received Device TUPC")]
        public string DeviceTUPC { get; set; }
        [DisplayName("Received Device Serial Number")]
        public string RecvdSerialNo { get; set; }
        [DisplayName("Received Device IMEI 1")]
        public string RecvdIMEI1 { get; set; }
        [DisplayName("Received Device IMEI 2")]
        public string RecvdIMEI2 { get; set; }
        [DisplayName("Is Received Device Physically Damaged?")]
        public bool IsPhyDamage { get; set; }
        [DisplayName("Receive Date")]
        public string ReceiveDate { get; set; }
        [DisplayName("Is Need Receive Approval?")]
        public bool ReceiveApprovalNeeded { get; set; }
        [DisplayName("Receiver Remarks")]
        public string RecvRemarks { get; set; }
        [DisplayName("Receive Approval Status")]
        public string RecvApprovalStatus { get; set; }
        [DisplayName("Receive Approval Date")]
        public string RecvApprovalDate { get; set; }
        [DisplayName("Receive Approved By")]
        public string RecvApprovedBy { get; set; }
        [DisplayName("Warranty Sticker Tempered")]
        public bool WarrantyStickerTempered { get; set; }
        [DisplayName("Device Water Damaged")]
        public bool DeviceWaterDamaged { get; set; }
        public string probF { get; set; }
        [DisplayName("Is OS/Software Reinstall Required? ")]
        public bool OS_Software_Reinstall { get; set; }
        [DisplayName("Is Customer Data Backup Done?")]
        public bool Customer_Data_Backup { get; set; }
        [DisplayName("Current OS/Software Name and Version")]
        public string Current_OS_Software_Name { get; set; }
        [DisplayName("Installed OS/Software Name and Version")]
        public string Installed_OS { get; set; }
        [DisplayName("Total Estimated Spare Cost")]
        public string TotalEstimatedSpareCost { get; set; }
        [DisplayName("Total Estimated Repair Cost")]
        public string TotalEstimatedRepairCost { get; set; }


        public string ProcessName { get; set; }
        [DisplayName("AWB Number")]
        public string AWBNo { get; set; }
        [DisplayName("Receiving Document")]
        public string ReceivingDoc { get; set; }
        [DisplayName("Schedule Pickup Date and Time")]
        public string ScheduleDate { get; set; }
        [DisplayName("Customer Full Information")]
        public string CustoFullInfo { get; set; }
        [DisplayName("TRC City/Location")]
        public string TRCCity { get; set; }
        [DisplayName("Inward Type")]
        public string InwardType { get; set; }
        [DisplayName("Number of Boxes")]
        public string NoOfBoxes { get; set; }
        [DisplayName("Box Condition")]
        public string BoxCondition { get; set; }
        public string ServiceProviderName { get; set; }
        [DisplayName("Purpose?")]
        public string PurPose { get; set; }
        [DisplayName("(TRC Information)")]
        public string TRCInfoFull { get; set; }
        [DisplayName("Is Returnable?")]
        public bool Returnable { get; set; }
        [DisplayName("Upload Photo of Damaged Box")]
        public string DamagedBox { get; set; }
        [DisplayName("Device Type")]
        public string SubCatName { get; set; }
        public string CustomerId { get; set; }
        [DisplayName("Serial Number")]
        public string Serial_No { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        [DisplayName("Problem Reported")]
        public string ProblemReported { get; set; }
        [DisplayName("Problem Observed")]
        public  string ProblemObserved { get; set; }
        [DisplayName("Warrenty Expiry Date")]
        public string WarrentyExpiryDate { get; set; }
        public string WarrantyStatus { get; set; }
        [DisplayName("Is Device Physically Damage Reported?")]
        public string PhysicallyDamageReported { get; set; }
        [DisplayName("Is IW Call Converted to OOW Call?")]
        public string OOWCall { get; set; }
        [DisplayName("Device Warranty Void?")]
        public string WarrantyVoid { get; set; }
        [DisplayName("Is Estimated Cost Approved?")]
        public string CostApproved { get; set; }
        [DisplayName("Customer Support  Remarks")]
        public string CustomerSupport { get; set; }
        [DisplayName("Received Device")]
        public string ReceivedDevice { get; set; }
        [DisplayName("Approver Remarks")]
        public string ApproverRemarks { get; set; }
        [DisplayName("Is Approve for Receiving Device")]
        public bool IsApproveforReceivingDevice { get; set; }
        public string CallStatus { get; set; }
        [DisplayName("Engineer Name")]
        public string Engg_Name { get; set; }
        [DisplayName("Current Status")]
        public string CurrentStatus { get; set; }
        [DisplayName("Problem Found")]
        public string[] ProblemFound { get; set; }
        public  string SpareType { get; set; }
        public  string SpareName { get; set; }
        public  string Quantity { get; set; }
        [DisplayName("Engineer Remarks")]
        public string EngineerRemarks { get; set; }
        [DisplayName("Approved Spare Cost")]
        public string ApprovedSpareCost { get; set; }
        [DisplayName("Need Approval of Spare Cost")]
        public string NeedApprovalofSpareCost { get; set; }
        [DisplayName("Additional Spare Cost")]
        public string AdditionalSpareCost { get; set; }
        [DisplayName("Device Service Charge")]
        public string DeviceServiceCharge { get; set; }
        [DisplayName("Current Repair Cost Approved")]
        public string CurrentRepairCostApproved { get; set; }
        [DisplayName("Approved Repair Cost")]
        public string ApprovedRepairCost { get; set; }
        [DisplayName("Need Approval of Repair Cost")]
        public string NeedApprovalofRepairCost { get; set; }
        [DisplayName("Is Repair Cost Approved?")]
        public string Prcc_Is_Repair_Cost_Approved { get; set; }
        public string CallBackDatetime { get; set; }
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
        public SelectList ReceivedDeviceList { get; set; }
        public SelectList Engg_NameList { get; set; }
        public SelectList RecvdModelList { get; set; }
        public  SelectList RecvdBrandlList { get; set; }
        public SelectList ProblemFoundList { get; set; }
        public  SelectList SpareTypeList { get; set; }
       public  SelectList SpareNameList { get; set; }
    }
}