using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;
using TogoFogo.Models.Customer_Support;

namespace TogoFogo.Repository.Customer_Support
{
    public class CustomerSupport : ICustomerSupport
    {
        private readonly ApplicationDbContext _context;

        public CustomerSupport()
        {
            _context = new ApplicationDbContext();
        }
        public Task<ResponseModel> AllocateCallASC(AllocateCallModel allocateCalls)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> AllocateCallASP(AllocateCallModel allocateCalls)
        {
            throw new NotImplementedException();
        }

        public async Task<CallToASPModel> GetCalls()
        {
            CallToASPModel calls = new CallToASPModel();
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GETREQUESTFORASP";
                command.CommandType = CommandType.StoredProcedure;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    calls.PendingCalls =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<UploadedExcelModel>(reader)
                           .ToList();
                    reader.NextResult();

                    calls.AllocatedCalls =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<CallAllocatedToASPModel>(reader)
                            .ToList();
                }
            }
            return calls;
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