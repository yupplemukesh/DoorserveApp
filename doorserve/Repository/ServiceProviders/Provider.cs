
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

namespace doorserve.Repository.ServiceProviders
{
    public class Provider : IProvider
    {

        private readonly ApplicationDbContext _context;
        public Provider()
        {
            _context = new ApplicationDbContext();

        }

        public async Task<List<ServiceProviderModel>> GetProviders(FilterModel filterModel)
        {

            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            param = new SqlParameter("@ProviderId", ToDBNull(filterModel.RefKey));
            sp.Add(param);
            param = new SqlParameter("@UserId", ToDBNull(filterModel.UserId));
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceProviderModel>("USPGetAllProviders @CompanyId,@ProviderId,@UserId", sp.ToArray() ).ToListAsync();
        }
        public async Task<List<serviceProviderData>> GetProvidersExcel(FilterModel filterModel)
        {

            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            param = new SqlParameter("@ProviderId", ToDBNull(filterModel.RefKey));
            sp.Add(param);
            var _context1 = _context.Database;
            _context1.CommandTimeout = 600;

            return await _context1.SqlQuery<serviceProviderData>("USPGetAllProvidersForExcel @CompanyId,@ProviderId", sp.ToArray()).ToListAsync();
        }

        public async Task<ServiceProviderModel> GetProviderById(Guid? serviceProviderId)
        {
            var ProviderModel = new ServiceProviderModel();
            SqlParameter client = new SqlParameter("@ProviderId", ToDBNull(serviceProviderId));
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPGetProviderById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(client);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    ProviderModel =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ServiceProviderModel>(reader)
                            .SingleOrDefault();
                    if (ProviderModel == null)
                        ProviderModel = new ServiceProviderModel();
                    ProviderModel.CurrentProviderName = ProviderModel.ProviderName;
                  
                    reader.NextResult();

                    ProviderModel.Organization =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<OrganizationModel>(reader)
                            .SingleOrDefault();
                    if (ProviderModel.Organization != null)
                    {
                        ProviderModel.Organization.OrgGSTFileUrl = "/UploadedImages/Providers/Gsts/" + ProviderModel.Organization.OrgGSTFileName;
                        ProviderModel.Organization.OrgPanFileUrl = "/UploadedImages/Providers/PANCards/" + ProviderModel.Organization.OrgPanFileName;
                    }
                   reader.NextResult();
                    ProviderModel.ContactPersons = ReadPersons(reader);
                    reader.NextResult();
                    ProviderModel.BankDetails = ReadBanks(reader);
                    reader.NextResult();
                    ProviderModel.Services = 
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ServiceOfferedModel>(reader)
                            .ToList();

                }
            }

