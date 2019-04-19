using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
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
        public async Task<MainClientDataModel> GetUploadedList(Guid? clientId)
        {
            var mainModel = new MainClientDataModel();

            SqlParameter client = new SqlParameter("@ClientId", ToDBNull(clientId));
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GETDATAUPLOADEDBYCLIENT";
                command.CommandType = CommandType.StoredProcedure;                
                command.Parameters.Add(client);
                command.Parameters.Add(new SqlParameter("@IsExport", ToDBNull(false)));
                command.Parameters.Add(new SqlParameter("@Type", ToDBNull('A')));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    mainModel.UploadedFiles =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<FileDetailModel>(reader)
                           .ToList();                 
                    reader.NextResult();

                    mainModel.UploadedData =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<UploadedExcelModel>(reader)
                            .ToList();
                    reader.NextResult();
                    mainModel.Calls = new CallsViewModel();
                    mainModel.Calls.OpenCalls=
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<UploadedExcelModel>(reader)
                           .ToList();
                    reader.NextResult();
                    mainModel.Calls.CloseCalls =
                    ((IObjectContextAdapter)_context)
                        .ObjectContext
                        .Translate<UploadedExcelModel>(reader)
                        .ToList();

                }
            }
            return mainModel;
        }
        public async Task<CallsViewModel> GetAssingedCalls()
        {

            var mainModel = new CallsViewModel();

            SqlParameter client = new SqlParameter("@ClientId", DBNull.Value);
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GETAssignedCalls";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(client);
                command.Parameters.Add(new SqlParameter("@IsExport", ToDBNull(false)));
                command.Parameters.Add(new SqlParameter("@Type", ToDBNull('A')));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    mainModel.OpenCalls =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<UploadedExcelModel>(reader)
                           .ToList();
                    reader.NextResult();

                    mainModel.CloseCalls =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<UploadedExcelModel>(reader)
                            .ToList();
                }
            }
            return mainModel;


        }
        private object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
        public async Task<ResponseModel> UploadClientData(ClientDataModel client, DataTable table)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ClientId", client.ClientId);
            sp.Add(pararm);
            pararm = new SqlParameter("@ServiceTypeId", client.ServiceTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@DeliveryTypeId", client.DeliveryTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@FileName", "Client Data");
            sp.Add(pararm);
            pararm = new SqlParameter("@DataTable", SqlDbType.Structured)
            {
                TypeName = "ClientDataTypes",
                Value = table
            };
            sp.Add(pararm);
            pararm = new SqlParameter("@User",client.UserId);
            sp.Add(pararm);

            var sql = "uploadClientData @CLIENTID,@ServiceTypeId,@DeliveryTypeId, @FileName,@DataTable,@User";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            return res;
        }
        public async Task<MainClientDataModel> GetExportAssingedCalls(char tabIndex, Guid? clientId)
        {
            MainClientDataModel main = new MainClientDataModel();
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ClientId", ToDBNull(clientId));
            sp.Add(pararm);
            pararm = new SqlParameter("@IsExport", true);
            sp.Add(pararm);
            pararm = new SqlParameter("@Type", tabIndex);
            sp.Add(pararm);
            main.Calls = new CallsViewModel();
            var query = "GETDATAUPLOADEDBYCLIENT @ClientId, @IsExport , @Type";
            if (tabIndex == 'O')
            {
                try
                {
                    main.Calls.OpenCalls = await _context.Database.SqlQuery<UploadedExcelModel>(query, sp.ToArray()).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (tabIndex == 'C')
                main.Calls.CloseCalls = await _context.Database.SqlQuery<UploadedExcelModel>(query, sp.ToArray()).ToListAsync();
            else if (tabIndex == 'D')
                main.UploadedData = await _context.Database.SqlQuery<UploadedExcelModel>(query, sp.ToArray()).ToListAsync();

            else
                main.UploadedFiles = await _context.Database.SqlQuery<FileDetailModel>(query, sp.ToArray()).ToListAsync();

            return main;

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