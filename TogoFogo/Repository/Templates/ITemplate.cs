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
        Task<TemplateListModel> GetTemplates();
        Task<TemplateModel> GetTemplateById(int TemplateId);
        //Task<TemplateListModel> GetTemplatesByMessageTypeActionType(int MessageTypeId, int ActionTypeId, string MailerTemplateName);
        Task<ResponseModel> AddUpdateDeleteTemplate(TemplateModel templateModel, char action);
      
        void Save();
    }
}
