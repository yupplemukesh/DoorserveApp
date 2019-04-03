using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
namespace TogoFogo.Repository.Menues
{
   public interface IMenues: IDisposable
    {
        Task<List<MenuMasterModel>> GetMenues();
        Task<MenuMasterModel> GetMenuById(string menuID);
        Task<ResponseModel> AddUpdateMenu(MenuMasterModel menu,char action);
        void Save();
    }
}
