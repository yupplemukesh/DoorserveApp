
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

namespace TogoFogo.Repository.ServiceProviders
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
            return await _context.Database.SqlQuery<ServiceProviderModel>("USPGetAllProviders @CompanyId", new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId))).ToListAsync();
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
                    ProviderModel.CurrentUserName = ProviderModel.UserName;
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
                    IsActive = Convert.ToBoolean(reader["IsActive"].ToString()),                   
                        AddresssId = new Guid(reader["AddresssId"].ToString()),
                        CityId = Convert.ToInt32(reader["CityId"].ToString()),
                        CountryId = Convert.ToInt32(reader["CountryId"].ToString()),
                        StateId = Convert.ToInt32(reader["StateId"].ToString()),
                        AddressTypeId = Convert.ToInt32(reader["AddressTypeId"].ToString()),
                        Locality = reader["Locality"].ToString(),
                        NearLocation = reader["NearLocation"].ToString(),
                        PinNumber = reader["PinNumber"].ToString(),
                        Address = reader["Address"].ToString(),
                        City= reader["City"].ToString()

                };

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
            string cat = "";
            if (provider.Activetab.ToLower() == "tab-1")
            {
                foreach (var item in provider.DeviceCategories)
                {
                    cat = cat + "," + item;

                }
                cat = cat.TrimStart(',');
                cat = cat.TrimEnd(',');
            }
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@PROVIDERID",ToDBNull(provider.ProviderId));          
            sp.Add(param);
            param = new SqlParameter("@PROCESSID", ToDBNull(provider.ProcessId));
            sp.Add(param);
            param = new SqlParameter("@PROVIDERCODE", ToDBNull(provider.ProviderCode));
            sp.Add(param);
            param = new SqlParameter("@PROVIDERNAME", ToDBNull(provider.ProviderName));
            sp.Add(param);
       
            param = new SqlParameter("@DEVICECATEGORIES", ToDBNull(cat));
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
            param = new SqlParameter("@SERVICETYPE", ToDBNull(provider.ServiceTypes));
            sp.Add(param);
            param = new SqlParameter("@SERVICEDELIVERYTYPE", ToDBNull(provider.ServiceDeliveryTypes));
            sp.Add(param);
            param = new SqlParameter("@tab", ToDBNull(provider.Activetab));
            sp.Add(param);
            param = new SqlParameter("@ISUSER", ToDBNull(provider.IsUser));
            sp.Add(param);
            param = new SqlParameter("@USERNAME", ToDBNull(provider.UserName));
            sp.Add(param);
            param = new SqlParameter("@Password", ToDBNull(provider.Password));
            sp.Add(param);
            param = new SqlParameter("@CompanyId", ToDBNull(provider.CompanyId));
            sp.Add(param);

            var sql = "USPInsertUpdateDeleteProvider @PROVIDERID,@PROCESSID,@PROVIDERCODE,@PROVIDERNAME,@DEVICECATEGORIES,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER,@SERVICETYPE" +
                        ",@SERVICEDELIVERYTYPE,@tab,@ISUSER,@USERNAME,@Password,@CompanyId";
       

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