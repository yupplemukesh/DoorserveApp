using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.EmailHeaderFooters
{
    interface IEmailHeaderFooters:IDisposable
    {
        Task<List<EmailHeaderFooterModel>> GetEmailHeaderFooters(Filters.FilterModel filter);
        Task<EmailHeaderFooterModel> GetEmailHeaderFooterById(int EmailHeaderFooterId);
        Task<ResponseModel> AddUpdateDeleteEmailHeaderFooter(EmailHeaderFooterModel emailHeaderFooterModel, char action);
        void Save();
    }
}
