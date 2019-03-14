
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class Engineer : IEngineer
    {

        private readonly ApplicationDbContext _context;
        public Engineer()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<ManageEngineerModel>> GetAllEngineers(Guid? serviceCenterId, Guid? ProviderId)
        {
              return await _context.Database.SqlQuery<ManageEngineerModel>("USPGetAllEngineers @CenterId,@providerId",
                new List<SqlParameter> {
                new SqlParameter("@CenterId", ToDBNull(serviceCenterId)),
                new SqlParameter("@providerId", ToDBNull(ProviderId))
                }).ToListAsync();
        }
        public async Task<ManageEngineerModel> GetEngineerById(int engineerId)
        {
            var engineer = new ManageEngineerModel();
            SqlParameter client = new SqlParameter("@EngineerId", engineerId);
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPGetEngineerById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(client);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    engineer =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ManageEngineerModel>(reader)
                            .SingleOrDefault();
                    engineer.EngineerPhoto = "/UploadedImages/Engineers/DP/"+ engineer.EngineerPhoto;                 
                }
            }

            return engineer;

        }
        public async Task<ResponseModel> AddUpdateDeleteEngineer(ManageEngineerModel engineer)
        {                      
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@EngineerId",ToDBNull(engineer.EngineerId));          
            sp.Add(param);
            //param = new SqlParameter("@PROCESSID", ToDBNull(client.ProcessId));
            //sp.Add(param);
            //param = new SqlParameter("@CLIENTCODE", ToDBNull(client.ClientCode));
            //sp.Add(param);
            //param = new SqlParameter("@CLIENTNAME", ToDBNull(client.ClientName));
            //sp.Add(param);
       
            //param = new SqlParameter("@DEVICECATEGORIES", ToDBNull(cat));
            //sp.Add(param);         
        
            //param = new SqlParameter("@ORGNAME", ToDBNull(client.Organization.OrgName));
            //sp.Add(param);
            //param = new SqlParameter("@ORGCODE", ToDBNull(client.Organization.OrgCode));
            //sp.Add(param);
            //param = new SqlParameter("@ORGIECNUMBER", ToDBNull(client.Organization.OrgIECNumber));
            //sp.Add(param);
            //param = new SqlParameter("@ORGSTATUTORYTYPE", ToDBNull(client.Organization.OrgStatutoryType));
            //sp.Add(param);
            //param = new SqlParameter("@ORGAPPLICATIONTAXTYPE", ToDBNull(client.Organization.OrgApplicationTaxType));
            //sp.Add(param);
            //param = new SqlParameter("@ORGGSTCATEGORY", ToDBNull(client.Organization.OrgGSTCategory));
            //sp.Add(param);
            //param = new SqlParameter("@ORGGSTNUMBER", ToDBNull(client.Organization.OrgGSTNumber));
            //sp.Add(param);
            //param = new SqlParameter("@ORGGSTFILEPATH", ToDBNull(client.Organization.OrgGSTFileName));
            //sp.Add(param);
            //param = new SqlParameter("@ORGPANNUMBER", ToDBNull(client.Organization.OrgPanNumber));
            //sp.Add(param);
            //param = new SqlParameter("@ORGPANFILEPATH", ToDBNull(client.Organization.OrgPanFileName));
            //sp.Add(param);
            //param = new SqlParameter("@ISACTIVE", (object)client.IsActive);
            //sp.Add(param);
            //param = new SqlParameter("@REMARKS", ToDBNull(client.Remarks));
            //sp.Add(param);
            //param = new SqlParameter("@ACTION", (object)client.action);
            //sp.Add(param);
            //param = new SqlParameter("@USER", (object)client.CreatedBy);
            //sp.Add(param);            
        

            var sql = "USPInsertUpdateEngineer @CLIENTID,@PROCESSID,@CLIENTCODE,@CLIENTNAME,@DEVICECATEGORIES,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER,@SERVICETYPE" +
                        ",@SERVICEDELIVERYTYPE,@tab,@ISUSER,@USERNAME,@Password";
       

            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
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