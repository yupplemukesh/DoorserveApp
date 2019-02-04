using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.Clients
{
    public interface IClients: IDisposable
    {
        Task<List<ClientModel>> GetClients();
        Task<ClientModel> GetClientByClientId(Guid clientId);
        Task<bool> AddUpdateDeleteClient(ClientModel client,char action);
        void Save();
    }
}
