using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Customer_Support;

namespace TogoFogo.Repository.Customer_Support
{
   public interface ICustomerSupport:IDisposable
    {
        Task<CallToASPModel> GetCalls();
        Task<ResponseModel> AllocateCallASP(AllocateCallModel allocateCalls);
        Task<ResponseModel> AllocateCallASC(AllocateCallModel allocateCalls);
        void Save();
    }
}
