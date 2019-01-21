using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class RPCAPModel:ReceiveMaterials
    {
        public string EMail_SMSMessage { get; set; }
        public string AccHolder { get; set; }
       
        public string CashDepositedByName { get; set; }
        public string ChequeNumber { get; set; }
        public string CashDepositedByMobile{ get; set; }
        public string ProofAdvancePay { get; set; }
        public string AdvancePaymentReceived { get; set; }
        public string ReceivedAdvancePayment { get; set; }        
        public string CustomerRejectAdvancePayment { get; set; }
        public string AccountsRemarks { get; set; }
        public string CustomerwantstopayAdvance { get; set; }
        public string CustomerSupportRemarks { get; set; }
    }
}