using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.ManageBanners
{
    interface IBanner:IDisposable
    {
        Task<List<ManageBannersModel>> GetBanner(FilterModel filterModel);
        Task<ManageBannersModel> GetBannerById(Guid BannerId);
        Task<ResponseModel> AddUpdateBanner(ManageBannersModel Banner);
        void Save();
    }
}
