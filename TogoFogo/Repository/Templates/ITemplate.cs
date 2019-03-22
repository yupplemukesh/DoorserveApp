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
        Task<TemplateModel> GetTemplateByGUID(int TemplateId,Guid? GUID);        
        Task<ResponseModel> AddUpdateDeleteTemplate(TemplateModel templateModel, char action);
        Task<List<TemplateModel>> GetUploadedExcelListByGUID(Guid GUID,string MessageTypeName);
        Task<ResponseModel> DeleteUploadedExcelData(Guid GUID, string MessageTypeName, string UploadedData);
        void Save(); 
    }
}
