using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.ServiceCenters
{
    public interface ICenter : IDisposable
    {
        Task<List<ServiceCenterModel>> GetCenters();
        Task<ServiceCenterModel> GetCenterById(Guid serviceCenterId);
        Task<ResponseModel> AddUpdateDeleteCenter(ServiceCenterModel center);    
        void Save();
    }
}
