using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;
namespace TogoFogo.Repository
{
   public interface IUploadFiles: IDisposable
    {
        Task<ResponseModel> UploadClientData(ClientDataModel client, DataTable table);
        Task<MainClientDataModel> GetUploadedList(Guid? ClientId);
        Task<CallsViewModel> GetAssingedCalls();
        
        void Save();
    }
}
