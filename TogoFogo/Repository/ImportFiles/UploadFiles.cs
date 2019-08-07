using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Filters;
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
        public  MainClientDataModel GetUploadedList(FilterModel filterModel)
        {
            var mainModel = new MainClientDataModel();
            var param= new SqlParameter("@ClientId", ToDBNull(filterModel.ClientId));
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GETDATAUPLOADEDBYCLIENT";
                command.CommandType = CommandType.StoredProcedure;                
                command.Parameters.Add(param);
                command.Parameters.Add(new SqlParameter("@IsExport", ToDBNull(false)));
                command.Parameters.Add(new SqlParameter("@Type", ToDBNull('A')));
                command.Parameters.Add(new SqlParameter("@CompId", ToDBNull(filterModel.CompId)));
                using (var reader =  command.ExecuteReader())
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
        public async Task<CallsViewModel> GetAssingedCalls(FilterModel filterModel)
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
                command.Parameters.Add(new SqlParameter("@CompId", ToDBNull(filterModel.CompId)));
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
            pararm = new SqlParameter("@FileName", client.FileName);
            sp.Add(pararm);
            pararm = new SqlParameter("@DataTable", SqlDbType.Structured)
            {
                TypeName = "ClientDataTypes",
                Value    =    table
            };
            sp.Add(pararm);
            pararm = new SqlParameter("@User",client.UserId);
            sp.Add(pararm);
            pararm = new SqlParameter("@compId", client.CompanyId);
            sp.Add(pararm); 
            var sql = "uploadClientData @CLIENTID,@ServiceTypeId,@DeliveryTypeId, @FileName,@DataTable,@User,@compId";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            return res;
        }
        public async Task<ResponseModel> UploadServiceProviders(ProviderFileModel provider, DataTable table)
        {
            var sp = new List<SqlParameter>();
            var pararm  = new SqlParameter("@FileName", ToDBNull( provider.FileName));
            sp.Add(pararm);
            pararm = new SqlParameter("@DataTable", SqlDbType.Structured)
            {
                TypeName = "ServiceProviders",
                Value = table
            };
            sp.Add(pararm);
            pararm = new SqlParameter("@User", provider.UserId);
            sp.Add(pararm);
            pararm=  new SqlParameter("@compId", ToDBNull(provider.CompanyId));
            sp.Add(pararm);
            var sql = "UploadServiceProviders @FileName,@DataTable, @User,@compId";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            return res;
        }
        public async Task<ResponseModel> UploadCityLocations(ProviderFileModel provider, DataTable table)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@FileName", ToDBNull(provider.FileName));
            sp.Add(pararm);
            pararm = new SqlParameter("@DataTable", SqlDbType.Structured)
            {
                TypeName = "Locations",
                Value = table
            };
            sp.Add(pararm);
            pararm = new SqlParameter("@User", provider.UserId);
            sp.Add(pararm);
           
            var sql = "UploadLocations @FileName,@DataTable, @User";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            return res;
        }

        public async Task<List<ProviderFileModel>> GetFiles(Guid? RefKey)
        {            
            var param = new SqlParameter("@RefKey", ToDBNull(RefKey));            
            return await _context.Database.SqlQuery<ProviderFileModel>("GetFileDetails @REFKEY", param).ToListAsync();
        }
        public async Task<ResponseModel> UploadServiceableAreaPins(ProviderFileModel provider, DataTable table)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@FileName", ToDBNull(provider.FileName));
            sp.Add(pararm);
             pararm = new SqlParameter("@RefKey", ToDBNull(provider.RefKey));
            sp.Add(pararm);
            pararm = new SqlParameter("@DataTable", SqlDbType.Structured)
            {
                TypeName = "AreaPinCodes",
                Value = table
            };
            sp.Add(pararm);
            pararm = new SqlParameter("@User", provider.UserId);
            sp.Add(pararm);
            pararm = new SqlParameter("@SysFileName", ToDBNull(provider.SysFileName));
            sp.Add(pararm);
            pararm = new SqlParameter("@Type", ToDBNull(provider.type));
            sp.Add(pararm);
            var sql = "UploadAreaPinCode @FileName,@RefKey,@DataTable, @User,@SysFileName,@Type";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            return res;
        }
        public async Task<MainClientDataModel> GetExportAssingedCalls(FilterModel filterModel)
        {
            MainClientDataModel main = new MainClientDataModel();
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ClientId", ToDBNull(filterModel.ClientId));
            sp.Add(pararm);
            pararm = new SqlParameter("@IsExport", true);
            sp.Add(pararm);
            pararm = new SqlParameter("@Type", ToDBNull(filterModel.tabIndex));
            sp.Add(pararm);
            pararm = new SqlParameter("@CompId", ToDBNull(filterModel.CompId));
            sp.Add(pararm);
            main.Calls = new CallsViewModel();
            var query = "GETDATAUPLOADEDBYCLIENT @ClientId, @IsExport , @Type,@CompId";
            if (filterModel.tabIndex == 'O')
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
            else if (filterModel.tabIndex == 'C')
                main.Calls.CloseCalls = await _context.Database.SqlQuery<UploadedExcelModel>(query, sp.ToArray()).ToListAsync();
            else if (filterModel.tabIndex == 'D')
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