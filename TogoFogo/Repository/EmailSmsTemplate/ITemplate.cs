using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Template;

namespace TogoFogo.Repository.EmailSmsTemplate
{
   public interface ITemplate:IDisposable
    {
        Task<List<TemplateModel>> GetTemplateByType(int TemplateType);
        Task<TemplateModel> GetTemplateById(int TemplateId);
        Task<ResponseModel> AddUpdateDeleteTemplate(TemplateModel templateModel, char action);
        void Save();
    }
}
