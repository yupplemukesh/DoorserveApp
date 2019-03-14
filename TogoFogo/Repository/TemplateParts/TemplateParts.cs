﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository.EmailHeaderFooters
{
    public class TemplateParts : ITemplateParts
    {
         private readonly ApplicationDbContext _context;
        public TemplateParts()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<TemplatePartModel>> GetTemplatePart()
        {          
            return await _context.Database.SqlQuery<TemplatePartModel>("USPTemplatePartDetail @TemplatePartId =null").ToListAsync();
        }
        public async Task<TemplatePartModel> GetTemplatePartById(int TemplatePartId)
        {
       
            SqlParameter param = new SqlParameter("@TemplatePartId", TemplatePartId);
            return await _context.Database.SqlQuery<TemplatePartModel>("USPTemplatePartDetail @TemplatePartId", param ).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> AddUpdateDeleteTemplatePart(TemplatePartModel templatePartModel, char action)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@TemplatePartId", ToDBNull(templatePartModel.TemplatePartId));
            sp.Add(param);
            param = new SqlParameter("@TemplatePartName", (object)templatePartModel.TemplatePartName);
            sp.Add(param);
            param = new SqlParameter("@HTMLPart", (object)templatePartModel.HTMLPart);
            sp.Add(param);
            param = new SqlParameter("@PlainTextPart", (object)templatePartModel.PlainTextPart);
            sp.Add(param);
            param = new SqlParameter("@User", (object)templatePartModel.AddeddBy);
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)templatePartModel.IsActive);
            sp.Add(param);
            param = new SqlParameter("@Action", (object)action);
            sp.Add(param);

            var sql = "USPTemplatePartADDUPDATE @TemplatePartId,@TemplatePartName,@HTMLPart,@PlainTextPart,@User,@IsActive,@Action";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;

            return res;
        }
        private  object ToDBNull(object value)
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