using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TogoFogo.Models
{
    public class BankDetailModel
    {
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [DisplayName("Company Name at Bank Account")]
        public string BankCompanyName { get; set; }
        [DisplayName("IFSC Code")]
        public string BankIFSCCode { get; set; }
        [DisplayName("Bank Branch")]
        public string BankBranchName { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public HttpPostedFileBase BankCancelledChequeFilePath { get; set; }

        [DisplayName("Upload Cancelled Cheque")]
        public string BankCancelledChequeFileName { get; set; }
    }
}