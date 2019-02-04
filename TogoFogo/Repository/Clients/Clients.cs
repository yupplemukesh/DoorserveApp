using System;
using System.Collections.Generic;
using System.Data;
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
            SqlParameter client = new SqlParameter("@ClientId", clientId);
            return await _context.Database.SqlQuery<ClientModel>("USPGetClientById @ClientId", client).SingleOrDefaultAsync();
        }

        public async Task<bool> AddUpdateDeleteClient(ClientModel client, char action)
        {
            bool result = false;
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
            param = new SqlParameter("@PROCESSNAME", (object)client.ProcessName);
            sp.Add(param);
            param = new SqlParameter("@CLIENTCODE", (object)client.ClientCode);
            sp.Add(param);
            param = new SqlParameter("@ClientName", (object)client.ClientName);
            sp.Add(param);
       
            param = new SqlParameter("@DeviceCategories", (object)cat);
            sp.Add(param);
            param = new SqlParameter("@ServiceDeliveryType", (object)client.ServiceDeliveryTypes);
            sp.Add(param);
            param = new SqlParameter("@ServiceType", (object)client.ServiceTypes);
            sp.Add(param);
            param = new SqlParameter("@ORGGSTCATEGORY", (object)client.Organization.OrgGSTCategory);
            sp.Add(param);
            param = new SqlParameter("@OrgName", (object)client.Organization.OrgName);
            sp.Add(param);
            param = new SqlParameter("@OrgCode", (object)client.Organization.OrgCode);
            sp.Add(param);
            param = new SqlParameter("@OrgIECNumber", ToDBNull(client.Organization.OrgIECNumber));
            sp.Add(param);
            param = new SqlParameter("@OrgStatutoryType", ToDBNull(client.Organization.OrgStatutoryType));
            sp.Add(param);
            param = new SqlParameter("@OrgApplicationTaxType", ToDBNull(client.Organization.OrgApplicationTaxType));
            sp.Add(param);
            param = new SqlParameter("@OrgGSTNumber", ToDBNull(client.Organization.OrgGSTNumber));
            sp.Add(param);
            param = new SqlParameter("@OrgGSTFilePath", ToDBNull(client.Organization.OrgGSTFileName));
            sp.Add(param);
            param = new SqlParameter("@ORGPANNUMBER", ToDBNull(client.Organization.OrgPanNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGPANFILEPATH", ToDBNull(client.Organization.OrgPanFileName));
            sp.Add(param);
            param = new SqlParameter("@CONADDRESSTYPE", ToDBNull(client));
            sp.Add(param);
            //param = new SqlParameter("@CONCOUNTRY", ToDBNull(client.ConCountry));
            //sp.Add(param);
            //param = new SqlParameter("@CONSTATE", ToDBNull(client.ConState));
            //sp.Add(param);
            //param = new SqlParameter("@CONCITY", ToDBNull(client.ConCity));
            //sp.Add(param);
            //param = new SqlParameter("@CONADDRESS", ToDBNull(client.ConAddress));
            //sp.Add(param);
            //param = new SqlParameter("@CONLOCALITY", ToDBNull(client.ConLocality));
            //sp.Add(param);
            //param = new SqlParameter("@CONNEARBYLOCATION", ToDBNull(client.ConNearByLocation));
            //sp.Add(param);
            //param = new SqlParameter("@CONPIN", ToDBNull(client.ConPinNumber));
            //sp.Add(param);
            //param = new SqlParameter("@CONFNAME", ToDBNull(client.ConFirstName));
            //sp.Add(param);
            //param = new SqlParameter("@CONLNAME", ToDBNull(client.ConLastName));
            //sp.Add(param);
            //param = new SqlParameter("@CONMOBILENUMBER", ToDBNull(client.ConMobileNumber));
            //sp.Add(param);
            //param = new SqlParameter("@CONEMAIL", ToDBNull(client.ConEmailAddress));
            //sp.Add(param);
            //param = new SqlParameter("@ISUSER", client.IsUser);
            //sp.Add(param);
            //param = new SqlParameter("@USERNAME", ToDBNull(client.UserName));
            //sp.Add(param);
            //param = new SqlParameter("@PASSWORD", ToDBNull(client.Password));
            //sp.Add(param);
            //param = new SqlParameter("@CONPANNUMBER", ToDBNull(client.ConPanNumber));
            //sp.Add(param);
            //param = new SqlParameter("@CONPANFILEPATH", ToDBNull(client.ConPanFileName));
            //sp.Add(param);
            //param = new SqlParameter("@CONVOTERID", ToDBNull(client.ConVoterId));
            //sp.Add(param);
            //param = new SqlParameter("@CONVOTERIDFILEPATH", ToDBNull(client.ConVoterIdFileName));
            //sp.Add(param);
            //param = new SqlParameter("@CONADHAARNUMBER", ToDBNull(client.ConAdhaarNumber));
            //sp.Add(param);
            //param = new SqlParameter("@CONADHAARFILEPATH", ToDBNull(client.ConAdhaarFileName));
            //sp.Add(param);
            //param = new SqlParameter("@BANKNAME", ToDBNull(client.BankName));
            //sp.Add(param);
            //param = new SqlParameter("@BANKACCNUMBER", ToDBNull(client.BankAccountNumber));
            //sp.Add(param);
            //param = new SqlParameter("@BANKCOMPNAME", ToDBNull(client.BankCompanyName));
            //sp.Add(param);
            //param = new SqlParameter("@BANKIFSC", ToDBNull(client.BankIFSCCode));
            //sp.Add(param);
            //param = new SqlParameter("@BANKBRANCHNAME", ToDBNull(client.BankBranchName));
            //sp.Add(param);
            //param = new SqlParameter("@BANKCANCELLEDCHEQUEFILEPATH", ToDBNull(client.BankCancelledChequeFileName));
            //sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)client.IsActive);
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)client.CreatedBy);
            sp.Add(param);
            param = new SqlParameter("@REMARKS", ToDBNull(client.Remarks));
            sp.Add(param);
         

            var sql = "USPInsertUpdateDeleteClient @CLIENTID,@PROCESSNAME,@CLIENTCODE,@CLIENTNAME,@DEVICECATEGORIES,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH,@CONADDRESSTYPE,@CONCOUNTRY,@CONSTATE, @CONCITY, @CONADDRESS , @CONLOCALITY, @CONNEARBYLOCATION,@CONPIN ,@CONFNAME ,@CONLNAME,@CONMOBILENUMBER," +
                        "@CONEMAIL,@ISUSER,@USERNAME,@PASSWORD,@CONPANNUMBER,@CONPANFILEPATH,@CONVOTERID,@CONVOTERIDFILEPATH, @CONADHAARNUMBER ,@CONADHAARFILEPATH ,@BANKNAME , @BANKACCNUMBER , @BANKCOMPNAME ,@BANKIFSC," +
                        "@BANKBRANCHNAME , @BANKCANCELLEDCHEQUEFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER ,@ServiceType,@ServiceDeliveryType";

 
       

            var res = await _context.Database.SqlQuery<string>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res == "Ok")
                result = true;

            return result;
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
    }
}