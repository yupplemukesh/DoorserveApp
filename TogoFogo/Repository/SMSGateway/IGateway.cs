using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;

namespace TogoFogo.Repository.SMSGateway
{
      public interface IGateway:IDisposable
    {
        Task<List<GatewayModel>> GetGatewayByType(FilterModel filter);
        Task<GatewayModel> GetGatewayById(int GatewayId);
        Task<ResponseModel> AddUpdateDeleteGateway(GatewayModel gatewayModel, char action);
        void Save();
        
    }
}
