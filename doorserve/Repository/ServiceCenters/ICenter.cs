using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Customer_Support;
using doorserve.Models.ServiceCenter;

namespace doorserve.Repository.ServiceCenters
{
    public interface ICenter : IDisposable
    {
        Task<ServiceCenterCallsModel> GetCallDetails(FilterModel filterModel);
        Task<List<ServiceCenterModel>> GetCenters(FilterModel filterModel);
        Task<CallDetailsModel> GetCallsDetailsById(string CRN);
        Task<ServiceCenterModel> GetCenterById(Guid? serviceCenterId);
        Task<ResponseModel> AddUpdateDeleteCenter(ServiceCenterModel center);
        Task<ResponseModel> UpdateCallsStatus(CallStatusModel callStatus);
        Task<EmployeeModel> GetTechnicianDetails(string EmpId);
        Task<ResponseModel> AssignCallsDetails(EmployeeModel assignCalls);
        Task<ResponseModel> UpdateCallsStatusDetails(CallStatusDetailsModel callStatusDetails);
        Task<ResponseModel> UpdateCallCenterCall(CallStatusDetailsModel callStatusDetails);
        Task<ResponseModel> EditCallAppointment(CallDetailsModel CallAppointment);
        void Save();
    }
}
