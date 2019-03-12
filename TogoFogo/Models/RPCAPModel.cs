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
            ReceivedDeviceList1 = new SelectList(Enumerable.Empty<SelectListItem>());
            RecvdBrandList = new SelectList(Enumerable.Empty<SelectListItem>());
            RecvdModelList1 = new SelectList(Enumerable.Empty<SelectListItem>());
            Engg_NameList1 = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareTypeList1 = new SelectList(Enumerable.Empty<SelectListItem>());
            SpareNameList1 = new SelectList(Enumerable.Empty<SelectListItem>());
            ProblemFoundList1 = new SelectList(Enumerable.Empty<SelectListItem>());
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
        public SelectList ReceivedDeviceList1 { get; set; }
        public SelectList RecvdBrandList { get; set; }
        public SelectList RecvdModelList1 { get; set; }
        public SelectList Engg_NameList1 { get; set; }
        public SelectList SpareTypeList1 { get; set; }
        public SelectList SpareNameList1 { get; set; }
        public SelectList ProblemFoundList1 { get; set; }

    }
}