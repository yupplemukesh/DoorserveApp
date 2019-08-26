using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Models;

namespace doorserve.Repository.EmailHeaderFooters
{
    interface ITemplateParts:IDisposable
    {
        Task<List<TemplatePartModel>> GetTemplatePart(Filters.FilterModel filter);
        Task<TemplatePartModel> GetTemplatePartById(int TemplatePartId);
        Task<ResponseModel> AddUpdateDeleteTemplatePart(TemplatePartModel templatePartModel, char action);
        void Save();
    }
}
