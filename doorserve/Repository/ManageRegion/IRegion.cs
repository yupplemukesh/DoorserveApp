using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.ManageRegion
{
    interface IRegion:IDisposable
    {
        Task<List<ManageRegionModel>> GetAllRegion(FilterModel filter);
        Task<ManageRegionModel> GetRegionById(Guid RegionId);
        Task<ResponseModel> AddUpdateRegion(ManageRegionModel Region);
        void Save();
    }
}
