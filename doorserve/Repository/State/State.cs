using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository.State
{
    
    public class State:IState
    {
        private readonly ApplicationDbContext _context;

        public State()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<ManageStateModel>> GetAllState(FilterModel filterModel)
        {
            var sp = new List<SqlParameter>();

            return await _context.Database.SqlQuery<ManageStateModel>("GetAllState", sp.ToArray()
                ).ToListAsync();
        }
        public async Task<ManageStateModel> GetStateById(long St_ID)
        {
            var param = new SqlParameter("@StateId", St_ID);
            return await _context.Database.SqlQuery<ManageStateModel>("USPGetStateById @StateId", param).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> AddUpdateState(ManageStateModel state)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@StateId", ToDBNull(state.St_ID));
            sp.Add(param);
            param = new SqlParameter("@StateName", ToDBNull(state.St_Name));
            sp.Add(param);
            param = new SqlParameter("@StateCode", ToDBNull(state.St_Code));
            sp.Add(param);
            param = new SqlParameter("@CntyID", ToDBNull(state.St_CntyID));
            sp.Add(param);
            param = new SqlParameter("@Remarks", ToDBNull(state.Remarks));
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)(state.IsActive));
            sp.Add(param);
            param = new SqlParameter("@User", ToDBNull(state.UserId));
            sp.Add(param);
            param = new SqlParameter("@Action", (object)state.Action);
            sp.Add(param);
            var sql = "USPInsertUpdateState @StateId,@StateName,@StateCode,@CntyID,@Remarks,@IsActive,@User,@Action";
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