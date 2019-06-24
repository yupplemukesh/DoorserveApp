using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Models
{
    public class BankDetailModel: RegistrationModel
    {
        public BankDetailModel()
            {
            BankList = new SelectList(Enumerable.Empty<SelectListItem>());

            }      
        public Guid? RefKey { get; set; }
        public Guid? bankId { get; set; }
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [Required]
        [DisplayName("Bank Name")]
        public int BankNameId { get; set; }
        [Required]
        [DisplayName("Bank Account Number")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter number only")]
        public string BankAccountNumber { get; set; }
        [Required]
        [DisplayName("Company Name at Bank Account")]
        public string BankCompanyName { get; set; }
        [Required]
        [RegularExpression(@"[A-Z|a-z]{4}[0][a-zA-Z0-9]{6}$", ErrorMessage = "Invalid IFSC Code Number")]
        [DisplayName("IFSC Code")]
        public string BankIFSCCode { get; set; }
        [DisplayName("Bank Branch")]
        public string BankBranchName { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public HttpPostedFileBase BankCancelledChequeFilePath { get; set; }
        public bool IsDefault { get; set; }
        [DisplayName("Upload Cancelled Cheque")]
        public string BankCancelledChequeFileName { get; set; }
        public string BankCancelledChequeFileUrl { get; set; }
        public SelectList BankList { get; set; }
        
    }
    public class RegistrationModel
    {
       public  bool IsActive { get; set; }
       public string Remarks { get; set; } 
       public bool IsUser { get; set; }
       public DateTime LastUpdateDate { get; set; }
       public string LastUpdatedBy { get; set; }
       public int UserId { get; set; }
        public bool CurrentIsUser { get; set; }
       public Guid? CompanyId { get; set; }
       public string Password { get; set; }
       public char EventAction { get; set; }

    }


    
}