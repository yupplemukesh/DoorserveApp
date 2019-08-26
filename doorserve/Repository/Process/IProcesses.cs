using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.Process
{
    interface IProcesses:IDisposable
    {
        Task<List<ProcessModel>> GetAllProcesses(FilterModel filter);
        Task<ProcessModel> GetProcessesById(int ProcessId);
        Task<ResponseModel> AddUpdateProcess(ProcessModel Process);
        void Save();
    }
}
