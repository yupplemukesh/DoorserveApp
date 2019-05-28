using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Models.Template;

namespace TogoFogo.Repository.EmailSmsTemplate
{
    public class Template : ITemplate
    {
        private readonly ApplicationDbContext _context;
        public Template()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<TemplateListModel> GetTemplates(FilterModel filterModel)
        {
            TemplateListModel list = new TemplateListModel();        
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPTemplateDetail";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@CompId", ToDBNull(filterModel.CompId)));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    list.Templates =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<TemplateModel>(reader)
                           .ToList();
                    reader.NextResult();

                    list.NonActionTemplates =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<TemplateModel>(reader)
                            .ToList();
                    reader.NextResult();

                    list.TemplateTrackerList =
                    ((IObjectContextAdapter)_context)
                        .ObjectContext
                        .Translate<TemplateTracker>(reader)
                        .ToList();
                }
            }

            return list;
        }
        public async Task<TemplateModel> GetTemplateByGUID(int TemplateId,Guid? GUID)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@TemplateId", TemplateId);
            sp.Add(param);
            param = new SqlParameter("@GUID",ToDBNull(  GUID));
            sp.Add(param);
      
            return await _context.Database.SqlQuery<TemplateModel>("UspGetActionNonActionListByGUID  @TemplateId,@GUID", sp.ToArray()).SingleOrDefaultAsync();
        }

        public async Task<TemplateModel> GetTemplateByActionName(string TemplateName)
        {

            SqlParameter param = new SqlParameter("@ActionName", TemplateName);
        
            return await _context.Database.SqlQuery<TemplateModel>("UspGetTemplateByActionName  @ActionName", param).SingleOrDefaultAsync();
        }
        public async Task<List<TemplateModel>> GetUploadedExcelListByGUID(Guid GUID,string MessageTypeName)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@GUID", GUID);
            sp.Add(param);
            param = new SqlParameter("@MessageTypeName", MessageTypeName);
            sp.Add(param);
           
               List<TemplateModel> list = new List<TemplateModel>();
                using (var connection = _context.Database.Connection)
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "UspGetUploadedExcelListByGUID";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(sp.ToArray());
                    using (var reader =await  command.ExecuteReaderAsync())
                    {
                        list =
                           ((IObjectContextAdapter)_context)
                               .ObjectContext
                               .Translate<TemplateModel>(reader)
                               .ToList();
                              reader.NextResult();
                    }
                }
               return list;

            }
        public async Task<ResponseModel> AddUpdateDeleteTemplate(TemplateModel templateModel, char action)
        {
            List<SqlParameter> sp = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@TemplateId", templateModel.TemplateId);
            sp.Add(param);
            param = new SqlParameter("@TemplateName", templateModel.TemplateName.Trim());
            sp.Add(param);
            param = new SqlParameter("@MailerTemplateName",ToDBNull(templateModel.MailerTemplateName));
            sp.Add(param);
            param = new SqlParameter("@TemplateTypeId", templateModel.TemplateTypeId);
            sp.Add(param);
            param = new SqlParameter("@MessageTypeName",ToDBNull(templateModel.MessageTypeName));
            sp.Add(param);
            param = new SqlParameter("@MessageTypeId", ToDBNull(templateModel.MessageTypeId));
            sp.Add(param);          
            param = new SqlParameter("@PriorityTypeId", ToDBNull(templateModel.PriorityTypeId));
            sp.Add(param);
            param = new SqlParameter("@GatewayId", ToDBNull(templateModel.GatewayId));
            sp.Add(param);
            param = new SqlParameter("@EmailHeaderFooterId", ToDBNull(templateModel.EmailHeaderFooterId));
            sp.Add(param);
            param = new SqlParameter("@ActionTypeId", ToDBNull(templateModel.ActionTypeId));
            sp.Add(param);
            param = new SqlParameter("@Subject", ToDBNull(templateModel.Subject));
            sp.Add(param);
            param = new SqlParameter("@Content", ToDBNull(templateModel.EmailBody));
            sp.Add(param);
            param = new SqlParameter("@ContentMeta", ToDBNull(templateModel.ContentMeta));
            sp.Add(param);
            param = new SqlParameter("@BccEmails", ToDBNull(templateModel.BccEmails));
            sp.Add(param);
            param = new SqlParameter("@IsSystemDefined", templateModel.IsSystemDefined);
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", templateModel.IsActive);
            sp.Add(param);
            param = new SqlParameter("@AddedBy", templateModel.UserId);
            sp.Add(param);
            param = new SqlParameter("@GUID", ToDBNull(templateModel.GUID));
            sp.Add(param);
            param = new SqlParameter("@ToEmail", ToDBNull(templateModel.ToEmail));
            sp.Add(param);
            param = new SqlParameter("@ToEmailCC", ToDBNull(templateModel.ToCCEmail));
            sp.Add(param);
            param = new SqlParameter("@UploadedEmail", ToDBNull(templateModel.UploadedEmail));
            sp.Add(param);
            param = new SqlParameter("@ToMobileNo", ToDBNull(templateModel.PhoneNumber));
            sp.Add(param);
            param = new SqlParameter("@UploadedMobile", ToDBNull(templateModel.UploadedMobile));
            sp.Add(param);
            param = new SqlParameter("@ScheduleDateTime", ToDBNull(templateModel.ScheduleDateTime));
            sp.Add(param);
            param = new SqlParameter("@TotalCount", ToDBNull(templateModel.TotalCount));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(templateModel.CompanyId));
            sp.Add(param);

            var sql = "UspInsertTemplateSave   @TemplateId,@TemplateName,@MailerTemplateName,@TemplateTypeId,@MessageTypeName,@MessageTypeId,@PriorityTypeId,@GatewayId,@EmailHeaderFooterId,@ActionTypeId,@Subject,@Content,@ContentMeta,@BccEmails,@IsSystemDefined,@IsActive,@AddedBy,@GUID,@ToEmail,@ToEmailCC,@UploadedEmail,@ToMobileNo,@UploadedMobile,@ScheduleDateTime,@TotalCount,@CompId";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;

            return res;
        }
        public async Task<ResponseModel> DeleteUploadedExcelData(Guid GUID,string MessageTypeName,string UploadedData)
        {
            List<SqlParameter> sp = new List<SqlParameter>();           
            SqlParameter param = new SqlParameter("@GUID", ToDBNull(GUID));
            sp.Add(param);
            param = new SqlParameter("@MessageTypeName", MessageTypeName);
            sp.Add(param);
            param = new SqlParameter("@RemoveUploaded", UploadedData);
            sp.Add(param);

            var sql = "UspDeleteUploadDetail   @GUID,@MessageTypeName,@RemoveUploaded";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;

            return res;
        }
        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        

        
    }
}

