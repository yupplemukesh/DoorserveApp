using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;

namespace TogoFogo.Repository.WildCards
{
      public interface IWildCards:IDisposable
    {
        Task<List<WildCardModel>> GetWildCards();
        Task<WildCardModel> GetActionByWildCardId(int WildCardId);
        Task<bool> AddUpdateDeleteWildCards(WildCardModel wildCardModel, char action);
        void Save();
    }
}
