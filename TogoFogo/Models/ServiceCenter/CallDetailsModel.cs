using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using TogoFogo.Models.ClientData;
using TogoFogo.Models.Customer_Support;

namespace TogoFogo.Models.ServiceCenter
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
        [Required]
        public string RejectionReason { get; set; }
        public Guid? EmpId { get; set; }
        public string TechnicianName { get; set; }
        public EmployeeModel Employee { get; set; }
        public List<PartsDetailsModel>   Parts { get; set; }
        public List<ProviderFileModel> Files { get; set; }
        public UploadedExcelModel _UploadedExcelModel { get; set; }
        public string ProblemDescription { get; set; }
        public string IssueOcurringSinceDate { get; set; }        
        public DateTime ? AppointmentDate { get; set; }
        // public string Remarks { get; set; }
        public Guid? PreviousCallId { get; set; }
        public string ProblemObserved { get; set; }
        public  int ? CStatus { get; set; }
        public int? ASPStatus { get; set; }
        public bool IsServiceApproved { get; set; }
        public string Param {get;set;}
        public string Symptom1 { get; set; }
        public string Symptom2 { get; set; }
        public string Symptom3 { get; set; }
        public string InvoiceFileName { get; set; }
        public string JobSheetFileName { get; set; }

    }
}