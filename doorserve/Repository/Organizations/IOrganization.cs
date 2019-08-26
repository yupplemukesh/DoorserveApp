using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Models;

namespace doorserve.Repository
{
    public interface IOrganization:IDisposable
    {
        Task<ResponseModel> AddUpdateOrgnization(OrganizationModel organization);
        Task<OrganizationModel> GetOrganizationByRefKey(Guid? refkey);
        void Save();
    }
}
