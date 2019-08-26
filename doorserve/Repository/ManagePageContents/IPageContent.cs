using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.ManagePageContents
{
    interface IPageContent:IDisposable
    {
        Task<List<ManagePageContentsModel>> GetAllPageContent(FilterModel filter);
        Task<ManagePageContentsModel> GetPageContentById(Guid ContentId);
        Task<ResponseModel> AddUpdatePageContent(ManagePageContentsModel PageContent);
        void Save();
    }
}
