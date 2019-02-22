using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;

namespace TogoFogo.Repository.ImportFiles
{
    public class UploadFiles:IUploadFiles
    {

        private readonly ApplicationDbContext _context;
        public UploadFiles()
        {
            _context = new ApplicationDbContext();

        }        
        public async Task<List<UploadedExcelModel>> GetUploadedList()
        {
            return await _context.Database.SqlQuery<UploadedExcelModel>("GETDATAUPLOADEDBYCLIENT").ToListAsync();

        }
        public async Task<ResponseModel> UploadClientData(ClientDataModel client, DataTable table)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ClientId", client.ClientId);
            sp.Add(pararm);
            pararm = new SqlParameter("@ServiceTypeId", client.ServiceTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@DataTable", SqlDbType.Structured)
            {
                TypeName = "ClientDataTypes",
                Value = table
            };
            sp.Add(pararm);
            pararm = new SqlParameter("@User",client.UserId);
            sp.Add(pararm);

            var sql = "uploadClientData @CLIENTID,@ServiceTypeId,@DataTable,@User";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            return res;
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