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
        Task<ResponseModel> AddUpdateDeleteClient(ClientModel client);
        Task<ResponseModel> AddUpdateBankDetails(BankDetailModel bank);
        Task<ResponseModel> AddUpdateContactDetails(ContactPersonModel contact);
        void Save();
    }
}
