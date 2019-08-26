using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Gateway;

namespace doorserve.Repository.WildCards
{
      public interface IWildCards:IDisposable
    {
        Task<List<WildCardModel>> GetWildCards(FilterModel filterModel);
        Task<WildCardModel> GetWildCardByWildCardId(int WildCardId);
        Task<ResponseModel> AddUpdateDeleteWildCards(WildCardModel wildCardModel, char action);
        void Save();
    }
}
