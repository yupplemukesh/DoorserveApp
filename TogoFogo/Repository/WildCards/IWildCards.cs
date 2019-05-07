using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;

namespace TogoFogo.Repository.WildCards
{
      public interface IWildCards:IDisposable
    {
        Task<List<WildCardModel>> GetWildCards(FilterModel filterModel);
        Task<WildCardModel> GetWildCardByWildCardId(int WildCardId);
        Task<ResponseModel> AddUpdateDeleteWildCards(WildCardModel wildCardModel, char action);
        void Save();
    }
}