            return ProviderModel;

        }

       private   List<OtherContactPersonModel> ReadPersons(DbDataReader reader)
        {
            List<OtherContactPersonModel> contacts = new List<OtherContactPersonModel>();

            while (reader.Read())
            {
                var person = new OtherContactPersonModel
                { ContactId = new Guid(reader["ContactId"].ToString()),
                    RefKey = new Guid(reader["RefKey"].ToString()),
                    ConFirstName = reader["ConFirstName"].ToString(),
                    ConLastName = reader["ConLastName"].ToString(),
                    ConMobileNumber = reader["ConMobileNumber"].ToString(),
                    ConEmailAddress = reader["ConEmailAddress"].ToString(),
                    ConAdhaarNumber = reader["ConAdhaarNumber"].ToString(),
                    ConPanNumber = reader["ConPanNumber"].ToString(),
                    ConVoterId = reader["ConVoterId"].ToString(),
                    ConAdhaarFileName = reader["ConAdhaarFileName"].ToString(),
                    ConPanFileName = reader["ConPanFileName"].ToString(),
                    ConVoterIdFileName = reader["ConVoterIdFileName"].ToString(),                   
                    IsUser = Convert.ToBoolean(reader["IsUser"].ToString()),
            
                    LocationId = Convert.ToInt32(reader["LocationId"].ToString()),
              
                        AddressTypeId = Convert.ToInt32(reader["AddressTypeId"].ToString()),
                        Locality = reader["Locality"].ToString(),
                        NearLocation = reader["NearLocation"].ToString(),
                        PinNumber = reader["PinNumber"].ToString(),
                        Address = reader["Address"].ToString(),                       
                     State = reader["State"].ToString(),
                    Country = reader["Country"].ToString(),
                    LocationName = reader["LocationName"].ToString()
                };

                if (!string.IsNullOrEmpty( reader["AddresssId"].ToString()) )
                    person.AddresssId =new Guid( reader["AddresssId"].ToString());
                person.CurrentIsUser = person.IsUser;
                person.ConVoterIdFileUrl = "/UploadedImages/Providers/VoterIds/" + person.ConVoterIdFileName;
                person.ConAdhaarFileUrl = "/UploadedImages/Providers/ADHRS/" + person.ConAdhaarFileName;
                person.ConPanFileUrl = "/UploadedImages/Providers/PANCards/" + person.ConPanFileName;
                contacts.Add(person);
            }

            return contacts;

        }


        private List<BankDetailModel> ReadBanks(DbDataReader reader)
        {
            List<BankDetailModel> banks = new List<BankDetailModel>();

            while (reader.Read())
            {
                var bank = new BankDetailModel
                {
                    bankId = new Guid(reader["BANKID"].ToString()),
                    RefKey = new Guid(reader["REFKEY"].ToString()),
                    BankName = reader["BankName"].ToString(),
                    BankNameId = Convert.ToInt32(reader["BankNameId"].ToString()),
                    BankIFSCCode = reader["BankIFSCCode"].ToString(),
                    BankAccountNumber = reader["BankAccountNumber"].ToString(),
                    BankCancelledChequeFileName = reader["CancelledChequeFileName"].ToString(),
                    BankCompanyName = reader["BankCompanyname"].ToString(),
                    BankBranchName = reader["bankBranchName"].ToString(),
                    IsActive = bool.Parse(reader["isActive"].ToString())
            };

                bank.BankCancelledChequeFileUrl = "/UploadedImages/Providers/Banks/" + bank.BankCancelledChequeFileName;
                banks.Add(bank);
            }

            return banks;

        }
        public async Task<ResponseModel> AddUpdateDeleteProvider(ServiceProviderModel provider)
        {            
          
            if (provider.Organization.IsSingleCenter == null)
                provider.Organization.IsSingleCenter = false;

            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@PROVIDERID",ToDBNull(provider.ProviderId));          
            sp.Add(param);
            param = new SqlParameter("@PROVIDERCODE", ToDBNull(provider.ProviderCode));
            sp.Add(param);
            param = new SqlParameter("@PROVIDERNAME", ToDBNull(provider.ProviderName));
            sp.Add(param);         
            param = new SqlParameter("@ORGNAME", ToDBNull(provider.Organization.OrgName));
            sp.Add(param);
            param = new SqlParameter("@ORGCODE", ToDBNull(provider.Organization.OrgCode));
            sp.Add(param);
            param = new SqlParameter("@ORGIECNUMBER", ToDBNull(provider.Organization.OrgIECNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGSTATUTORYTYPE", ToDBNull(provider.Organization.OrgStatutoryType));
            sp.Add(param);
            param = new SqlParameter("@ORGAPPLICATIONTAXTYPE", ToDBNull(provider.Organization.OrgApplicationTaxType));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTCATEGORY", ToDBNull(provider.Organization.OrgGSTCategory));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTNUMBER", ToDBNull(provider.Organization.OrgGSTNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTFILEPATH", ToDBNull(provider.Organization.OrgGSTFileName));
            sp.Add(param);
            param = new SqlParameter("@ORGPANNUMBER", ToDBNull(provider.Organization.OrgPanNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGPANFILEPATH", ToDBNull(provider.Organization.OrgPanFileName));
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)provider.IsActive);
            sp.Add(param);
            param = new SqlParameter("@REMARKS", ToDBNull(provider.Remarks));
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)provider.action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)provider.CreatedBy);
            sp.Add(param);            
            param = new SqlParameter("@tab", ToDBNull(provider.Activetab));
            sp.Add(param);         
            param = new SqlParameter("@CompanyId", ToDBNull(provider.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@IsSingleCenter", ToDBNull(provider.Organization.IsSingleCenter));
            sp.Add(param);
            var sql = "USPInsertUpdateDeleteProvider @PROVIDERID,@PROVIDERCODE,@PROVIDERNAME,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER" +
                        ",@tab,@CompanyId,@IsSingleCenter";
       

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