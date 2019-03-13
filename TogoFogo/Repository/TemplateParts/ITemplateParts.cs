using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.EmailHeaderFooters
{
    interface ITemplateParts:IDisposable
    {
        Task<List<TemplatePartModel>> GetTemplatePart();
        Task<TemplatePartModel> GetTemplatePartById(int TemplatePartId);
        Task<ResponseModel> AddUpdateDeleteTemplatePart(TemplatePartModel templatePartModel, char action);
        void Save();
    }
}
