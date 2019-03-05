using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public async Task<TemplateListModel> GetTemplates()
        {
            TemplateListModel list = new TemplateListModel();
            var param = new SqlParameter("@TEMPLATEID", DBNull.Value);
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPTemplateDetail";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(param);
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
                }
            }

            return list;
        }
        public async Task<TemplateModel> GetTemplateById(int TemplateId)
        {

            SqlParameter param = new SqlParameter("@TemplateId", TemplateId);
            return await _context.Database.SqlQuery<TemplateModel>("USPTemplateDetail  @TemplateId", param).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> AddUpdateDeleteTemplate(TemplateModel templateModel, char action)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@TemplateId", templateModel.TemplateId);
            sp.Add(param);
            param = new SqlParameter("@TemplateTypeId", (object)templateModel.TemplateTypeId);
            sp.Add(param);
            param = new SqlParameter("@TemplateName", (object)templateModel.TemplateName);
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)templateModel.IsActive);
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)action);
            sp.Add(param);
            param = new SqlParameter("@IsDeleted", (object)templateModel.IsDeleted);
            sp.Add(param);
            param = new SqlParameter("@IsSystemDefined", (object)templateModel.IsSystemDefined);
            sp.Add(param);
            param = new SqlParameter("@Subject", ToDBNull(templateModel.Subject));
            sp.Add(param);
            param = new SqlParameter("@MessageTypeId", ToDBNull(templateModel.MessageTypeId));
            sp.Add(param);
            param = new SqlParameter("@PriorityType", ToDBNull(templateModel.PriorityType));
            sp.Add(param);
            param = new SqlParameter("@Content", ToDBNull(templateModel.Content));
            sp.Add(param);
            param = new SqlParameter("@ContentMeta", ToDBNull(templateModel.ContentMeta));
            sp.Add(param);
            param = new SqlParameter("@ToEmail", ToDBNull(templateModel.ToEmail));
            sp.Add(param);
            param = new SqlParameter("@BccEmails", ToDBNull(templateModel.BccEmails));
            sp.Add(param);
            param = new SqlParameter("@EmailFrom", ToDBNull(templateModel.EmailFrom));
            sp.Add(param);
            param = new SqlParameter("@EmailTo", ToDBNull(templateModel.EmailTo));
            sp.Add(param);
            param = new SqlParameter("@EmailBCC", ToDBNull(templateModel.EmailBCC));
            sp.Add(param);
            param = new SqlParameter("@EmailBody", ToDBNull(templateModel.EmailBody));
            sp.Add(param);
            param = new SqlParameter("@MessageText", ToDBNull(templateModel.MessageText));
            sp.Add(param);
            param = new SqlParameter("@USER", (object)templateModel.AddedBy);
            sp.Add(param);
            param = new SqlParameter("@SmsFrom", ToDBNull(templateModel.SmsFrom));
            sp.Add(param);
            param = new SqlParameter("@PhoneNumber", ToDBNull(templateModel.PhoneNumber));
            sp.Add(param);

           var sql = "USPTemplateADDUPDATE @TemplateId,@TemplateTypeId,@TemplateName,@ISACTIVE,@IsDeleted,@IsSystemDefined,@Subject,@MessageTypeId,@PriorityType,@Content,@ContentMeta,@ToEmail,@BccEmails," +
                "@EmailFrom,@EmailTo,@EmailBCC,@EmailBody,@MessageText,@USERID,@SmsFrom,@PhoneNumber";
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

