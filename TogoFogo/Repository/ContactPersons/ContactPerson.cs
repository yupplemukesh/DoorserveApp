using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class ContactPerson: IContactPerson
    {
        private readonly ApplicationDbContext _context;
        public ContactPerson()
        {
            _context = new ApplicationDbContext();

        }
        public async Task<ResponseModel> AddUpdateContactDetails(ContactPersonModel contact)
        {

           
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CONTACTID",ToDBNull(contact.ContactId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(contact.RefKey));  
            sp.Add(param);
            param = new SqlParameter("@CONADDRESSTYPEID", ToDBNull(contact.AddressTypeId));
            sp.Add(param);
            param = new SqlParameter("@CONCOUNTRYID", ToDBNull(contact.CountryId));
            sp.Add(param);
            param = new SqlParameter("@CONSTATEID", ToDBNull(contact.StateId));
            sp.Add(param);
            param = new SqlParameter("@CONCITYID", ToDBNull(contact.CityId));
            sp.Add(param);
            param = new SqlParameter("@CONADDRESS", ToDBNull(contact.Address));
            sp.Add(param);
            param = new SqlParameter("@CONLOCALITY", ToDBNull(contact.Locality));
            sp.Add(param);
            param = new SqlParameter("@CONNEARBYLOCATION", ToDBNull(contact.NearLocation));
            sp.Add(param);
            param = new SqlParameter("@CONPIN", ToDBNull(contact.PinNumber));
            sp.Add(param);
            param = new SqlParameter("@CONFNAME", ToDBNull(contact.ConFirstName));
            sp.Add(param);
            param = new SqlParameter("@CONLNAME", ToDBNull(contact.ConLastName));
            sp.Add(param);
            param = new SqlParameter("@CONNUMBER", ToDBNull(contact.ConMobileNumber));
            sp.Add(param);
            param = new SqlParameter("@CONEMAIL", ToDBNull(contact.ConEmailAddress));
            sp.Add(param);          
            param = new SqlParameter("@CONPANNUMBER", ToDBNull(contact.ConPanNumber));
            sp.Add(param);
            param = new SqlParameter("@CONPANFILENAME", ToDBNull(contact.ConPanFileName));
            sp.Add(param);
            param = new SqlParameter("@CONVOTERID", ToDBNull(contact.ConVoterId));
            sp.Add(param);
            param = new SqlParameter("@CONVOTERIDFILENAME", ToDBNull(contact.ConVoterIdFileName));
            sp.Add(param);
            param = new SqlParameter("@CONADHAARNUMBER", ToDBNull(contact.ConAdhaarNumber));
            sp.Add(param);
            param = new SqlParameter("@CONADHAARFILENAME", ToDBNull(contact.ConAdhaarFileName));
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)contact.Action);
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(contact.UserId));
            sp.Add(param);
            param = new SqlParameter("@ISUSER", ToDBNull(contact.IsUser));
            sp.Add(param);
            param = new SqlParameter("@USERTYPEID", ToDBNull(contact.UserTypeId));
            sp.Add(param);
            param = new SqlParameter("@DefaultPWD", ToDBNull(contact.Password));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(contact.CompanyId));
            sp.Add(param);
            var sql = "USPADDOREDITCONTACTS @CONTACTID,@REFKEY,@CONADDRESSTYPEID,@CONCOUNTRYID,@CONSTATEID,@CONCITYID,@CONADDRESS,@CONLOCALITY ,@CONNEARBYLOCATION,@CONPIN," +
                "@CONFNAME,@CONLNAME,@CONNUMBER,@CONEMAIL,@CONPANNUMBER,@CONPANFILENAME,@CONVOTERID,@CONVOTERIDFILENAME,@CONADHAARNUMBER,@CONADHAARFILENAME,@ACTION,@USER,@ISUSER,@USERTYPEID, @DefaultPWD,@CompId";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode==0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
        public async Task<List<ContactPersonModel>> GetContactPersonsByRefKey(Guid? refKey)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ContactId", DBNull.Value);
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(refKey));
            sp.Add(param);
            var sql = "USPGETCONTACTPERSONS @ContactId,@REFKEY";
            var res= await _context.Database.SqlQuery<ContactPersonModel>(sql, sp.ToArray()).ToListAsync();
            return res;
        }
        public async Task<ContactPersonModel> GetContactPersonByContactId(Guid contactId)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ContactId", ToDBNull(contactId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", DBNull.Value);
            sp.Add(param);
            var sql = "USPGETCONTACTPERSONS @ContactId,@REFKEY";
            return await _context.Database.SqlQuery<ContactPersonModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
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