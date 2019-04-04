using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Customer_Support;
using TogoFogo.Models.ServiceCenter;

namespace TogoFogo.Repository.ServiceCenters
{
    public interface ICenter : IDisposable
    {
        Task<ServiceCenterCallsModel> GetCallDetails();
        Task<List<ServiceCenterModel>> GetCenters(Guid? providerId);
        Task<ServiceCenterModel> GetCenterById(Guid serviceCenterId);
        Task<ResponseModel> AddUpdateDeleteCenter(ServiceCenterModel center);
        Task<ResponseModel> UpdateCallsStatus(CallStatusModel callStatus);
        void Save();
    }
}
