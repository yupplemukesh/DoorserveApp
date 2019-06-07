using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;
namespace TogoFogo.Repository.Country
{
    interface ICountry:IDisposable
    {
        Task<List<ManageCountryModel>> GetAllCountry(FilterModel filter);
        Task<ManageCountryModel> GetCountryById(long Cnty_Id);
        Task<ResponseModel> AddUpdateCountry(ManageCountryModel contry);
        void Save();
    }
}
