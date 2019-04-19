
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository.Clients
{
    public class Client : IClient
    {

        private readonly ApplicationDbContext _context;
        public Client()
        {
            _context = new ApplicationDbContext();

        }

        public async Task<List<ClientModel>> GetClients()
        {
            return await _context.Database.SqlQuery<ClientModel>("USPGetAllClients").ToListAsync();
        }

        public async Task<ClientModel> GetClientByClientId(Guid? clientId)
        {
            var ClientModel = new ClientModel();
            SqlParameter client = new SqlParameter("@ClientId", ToDBNull(clientId));
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPGetClientById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(client);
                using (var reader = await command.ExecuteReaderAsync())
                {
                     ClientModel=
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ClientModel>(reader)
                            .SingleOrDefault();
                    if (ClientModel == null)
                        ClientModel = new ClientModel();
                    ClientModel.CurrentClientName = ClientModel.ClientName;
                    ClientModel.CurrentUserName = ClientModel.UserName;
                    reader.NextResult();

                    ClientModel.Organization =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<OrganizationModel>(reader)
                            .SingleOrDefault();
                    if (ClientModel.Organization != null)
                    {
                        ClientModel.Organization.OrgGSTFileUrl = "/UploadedImages/Clients/Gsts/" + ClientModel.Organization.OrgGSTFileName;
                        ClientModel.Organization.OrgPanFileUrl = "/UploadedImages/Clients/PANCards/" + ClientModel.Organization.OrgPanFileName;
                    }
                    reader.NextResult();
                    ClientModel.ContactPersons = ReadPersons(reader);
                    reader.NextResult();

                    ClientModel.BankDetails = ReadBanks(reader);

                }
            }

            return ClientModel;

        }

       private   List<ContactPersonModel> ReadPersons(DbDataReader reader)
        {
            List<ContactPersonModel> contacts = new List<ContactPersonModel>();

            while (reader.Read())
            {
                var person = new ContactPersonModel { ContactId = new Guid(reader["ContactId"].ToString()),
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
                    City = reader["City"].ToString()
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
        public async Task<ResponseModel> AddUpdateDeleteClient(ClientModel client)
        {            
            string cat = "";
            if (client.Activetab.ToLower() == "tab-1")
            {
                foreach (var item in client.DeviceCategories)
                {
                    cat = cat + "," + item;

                }
                cat = cat.TrimStart(',');
                cat = cat.TrimEnd(',');
            }
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CLIENTID",ToDBNull(client.ClientId));          
            sp.Add(param);
            param = new SqlParameter("@PROCESSID", ToDBNull(client.ProcessId));
            sp.Add(param);
            param = new SqlParameter("@CLIENTCODE", ToDBNull(client.ClientCode));
            sp.Add(param);
            param = new SqlParameter("@CLIENTNAME", ToDBNull(client.ClientName));
            sp.Add(param);
       
            param = new SqlParameter("@DEVICECATEGORIES", ToDBNull(cat));
            sp.Add(param);         
        
            param = new SqlParameter("@ORGNAME", ToDBNull(client.Organization.OrgName));
            sp.Add(param);
            param = new SqlParameter("@ORGCODE", ToDBNull(client.Organization.OrgCode));
            sp.Add(param);
            param = new SqlParameter("@ORGIECNUMBER", ToDBNull(client.Organization.OrgIECNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGSTATUTORYTYPE", ToDBNull(client.Organization.OrgStatutoryType));
            sp.Add(param);
            param = new SqlParameter("@ORGAPPLICATIONTAXTYPE", ToDBNull(client.Organization.OrgApplicationTaxType));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTCATEGORY", ToDBNull(client.Organization.OrgGSTCategory));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTNUMBER", ToDBNull(client.Organization.OrgGSTNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTFILEPATH", ToDBNull(client.Organization.OrgGSTFileName));
            sp.Add(param);
            param = new SqlParameter("@ORGPANNUMBER", ToDBNull(client.Organization.OrgPanNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGPANFILEPATH", ToDBNull(client.Organization.OrgPanFileName));
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)client.IsActive);
            sp.Add(param);
            param = new SqlParameter("@REMARKS", ToDBNull(client.Remarks));
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)client.action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)client.CreatedBy);
            sp.Add(param);            
            param = new SqlParameter("@SERVICETYPE", ToDBNull(client.ServiceTypes));
            sp.Add(param);
            param = new SqlParameter("@SERVICEDELIVERYTYPE", ToDBNull(client.ServiceDeliveryTypes));
            sp.Add(param);
            param = new SqlParameter("@tab", ToDBNull(client.Activetab));
            sp.Add(param);
            param = new SqlParameter("@ISUSER", ToDBNull(client.IsUser));
            sp.Add(param);
            param = new SqlParameter("@USERNAME", ToDBNull(client.UserName));
            sp.Add(param);
            param = new SqlParameter("@Password", ToDBNull(client.Password));
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