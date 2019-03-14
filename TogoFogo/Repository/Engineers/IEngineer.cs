using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public interface IEngineer: IDisposable
    {
        Task<List<ManageEngineerModel>> GetAllEngineers(Guid ? serviceCenterId, Guid? providerId);
        Task<ManageEngineerModel> GetEngineerById(int engineerId);
        Task<ResponseModel> AddUpdateDeleteEngineer(ManageEngineerModel engineer);    
        void Save();
    }
}
