using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class RPCAPModel:ReceiveMaterials
    {
        public RPCAPModel()
        {
            ReceivedDeviceList = new SelectList(Enumerable.Empty<SelectListItem>());
            RecvdBrandList = new SelectList(Enumerable.Empty<SelectListItem>());
            RecvdModelList = new SelectList(Enumerable.Empty<SelectListItem>());
            Engg_NameList = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareTypeList = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareNameList = new SelectList(Enumerable.Empty<SelectListItem>());
            ProblemFoundList = new SelectList(Enumerable.Empty<SelectListItem>());
        }

        public string EMail_SMSMessage { get; set; }
        public string AccHolder { get; set; }
        public string CashDepositedByName { get; set; }
        public string CashDepositedByMobile{ get; set; }
        public string ProofAdvancePay { get; set; }
        public string AdvancePaymentReceived { get; set; }
        public string ReceivedAdvancePayment { get; set; }        
        public string CustomerRejectAdvancePayment { get; set; }
        public string CustomerwantstopayAdvance { get; set; }
        public string CustomerSupportRemarks { get; set; }
        public List<RPCAPModel> _RpcapModelList { get; set; }
        public UserActionRights _UserActionRights { get; set; }

        public SelectList RecvdBrandList { get; set; }
    }
}