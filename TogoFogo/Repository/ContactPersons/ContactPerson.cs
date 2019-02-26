﻿using System;
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
            param = new SqlParameter("@CONADDRESSTYPEID", ToDBNull(contact.ConAddress.AddressTypeId));
            sp.Add(param);
            param = new SqlParameter("@CONCOUNTRYID", ToDBNull(contact.ConAddress.CountryId));
            sp.Add(param);
            param = new SqlParameter("@CONSTATEID", ToDBNull(contact.ConAddress.StateId));
            sp.Add(param);
            param = new SqlParameter("@CONCITYID", ToDBNull(contact.ConAddress.CityId));
            sp.Add(param);
            param = new SqlParameter("@CONADDRESS", ToDBNull(contact.ConAddress.Address));
            sp.Add(param);
            param = new SqlParameter("@CONLOCALITY", ToDBNull(contact.ConAddress.Locality));
            sp.Add(param);
            param = new SqlParameter("@CONNEARBYLOCATION", ToDBNull(contact.ConAddress.NearLocation));
            sp.Add(param);
            param = new SqlParameter("@CONPIN", ToDBNull(contact.ConAddress.PinNumber));
            sp.Add(param);
            param = new SqlParameter("@CONFNAME", ToDBNull(contact.ConFirstName));
            sp.Add(param);
            param = new SqlParameter("@CONLNAME", ToDBNull(contact.ConLastName));
            sp.Add(param);
            param = new SqlParameter("@CONNUMBER", ToDBNull(contact.ConMobileNumber));
            sp.Add(param);
            param = new SqlParameter("@CONEMAIL", ToDBNull(contact.ConEmailAddress));
            sp.Add(param);
            param = new SqlParameter("@ISUSER", (object)contact.IsUser);
            sp.Add(param);
            param = new SqlParameter("@USERNAME", ToDBNull(contact.UserName));
            sp.Add(param);
            param = new SqlParameter("@PASSWORD", ToDBNull(contact.Password));
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
            param = new SqlParameter("@USER", (object)contact.UserID);
            sp.Add(param);

            var sql = "USPADDOREDITCONTACTS @CONTACTID,@REFKEY,@CONADDRESSTYPEID,@CONCOUNTRYID,@CONSTATEID,@CONCITYID,@CONADDRESS,@CONLOCALITY ,@CONNEARBYLOCATION,@CONPIN," +
                "@CONFNAME,@CONLNAME,@CONNUMBER,@CONEMAIL,@ISUSER,@USERNAME,@PASSWORD,@CONPANNUMBER,@CONPANFILENAME,@CONVOTERID,@CONVOTERIDFILENAME,@CONADHAARNUMBER,@CONADHAARFILENAME,@ACTION,@USER";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode==0)
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