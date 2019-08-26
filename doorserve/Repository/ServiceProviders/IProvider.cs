using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.ServiceProviders
{
    public interface IProvider : IDisposable
    {
        Task<List<ServiceProviderModel>> GetProviders(FilterModel filterModel);
        Task<ServiceProviderModel> GetProviderById(Guid? serviceProviderId);
        Task<ResponseModel> AddUpdateDeleteProvider(ServiceProviderModel provider);
        Task<List<serviceProviderData>> GetProvidersExcel(FilterModel filterModel);
        void Save();
    }
}
