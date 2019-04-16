using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.Clients
{
    public interface IClient: IDisposable
    {
        Task<List<ClientModel>> GetClients();
        Task<ClientModel> GetClientByClientId(Guid? clientId);
        Task<ResponseModel> AddUpdateDeleteClient(ClientModel client);    
        void Save();
    }
}
