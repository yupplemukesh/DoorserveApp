using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Company;

namespace TogoFogo.Repository.Company
{
    public class Company
    {
        private readonly ApplicationDbContext _context;
        public Company()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<List<CompanyModel>> GetCompanyDetails()
        {
            return await _context.Database.SqlQuery<CompanyModel>("").ToListAsync();
        }

        public async Task<CompanyModel> GetCompanyDetailByCompanyId(Guid CompanyId)
        {
            var companymodel = new CompanyModel();
            SqlParameter company = new SqlParameter("",CompanyId);
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(company);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    companymodel =
                        ((IObjectContextAdapter)_context)
                        .ObjectContext
                        .Translate<CompanyModel>(reader)
                        .SingleOrDefault();
                    companymodel.CurrentCompanyName = companymodel.CompanyName;
                    reader.NextResult();
                    companymodel.Organization =
                        ((IObjectContextAdapter)_context)
                        .ObjectContext
                        .Translate<OrganizationModel>(reader)
                        .SingleOrDefault();
                    reader.NextResult();
                    companymodel.Agreement =
                        ((IObjectContextAdapter)_context)
                        .ObjectContext
                        .Translate<AgreementModel>(reader)
                        .SingleOrDefault();
                    reader.NextResult();
                    companymodel.Services = ReadServices(reader);
                    reader.NextResult();
                    companymodel.Contacts = ReadPersons(reader);
                    reader.NextResult(); 
                    companymodel.BankDetails = ReadBanks(reader);

                    

                }
               
            }
            return companymodel;
        }


        private List<ContactPersonModel> ReadPersons(DbDataReader reader)
        {
            List<ContactPersonModel> contacts = new List<ContactPersonModel>();

            while (reader.Read())
            {
                var person = new ContactPersonModel
                {
                    ContactId = new Guid(reader["ContactId"].ToString()),
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
                    isActive = Convert.ToBoolean(reader["IsActive"].ToString()),
                    AddresssId = new Guid(reader["AddresssId"].ToString()),
                    CityId = Convert.ToInt32(reader["CityId"].ToString()),
                    CountryId = Convert.ToInt32(reader["CountryId"].ToString()),
                    StateId = Convert.ToInt32(reader["StateId"].ToString()),
                    AddressTypeId = Convert.ToInt32(reader["AddressTypeId"].ToString()),
                    Locality = reader["Locality"].ToString(),
                    NearLocation = reader["NearLocation"].ToString(),
                    PinNumber = reader["PinNumber"].ToString(),
                    Address = reader["Address"].ToString()
                };

                person.ConVoterIdFileUrl = "/UploadedImages/Clients/VoterIds/" + person.ConVoterIdFileName;
                person.ConAdhaarFileUrl = "/UploadedImages/Clients/ADHRS/" + person.ConAdhaarFileName;
                person.ConPanFileUrl = "/UploadedImages/Clients/PANCards/" + person.ConPanFileName;
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

                bank.BankCancelledChequeFileUrl = "/UploadedImages/Clients/Banks/" + bank.BankCancelledChequeFileName;
                banks.Add(bank);
            }

            return banks;

        }


        private List<ServiceModel> ReadServices(DbDataReader reader)
        {
            List<ServiceModel> Services = new List<ServiceModel>();

            while (reader.Read())
            {
                var Service = new ServiceModel
                {
                    //CompanyTypeId = new Guid(reader["COMPANYTYPEID"].ToString()),
                    //RefKey = new Guid(reader["REFKEY"].ToString()),
                    ServiceName = reader["ServiceName"].ToString(),
                    Note = reader["Note"].ToString(),
                    RatePerUser = Convert.ToDecimal(reader["RatePerUser"].ToString()),                   
                    IsActive = bool.Parse(reader["isActive"].ToString())
                };
                Services.Add(Service);                
            }
            return Services;
        }

        public async Task<ResponseModel> AddUpdateDeleteCompany(CompanyModel company)
        {            
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@COMPANYID", ToDBNull(company.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@COMPANYNAME", ToDBNull(company.CompanyName));
            sp.Add(param);
            param = new SqlParameter("@COMPANYLOGO", ToDBNull(company.CompanyLogo));
            sp.Add(param);
            param = new SqlParameter("@DOMAINNAME", ToDBNull(company.CompanyWebsiteDomainName));
            sp.Add(param);        

            param = new SqlParameter("@ORGNAME", ToDBNull(company.Organization.OrgName));
            sp.Add(param);
            param = new SqlParameter("@ORGCODE", ToDBNull(company.Organization.OrgCode));
            sp.Add(param);
            param = new SqlParameter("@ORGIECNUMBER", ToDBNull(company.Organization.OrgIECNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGSTATUTORYTYPE", ToDBNull(company.Organization.OrgStatutoryType));
            sp.Add(param);
            param = new SqlParameter("@ORGAPPLICATIONTAXTYPE", ToDBNull(company.Organization.OrgApplicationTaxType));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTCATEGORY", ToDBNull(company.Organization.OrgGSTCategory));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTNUMBER", ToDBNull(company.Organization.OrgGSTNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTFILEPATH", ToDBNull(company.Organization.OrgGSTFileName));
            sp.Add(param);
            param = new SqlParameter("@ORGPANNUMBER", ToDBNull(company.Organization.OrgPanNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGPANFILEPATH", ToDBNull(company.Organization.OrgPanFileName));
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)company.IsActive);
            sp.Add(param);        
            param = new SqlParameter("@USER", (object)company.CreatedBy);
            sp.Add(param);            

           var sql = "USPInsertUpdateDeleteClient @CLIENTID,@PROCESSID,@CLIENTCODE,@CLIENTNAME,@DEVICECATEGORIES,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER,@SERVICETYPE" +
                        ",@SERVICEDELIVERYTYPE,@tab,@ISUSER,@USERNAME,@Password";


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