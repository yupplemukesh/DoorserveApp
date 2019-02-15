using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class BankDetailModel
    {
        public BankDetailModel()
            {
            BankList = new SelectList(Enumerable.Empty<SelectListItem>());

            }      
        public Guid RefKey { get; set; }
        public Guid? bankId { get; set; }
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [Required]
        [DisplayName("Bank Name")]
        public int BankNameId { get; set; }
        [Required]
        [DisplayName("Bank Account Number")]
        public string BankAccountNumber { get; set; }
        [Required]
        [DisplayName("Company Name at Bank Account")]
        public string BankCompanyName { get; set; }
        [Required]
        [DisplayName("IFSC Code")]
        public string BankIFSCCode { get; set; }
        [DisplayName("Bank Branch")]
        public string BankBranchName { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public HttpPostedFileBase BankCancelledChequeFilePath { get; set; }
        public int UserId { get; set; }
        public char Action { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public string BankCancelledChequeFileName { get; set; }
        public SelectList BankList { get; set; }
    }
}