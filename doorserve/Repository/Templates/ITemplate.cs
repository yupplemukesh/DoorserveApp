using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Template;

namespace doorserve.Repository.EmailSmsTemplate
{
   public interface ITemplate:IDisposable
    {
        Task<TemplateListModel> GetTemplates(FilterModel filterModel);
        Task<TemplateModel> GetTemplateByGUID(int TemplateId,Guid? GUID);
        Task<List<TemplateModel>> GetTemplateByActionName(string TemplateName,Guid? CompId);
        Task<ResponseModel> AddUpdateDeleteTemplate(TemplateModel templateModel, char action);
        Task<List<TemplateModel>> GetUploadedExcelListByGUID(Guid GUID,string MessageTypeName);
        Task<ResponseModel> DeleteUploadedExcelData(Guid GUID, string MessageTypeName, string UploadedData);
        void Save(); 
    }
}
