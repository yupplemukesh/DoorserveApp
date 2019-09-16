using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository
{
   public interface IServices: IDisposable
    {
        Task<ResponseModel> AddEditServices(ServiceModel serviceModel);
        Task<ResponseModel> AddOrEditServiceableAreaPin(ServiceOfferedModel areaPinModel);
        Task<ServiceViewModel> GetService(FilterModel filterModel);
        Task<ServiceOfferedModel> GetServiceOfferd(FilterModel filterModel);
        Task<ServiceOfferedModel> GetServiceAreaPin(FilterModel filterModel);
        Task<List<ServiceOfferedModel>> GetServiceAreaPins(FilterModel filterModel);


        void Save();
    }
}
