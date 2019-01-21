﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    
    public class RmFormModel
    {
        [DisplayName("Call Request Number")]
        public string CC_NO { get; set; }
    }
    public class spareTestPFELSForm
    {
        //public string spareCcNoField { get; set; }
        public string TablespaceCC_NOField { get; set; }
        public string TablespaceJobNumberField { get; set; }
        public string TablespareTypeField { get; set; }
        public string TablespareCodeField { get; set; }
        public string TablesparePartPhoto { get; set; }
        public string TablespareNameField { get; set; }
        public string TablespareQuantityField { get; set; }
        public string TablesparePriceField { get; set; }
        public string TablespareTotalField { get; set; }
    }
    public class ReceiveMaterials:AllData
    {
      
        public string ProblemDropdown { get; set; }
        public string DEVWarranty { get; set; }

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
        //[DisplayName("Is customer agree to wipe device data?")]
        //public string wipedevice { get; set; }
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
        //public string CallRequestDate { get; set; }
       
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

        //
       
    }
}