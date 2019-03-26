using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class PWIRRModel : AllData
    {
        public List<spareTest> test { get; set; }
        [DisplayName("Is customer agree to wipe device data?")]
        public string wipedevicedata { get; set; }
        [DisplayName("Select TRC")]
        public override  string SelectTrc { get; set; }
        public string Pending { get; set; }
        public string Reject { get; set; }
        [DisplayName("Engineer Visit Date and Time")]
        public string EngineerVisit { get; set; }
        public string SchedulePickup { get; set; }
        [DisplayName("Warranty Status")]
        public override string WarrantyStatus { get; set; }
        [DisplayName("Warranty Expiry Date")]
        public override string WarrantyExpiryDate { get; set; }
        [DisplayName("Call Request Reject Reason")]
        public string CallRequestReject { get; set; }
        [DisplayName("Problem Observed")]
        [Required]
        public new  string[] ProblemObserved { get; set; }
    }
    public class GetTrcAddressInfo
    {
        public string Address { get; set; }
        public string LOCALITY { get; set; }
        public string NEAR_BY_LOCATION { get; set; }
        public string PIN_CODE { get; set; }

    }
    public class GetCourierInfoModel
    {
        public string MobileNumber { get; set; }
        public string UploadedCourierFile { get; set; }
        public string IsActive { get; set; }
        public string BikeNumber { get; set; }
        public string BikeMakeandModel { get; set; }
    }
    public class spareTest
    {
        public string spareCcNoField { get; set; }
        public string spaceJobNumberField { get; set; }
        public string spareTypeField { get; set; }
        public string spareNameField { get; set; }
        public string sparePriceField { get; set; }
        public string spareQuantityField { get; set; }
    }
}