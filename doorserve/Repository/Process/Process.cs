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


namespace doorserve.Repository.Process
{
    public class Process:IProcesses
    {
        private readonly ApplicationDbContext _context;
        public Process()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<ProcessModel>> GetAllProcesses(FilterModel filterModel)
        {
            var sp = new List<SqlParameter>();
            var param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            return await _context.Database.SqlQuery<ProcessModel>("USPGetAllProcess @CompanyId", sp.ToArray()
                ).ToListAsync();
        }

        public async Task<ProcessModel> GetProcessesById(int ProcessId)
        {
            var param = new SqlParameter("@ProcessId", ProcessId);
            return await _context.Database.SqlQuery<ProcessModel>("USPGetProcessById @ProcessId", param).SingleOrDefaultAsync();
           

            }

        public async Task<ResponseModel> AddUpdateProcess(ProcessModel Process)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ProcessId", ToDBNull(Process.ProcessId));
            sp.Add(param);
            param = new SqlParameter("@ProcessName", ToDBNull(Process.ProcessName));
            sp.Add(param);           
            param = new SqlParameter("@ProcessCode", ToDBNull(Process.ProcessCode));
            sp.Add(param);
            param = new SqlParameter("@ProcessOwner", ToDBNull(Process.ProcessOwner));
            sp.Add(param);
            param = new SqlParameter("@Remark", ToDBNull(Process.Remark));
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)(Process.IsActive));
            sp.Add(param);
            param = new SqlParameter("@User", ToDBNull(Process.UserId));
            sp.Add(param);
            param = new SqlParameter("@CompanyId", ToDBNull(Process.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@Action", (object)Process.Action);
            sp.Add(param);
            var sql = "USPInsertUpdateProcess @ProcessId,@ProcessName,@ProcessCode,@ProcessOwner,@Remark,@IsActive,@User,@CompanyId,@Action";
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
       
        
