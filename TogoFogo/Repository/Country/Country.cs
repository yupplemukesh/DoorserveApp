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

namespace TogoFogo.Repository.Country
{
    public class Country:ICountry
    {
        private readonly ApplicationDbContext _context;
        public Country()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<ManageCountryModel>> GetAllCountry(FilterModel filterModel)
        {
            var sp = new List<SqlParameter>();
           
            return await _context.Database.SqlQuery<ManageCountryModel>("GetCountry", sp.ToArray()
                ).ToListAsync();
        }

        public async Task<ManageCountryModel> GetCountryById(long Cnty_Id)
        {
            var param = new SqlParameter("@CntyId", Cnty_Id);
            return await _context.Database.SqlQuery<ManageCountryModel>("USPGetCountyById @CntyId", param).SingleOrDefaultAsync();


        }

        public async Task<ResponseModel> AddUpdateCountry(ManageCountryModel contry)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@cntyId", ToDBNull(contry.Cnty_Id));
            sp.Add(param);
            param = new SqlParameter("@CntyName", ToDBNull(contry.Cnty_Name));
            sp.Add(param);
            param = new SqlParameter("@Remarks", ToDBNull(contry.Remarks));
            sp.Add(param);          
            param = new SqlParameter("@IsActive", (object)(contry.IsActive));
            sp.Add(param);
            param = new SqlParameter("@User", ToDBNull(contry.UserId));
            sp.Add(param);           
            param = new SqlParameter("@Action", (object)contry.Action);
            sp.Add(param);
            var sql = "USPInsertUpdateCountry @cntyId,@CntyName,@Remarks,@IsActive,@User,@Action";
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