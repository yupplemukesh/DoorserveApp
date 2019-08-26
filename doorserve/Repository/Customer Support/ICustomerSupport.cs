using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Customer_Support;

namespace doorserve.Repository.Customer_Support
{
   public interface ICustomerSupport:IDisposable
    {
        Task<CallToASPModel> GetASPCalls(FilterModel filter);
        Task<CallToASCModel> GetASCCalls(FilterModel filter);
        Task<ResponseModel> AllocateCall(AllocateCallModel allocateCalls);
      
        void Save();
    }
}
