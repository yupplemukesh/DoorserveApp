using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public interface IActionTypes:IDisposable
    {
        Task<List<ActionTypeModel>> GetActiontypes();
        Task<ActionTypeModel> GetActionByActionId(int ActionTypeId);
        Task<ResponseModel> AddUpdateDeleteActionTypes(ActionTypeModel actionTypeModel, char action);
        void Save();

    }
}
