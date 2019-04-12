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

namespace TogoFogo.Repository
{
    public class Company:ICompany
    {
        private readonly ApplicationDbContext _context;
        public Company()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<CompanyModel>> GetCompanyDetails()
        {
            var param = new SqlParameter("@CompId", DBNull.Value);
            return await _context.Database.SqlQuery<CompanyModel>("GetCompany @CompId", param).ToListAsync();
        }
        public async Task<CompanyModel> GetCompanyDetailByCompanyId(Guid CompanyId)
        {          
                var param = new SqlParameter("@CompId", CompanyId);
                return await _context.Database.SqlQuery<CompanyModel>("GetCompany @CompId", param).SingleOrDefaultAsync();
        }

        public async Task<AgreementModel> GetAgreement(Guid CompanyId)
        {
            var param = new SqlParameter("@CompId", CompanyId);
            return await _context.Database.SqlQuery<AgreementModel>("GETAGREEMENTBYCOMP @CompId", param).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> AddUpdateDeleteCompany(CompanyModel company)
        {            
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CompId", ToDBNull(company.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@CompCode", ToDBNull(company.CompanyCode));
            sp.Add(param);
            param = new SqlParameter("@CompTypeId", ToDBNull(company.CompanyTypeId));
            sp.Add(param);
            param = new SqlParameter("@CompName", ToDBNull(company.CompanyName));
            sp.Add(param);        
            param = new SqlParameter("@CurrentCompName", ToDBNull(company.CurrentCompanyName));
            sp.Add(param);
            param = new SqlParameter("@CompDomain", ToDBNull(company.CompanyWebsiteDomainName));
            sp.Add(param);
            param = new SqlParameter("@ExpiryDate", ToDBNull(company.DomainExpiryDate));
            sp.Add(param);
            param = new SqlParameter("@LogoFileName", ToDBNull(company.CompanyLogo));
            sp.Add(param);
            param = new SqlParameter("@AndroidAppName", ToDBNull(company.AndroidAppName));
            sp.Add(param);
            param = new SqlParameter("@AndroidAppSettings", ToDBNull(company.AndroidAppSetting));
            sp.Add(param);
            param = new SqlParameter("@IOSAppName", ToDBNull(company.IOSAppName));
            sp.Add(param);
            param = new SqlParameter("@IOSAppSettings", ToDBNull(company.IOSAppSetting));
            sp.Add(param);
            param = new SqlParameter("@ACTION", ToDBNull(company.Action));
            sp.Add(param);
            param = new SqlParameter("@IsActive", ToDBNull(company.IsActive));
            sp.Add(param);         
            param = new SqlParameter("@Comments", ToDBNull(company.Comments));
            sp.Add(param);
            param = new SqlParameter("@ActiveTab", ToDBNull(company.ActiveTab));
            sp.Add(param);      
            param = new SqlParameter("@UserId", (object)company.CreatedBy);
            sp.Add(param);            
           var sql = "USPAddorEditCompany @CompId,@CompCode,@CompTypeId,@CompName,@CurrentCompName,@CompDomain , @ExpiryDate,@LogoFileName" +
                        ",@AndroidAppName,@AndroidAppSettings,@IOSAppName,@IOSAppSettings,@ACTION,@IsActive, @Comments,@ActiveTab,@UserId";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode == 1)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }

        public async Task<ResponseModel> AddOrEditAgreeement(AgreementModel agreement)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@AGRID", ToDBNull(agreement.AGRId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(agreement.RefKey));
            sp.Add(param);       
            param = new SqlParameter("@AGRSTARTDATE", ToDBNull(agreement.AgreementStartDate));
            sp.Add(param);
            param = new SqlParameter("@AGRPERIOD", ToDBNull(agreement.AgreementPeriod));
            sp.Add(param);
            param = new SqlParameter("@AGRNUMBER", ToDBNull(agreement.AgreementNumber));
            sp.Add(param);
            param = new SqlParameter("@AGRFILE", ToDBNull(agreement.AgreementFile));
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", ToDBNull(agreement.IsActive));
            sp.Add(param);
            param = new SqlParameter("@COMMENTS", ToDBNull(agreement.Comments));
            sp.Add(param);
            param = new SqlParameter("@SERVICETYPES", ToDBNull(agreement.ServiceTypes));
            sp.Add(param);
            param = new SqlParameter("@DELIVERYTYPES", ToDBNull(agreement.DeliveryTypes));
            sp.Add(param);
            param = new SqlParameter("@ACTION", ToDBNull(agreement.Action));
            sp.Add(param);
            param = new SqlParameter("@USERID", ToDBNull(agreement.CreatedBy));
            sp.Add(param);
         

            var sql = "USPADDOREDITAGREEMENT @AGRID,@REFKEY,@AGRSTARTDATE,@AGRPERIOD,@AGRNUMBER , @AGRFILE,@ISACTIVE,@COMMENTS" +
                         ",@SERVICETYPES,@DELIVERYTYPES,@ACTION,@USERID";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 1)
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