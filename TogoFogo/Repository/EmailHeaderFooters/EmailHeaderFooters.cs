﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository.EmailHeaderFooters
{
    public class EmailHeaderFooters:IEmailHeaderFooters
    {
         private readonly ApplicationDbContext _context;
        public EmailHeaderFooters()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<EmailHeaderFooterModel>> GetEmailHeaderFooters()
        {          
            return await _context.Database.SqlQuery<EmailHeaderFooterModel>("USPEmailHeaderFooterDetail @EmailHeaderFooterId =null").ToListAsync();
        }
        public async Task<EmailHeaderFooterModel> GetEmailHeaderFooterById(int EmailHeaderFooterId)
        {
       
            SqlParameter param = new SqlParameter("@EmailHeaderFooterId", EmailHeaderFooterId);
            return await _context.Database.SqlQuery<EmailHeaderFooterModel>("USPEmailHeaderFooterDetail @EmailHeaderFooterId", param ).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> AddUpdateDeleteEmailHeaderFooter(EmailHeaderFooterModel emailHeaderFooterModel, char action)
        {
            var cat = "";
            foreach(var item in emailHeaderFooterModel.ActionTypeId)
            {
                cat = cat + "," + item;
            }
            cat = cat.TrimStart(',');
            cat = cat.TrimEnd(',');
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@EmailHeaderFooterId",ToDBNull(emailHeaderFooterModel.EmailHeaderFooterId));
            sp.Add(param);
            param = new SqlParameter("@ActionTypeId", (object)cat);
            sp.Add(param);
            param = new SqlParameter("@Name", (object)emailHeaderFooterModel.Name);
            sp.Add(param);
            param = new SqlParameter("@HeaderHTML", (object)emailHeaderFooterModel.HeaderHTML);
            sp.Add(param);
            param = new SqlParameter("@FooterHTML", (object)emailHeaderFooterModel.FooterHTML);
            sp.Add(param);
            param = new SqlParameter("@User", (object)emailHeaderFooterModel.AddeddBy);
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)emailHeaderFooterModel.ISACTIVE);
            sp.Add(param);
            param = new SqlParameter("@Action", (object)action);
            sp.Add(param);

            var sql = "USPEmailHeaderFooterADDUPDATE @EmailHeaderFooterId,@ActionTypeId,@Name,@HeaderHTML,@FooterHTML,@User,@ISACTIVE,@Action";
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