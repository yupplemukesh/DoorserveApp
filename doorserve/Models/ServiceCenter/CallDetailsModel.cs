using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using doorserve.Models.ClientData;
using doorserve.Models.Customer_Support;

namespace doorserve.Models.ServiceCenter
{
    public class CallDetailsModel: CallAllocatedToASCModel
    {
        public CallDetailsModel()
        {
            StatusList = new SelectList(Enumerable.Empty<SelectListItem>());
            _UploadedExcelModel = new UploadedExcelModel();
           
        }
        public bool IsAssingedCall { get; set; }
        public int? StatusId { get; set; }      

        public string RejectionReason { get; set; }
        public string CancelReason { get; set; }
        public string CompLogo { get; set; }
        public Guid? EmpId { get; set; }
        public string TechnicianName { get; set; }
       [MinLength(10)]
       [MaxLength(10)]
       [DisplayName("Technician Contact Number")]
        public string TechnicianContactNumber { get; set; }
        public EmployeeModel Employee { get; set; }
        public List<PartsDetailsModel>   Parts { get; set; }
        public List<ProviderFileModel> Files { get; set; }
        public UploadedExcelModel _UploadedExcelModel { get; set; }
        public string ProblemDescription { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string IssueOcurringSinceDate { get; set; }        
        public DateTime ? AppointmentDate { get; set; }
        // public string Remarks { get; set; }
        public Guid? PreviousCallId { get; set; }
        public string ProblemObserved { get; set; }
        public  int ? CStatus { get; set; }
        public int? ASPStatus { get; set; }
        public decimal? MinimumApprovalCost { get; set; }
        public bool IsServiceApproved { get; set; }
        public string Param {get;set;}
        public string Symptom1 { get; set; }
        public string Symptom2 { get; set; }
        public string Symptom3 { get; set; }
        public string InvoiceFileName { get; set; }
        public string JobSheetFileName { get; set; }

        public string tab_index { get; set;  }

    }
}