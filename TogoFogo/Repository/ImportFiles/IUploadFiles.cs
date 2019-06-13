using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;
namespace TogoFogo.Repository
{
   public interface IUploadFiles: IDisposable
    {
        Task<ResponseModel> UploadClientData(ClientDataModel client, DataTable table);
        MainClientDataModel GetUploadedList(FilterModel filterModel);
        Task<CallsViewModel> GetAssingedCalls(FilterModel filterModel);
        Task<MainClientDataModel> GetExportAssingedCalls(FilterModel filterModel);
        Task<ResponseModel> UploadServiceProviders(ProviderFileModel provider, DataTable table);
        Task<ResponseModel> UploadCityLocations(ProviderFileModel provider, DataTable table);
        void Save();
    }
}
