using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.ClientData;
namespace doorserve.Repository
{
   public interface IUploadFiles: IDisposable
    {
        Task<ResponseModel> UploadClientData(FileDetailModel client, DataTable table);
        MainClientDataModel GetUploadedList(FilterModel filterModel);
        
        Task<CallsViewModel> GetAssingedCalls(FilterModel filterModel);
        Task<MainClientDataModel> GetExportAssingedCalls(FilterModel filterModel);
        Task<ResponseModel> UploadServiceProviders(ProviderFileModel provider, DataTable table);
        Task<ResponseModel> UploadCityLocations(ProviderFileModel provider, DataTable table);
        Task<ResponseModel> UploadServiceableAreaPins(ProviderFileModel provider, DataTable table);
        Task<List<ProviderFileModel>> GetFiles(Guid? RefKey);
        void Save();
    }
}
