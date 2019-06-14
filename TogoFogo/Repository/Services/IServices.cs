using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
   public interface IServices: IDisposable
    {
        Task<ResponseModel> AddEditServices(ServiceModel serviceModel);
        Task<ResponseModel> AddOrEditServiceableAreaPin(ServiceOfferedModel areaPinModel);
        Task<ServiceModel> GetService(FilterModel filterModel);
        Task<ServiceOfferedModel> GetServiceOfferd(FilterModel filterModel);
        Task<ServiceOfferedModel> GetServiceAreaPin(FilterModel filterModel);
        Task<List<ServiceOfferedModel>> GetServiceAreaPins(FilterModel filterModel);

        void Save();
    }
}
