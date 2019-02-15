﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class Bank: IBank
    {
        private readonly ApplicationDbContext _context;
        public Bank()
        {
            _context = new ApplicationDbContext();

        }
        public async Task<ResponseModel> AddUpdateBankDetails(BankDetailModel bank)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@BANKID", bank.bankId);
            sp.Add(param);
            param = new SqlParameter("@BANKNAMEID", ToDBNull(bank.BankNameId));
            sp.Add(param);
            param = new SqlParameter("@BANKACCNUMBER", ToDBNull(bank.BankAccountNumber));
            sp.Add(param);
            param = new SqlParameter("@BANKCOMPATACC", ToDBNull(bank.BankCompanyName));
            sp.Add(param);

            param = new SqlParameter("@BANKBRANCH", ToDBNull(bank.BankBranchName));
            sp.Add(param);
            param = new SqlParameter("@BANKIFSC", ToDBNull(bank.BankIFSCCode));
            sp.Add(param);
            param = new SqlParameter("@BankCancelledChequeFileName", ToDBNull(bank.BankCancelledChequeFileName));
            sp.Add(param);
            param = new SqlParameter("@USER", (object)bank.UserId);
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(bank.RefKey));
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)bank.Action);
            sp.Add(param);

            var sql = "USPADDOREDITBANKDETAILS @BANKID,@BANKNAMEID,@BANKCOMPATACC,@BANKBRANCH,@BANKIFSC,@BankCancelledChequeFileName,@USER,@REFKEY ,@ACTION";


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