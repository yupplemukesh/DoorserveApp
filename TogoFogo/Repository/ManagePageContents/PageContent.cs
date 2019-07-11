using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;

namespace TogoFogo.Repository.ManagePageContents
{
    public class PageContent:IPageContent
    {
        private readonly ApplicationDbContext _context;

        public PageContent()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<ManagePageContentsModel>> GetAllPageContent(FilterModel filter)
        {
            var sp = new List<SqlParameter>();
            var param = new SqlParameter("@CompanyId", ToDBNull(filter.CompId));
            sp.Add(param);
            param = new SqlParameter("@ContentId", ToDBNull(filter.RefKey));
            sp.Add(param);
            return await _context.Database.SqlQuery<ManagePageContentsModel>("USPGetAllPageContent @CompanyId, @ContentId", sp.ToArray()
                ).ToListAsync();
        }

        public async Task<ManagePageContentsModel> GetPageContentById(Guid ContentId)
        {
            var sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CompId", DBNull.Value);
            sp.Add(param);
             param = new SqlParameter("@ContentId", ContentId);
            sp.Add(param);
            
            return await _context.Database.SqlQuery<ManagePageContentsModel>("USPGetAllPageContent @CompId,@ContentId ", sp.ToArray()).SingleOrDefaultAsync();


        }

        public async Task<ResponseModel> AddUpdatePageContent(ManagePageContentsModel PageContent)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ContentId", ToDBNull(PageContent.ContentId));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(PageContent.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@PageId", ToDBNull(PageContent.PageId));
            sp.Add(param);
            param = new SqlParameter("@SectionId", ToDBNull(PageContent.SectionId));
            sp.Add(param);
            param = new SqlParameter("@Description", ToDBNull(PageContent.Description));
            sp.Add(param);        
            param = new SqlParameter("@MetaTitle", ToDBNull(PageContent.MetaTitle));
            sp.Add(param);
            param = new SqlParameter("@MetaNameDescription", ToDBNull(PageContent.MetaNameDescription));
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)(PageContent.IsActive));
            sp.Add(param);
            param = new SqlParameter("@User", ToDBNull(PageContent.UserId));
            sp.Add(param);
            param = new SqlParameter("@Action", (object)PageContent.EventAction);
            sp.Add(param);
            var sql = "USPInsertUpdatePageContent @ContentId,@CompId,@PageId,@SectionId,@Description,@MetaTitle,@MetaNameDescription,@isactive, @User,@Action";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }


        private object ToDBNull(object value)
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