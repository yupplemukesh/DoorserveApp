using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using doorserve.Models;

namespace doorserve.Repository
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
            SqlParameter param = new SqlParameter("@BANKID", ToDBNull(bank.bankId));
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
            param = new SqlParameter("@ACTION", (object)bank.EventAction);
            sp.Add(param);
            param = new SqlParameter("@IsDefault", (object)bank.IsDefault);
            sp.Add(param);            

            var sql = "USPADDOREDITBANKDETAILS @BANKID,@BANKNAMEID,@BANKACCNUMBER,@BANKCOMPATACC,@BANKBRANCH,@BANKIFSC,@BankCancelledChequeFileName,@USER,@REFKEY ,@ACTION,@IsDefault";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode==0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
        public async Task<List<BankDetailModel>> GetBanksByRefKey(Guid? refKey)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@BANKID", DBNull.Value);
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(refKey));
            sp.Add(param);
            var sql = "USPGETACCOUNTS @BANKID,@REFKEY";
            return await _context.Database.SqlQuery<BankDetailModel>(sql, sp.ToArray()).ToListAsync();
        }
        public async Task<BankDetailModel> GetBankByBankId(Guid bankId)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@BANKID", ToDBNull(bankId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", DBNull.Value);
            sp.Add(param);
            var sql = "USPGETACCOUNTS @BANKID,@REFKEY";
            return await _context.Database.SqlQuery<BankDetailModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
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