using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.State
{
    interface IState:IDisposable
    {
        Task<List<ManageStateModel>> GetAllState(FilterModel filter);
        Task<ManageStateModel> GetStateById(long St_ID);
        Task<ResponseModel> AddUpdateState(ManageStateModel state);
        void Save();
    }
}
