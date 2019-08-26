using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Gateway;

namespace doorserve.Repository.SMSGateway
{
      public interface IGateway:IDisposable
    {
        Task<List<GatewayModel>> GetGatewayByType(FilterModel filter);
        Task<GatewayModel> GetGatewayById(int GatewayId);
        Task<ResponseModel> AddUpdateDeleteGateway(GatewayModel gatewayModel, char action);
        void Save();
        
    }
}
