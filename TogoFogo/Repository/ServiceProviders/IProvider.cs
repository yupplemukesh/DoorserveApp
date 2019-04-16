using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.ServiceProviders
{
    public interface IProvider : IDisposable
    {
        Task<List<ServiceProviderModel>> GetProviders();
        Task<ServiceProviderModel> GetProviderById(Guid? serviceProviderId);
        Task<ResponseModel> AddUpdateDeleteProvider(ServiceProviderModel provider);    
        void Save();
    }
}
