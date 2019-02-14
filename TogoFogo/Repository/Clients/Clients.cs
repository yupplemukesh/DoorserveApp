using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository.Clients
{
    public class Clients : IClients
    {

        private readonly ApplicationDbContext _context;
        public Clients()
        {
            _context = new ApplicationDbContext();

        }

        public async Task<List<ClientModel>> GetClients()
        {
            return await _context.Database.SqlQuery<ClientModel>("USPGetAllClients").ToListAsync();
        }

        public async Task<ClientModel> GetClientByClientId(Guid clientId)
        {
            var ClientModel = new ClientModel();
            SqlParameter client = new SqlParameter("@ClientId", clientId);
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
                    reader.NextResult();

                    ClientModel.Organization =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<OrganizationModel>(reader)
                            .SingleOrDefault();
                    reader.NextResult();

                    ClientModel.ContactPersons =
                     ((IObjectContextAdapter)_context)
                         .ObjectContext
                         .Translate<ContactPersonModel>(reader)
                         .ToList();
                    reader.NextResult();

                    ClientModel.BankDetails =
                   ((IObjectContextAdapter)_context)
                       .ObjectContext
                       .Translate<BankDetailModel>(reader)
                       .ToList();

                }
            }

            return ClientModel;

        }

        public async Task<ResponseModel> AddUpdateDeleteClient(ClientModel client)
        {            
            string cat = "";
           foreach(var item in client.DeviceCategories)
            {
                cat = cat + "," + item;

            }
            cat = cat.TrimStart(',');
            cat = cat.TrimEnd(',');

            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CLIENTID",client.ClientId);          
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

            var sql = "USPInsertUpdateDeleteClient @CLIENTID,@PROCESSID,@CLIENTCODE,@CLIENTNAME,@DEVICECATEGORIES,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER,@SERVICETYPE,@SERVICEDELIVERYTYPE";
       

            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
      
        public static object ToDBNull(object value)
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

        public async Task<ResponseModel> AddUpdateBankDetails(BankDetailModel bank)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@BANKID", bank.bankId);
            sp.Add(param);
            param = new SqlParameter("@BANKNAMEID", (object)bank.BankNameId);
            sp.Add(param);
            param = new SqlParameter("@BANKACCNUMBER", (object)bank.BankAccountNumber);
            sp.Add(param);
            param = new SqlParameter("@BANKCOMPATACC", (object)bank.BankCompanyName);
            sp.Add(param);

            param = new SqlParameter("@BANKBRANCH", (object)bank.BankBranchName);
            sp.Add(param);
            param = new SqlParameter("@BANKIFSC", (object)bank.BankIFSCCode);
            sp.Add(param);
            param = new SqlParameter("@BankCancelledChequeFileName", (object)bank.BankCancelledChequeFileName);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)bank.UserId);
            sp.Add(param);
            param = new SqlParameter("@CLIENTID", (object)bank.ClientId);
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)bank.Action);
            sp.Add(param);   

            var sql = "USPADDOREDITBANKDETAILS @BANKID,@BANKNAMEID,@BANKCOMPATACC,@BANKBRANCH,@BANKIFSC,@BankCancelledChequeFileName,@USER,@CLIENTID ,@ACTION";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.Response == "Ok")
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }

        public async Task<ResponseModel> AddUpdateContactDetails(ContactPersonModel contact)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CONTACTID", contact.ContactId);
            sp.Add(param);
            param = new SqlParameter("@CLIENTID", (object)contact.ClientId);
            sp.Add(param);
            param = new SqlParameter("@CONADDRESSTYPEID", (object)contact.ConAddressType);
            sp.Add(param);
            param = new SqlParameter("@CONCOUNTRYID", (object)contact.ConCountry);
            sp.Add(param);
            param = new SqlParameter("@CONSTATEID", (object)contact.ConState);
            sp.Add(param);
            param = new SqlParameter("@CONCITYID", (object)contact.ConCity);
            sp.Add(param);
            param = new SqlParameter("@CONADDRESS", (object)contact.ConCountry);
            sp.Add(param);
            param = new SqlParameter("@CONLOCALITY", (object)contact.ConLocality);
            sp.Add(param);
            param = new SqlParameter("@CONNEARBYLOCATION", (object)contact.ConNearByLocation);
            sp.Add(param);
            param = new SqlParameter("@CONPIN", (object)contact.ConPinNumber);
            sp.Add(param);
            param = new SqlParameter("@CONFNAME", (object)contact.ConFirstName);
            sp.Add(param);
            param = new SqlParameter("@CONLNAME", (object)contact.ConLastName);
            sp.Add(param);
            param = new SqlParameter("@CONNUMBER", (object)contact.ConMobileNumber);
            sp.Add(param);
            param = new SqlParameter("@CONEMAIL", (object)contact.ConEmailAddress);
            sp.Add(param);
            param = new SqlParameter("@ISUSER", (object)contact.IsUser);
            sp.Add(param);
            param = new SqlParameter("@USERNAME", (object)contact.UserName);
            sp.Add(param);
            param = new SqlParameter("@PASSWORD", (object)contact.Password);
            sp.Add(param);
            param = new SqlParameter("@CONPANNUMBER", (object)contact.ConPanNumber);
            sp.Add(param);
            param = new SqlParameter("@CONPANFILENAME", (object)contact.ConPanFileName);
            sp.Add(param);
            param = new SqlParameter("@CONVOTERID", (object)contact.ConVoterId);
            sp.Add(param);
            param = new SqlParameter("@CONVOTERIDFILENAME", (object)contact.ConVoterIdFileName);
            sp.Add(param);
            param = new SqlParameter("@CONADHAARNUMBER", (object)contact.ConAdhaarNumber);
            sp.Add(param);
            param = new SqlParameter("@CONADHAARFILENAME", (object)contact.ConAdhaarNumberFilePath);
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)contact.Action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)contact.UserID);
            sp.Add(param);

            var sql = "USPADDOREDITCONTACTS @CONTACTID,@CLIENTID,@CONADDRESSTYPEID,@CONCOUNTRYID,@CONSTATEID,@CONCITYID,@CONADDRESS,@CONLOCALITY ,@CONNEARBYLOCATION,@CONPIN," +
                "@CONFNAME,@CONLNAME,@CONNUMBER,@CONEMAIL,@ISUSER,@USERNAME,@PASSWORD,@CONPANNUMBER,@CONPANFILENAME,@CONVOTERID,@CONVOTERIDFILENAME,@CONADHAARNUMBER,@CONADHAARFILENAME,@ACTION,@USER";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.Response == "Ok")
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
    }
}