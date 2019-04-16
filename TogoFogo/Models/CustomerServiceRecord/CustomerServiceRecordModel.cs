using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models.CustomerServiceRecord
{
    public class CustomerServiceRecordModel
    {
        public CustomerServiceRecordModel()
        {
            _Contact = new ContactPersonModel();
            _Address = new AddressDetail();
            _Product = new ProductModel();
            _ServiceType = new ServiceType();
            _CallType = new CallType();
            _SetCondition = new SetCondition();
            _RecievedItems = new RecievedItems();
        }
        public ProductModel _Product { get; set; }
        public AddressDetail _Address { get; set; }
        public ContactPersonModel _Contact { get; set; }
        public ServiceType _ServiceType { get; set; }
        public CallType _CallType { get; set; }
        public SetCondition _SetCondition { get; set; }
        public RecievedItems _RecievedItems { get; set; }

        public class ServiceType
        {
            public bool CI { get; set; }
            public bool IH { get; set; }
            public bool INS { get; set; }
            public bool UINS { get; set; }
            public bool PMS { get; set; }
            public bool REP { get; set; }
        }
        public class CallType
        {
            public bool IW { get; set; }
            public bool OW { get; set; }
            public bool AMC { get; set; }
            public bool INSU { get; set; }
            public bool REF { get; set; }
            public bool VIP { get; set; }
        }

        public string Symptom1 { get; set; }
        public string Symptom2 { get; set; }
        public string Symptom3 { get; set; }
        public string JobNo { get; set; }
        public DateTime JobDate { get; set; }
        public DateTime JobTime { get; set; }
        public string AMCNo { get; set; }
        public string INSUNo { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string ModelNo { get; set; }
        public string CompNo { get; set; }
        public string PanelNo { get; set; }
        public string IMEINo { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DLRName { get; set; }
        public string Location { get; set; }
        public DateTime AMCFrom { get; set; }
        public DateTime AMCTo { get; set; }

        public class SetCondition
            {
            public bool Tempered { get; set; }
            public bool PhysicalDamage { get; set; }
            public bool WaterLog { get; set; }
            public bool Scratches { get; set; }
            public string Missing { get; set; }
             }
        public class RecievedItems
        {
          public bool Remote { get; set; }
            public bool Adapter { get; set; }
            public bool Stand { get; set; }
            public bool Charger { get; set; }
            public bool Battery { get; set; }
            public string Others { get; set; }
            public string Condition { get; set; }
            public string Defect1 { get; set; }
            public string Defect2 { get; set; }
            public string Symptom { get; set; }
            public string Repair { get; set; }
        }

        public string DefectDetectByEng { get; set; }
        public string RepairReport1 { get; set; }
        public string RepairReport2 { get; set; }
        public string EstimateRepairs { get; set; }
        public string EngineerSign { get; set; }
        public string CustomerSign { get; set; }
        public string CustomerApproval { get; set; }
        public string Discount { get; set; }
        public string DiscountApproval { get; set; }
        public bool Cash { get; set; }
        public string Cheque { get; set; }
        public string EngineerName { get; set; }
        public string ManagedBy { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public char Action { get; set; }
    }
}