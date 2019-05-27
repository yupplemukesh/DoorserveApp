using System;
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
            _CallAppointmentModel = new CallAppointmentModel();
           
        }
        
        public int StatusId { get; set; }
       
        public SelectList StatusList { get; set; }
        [Required]
        public string RejectionReason { get; set; }
        public Guid EmpId { get; set; }
        public string TechnicianName { get; set; }
        public EmployeeModel Employee { get; set; }
        public UploadedExcelModel _UploadedExcelModel { get; set; }
        public CallAppointmentModel _CallAppointmentModel { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Remark { get; set; }
        public string Param {get;set;}

       
    }
}