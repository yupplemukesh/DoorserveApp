using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class Organization:IOrganization
    {
        private readonly ApplicationDbContext _context;
        public Organization()
        {
            _context = new ApplicationDbContext();

        }
        public async Task<ResponseModel> AddUpdateOrgnization(OrganizationModel organization)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
       
            SqlParameter param   = new SqlParameter("@ORGID", ToDBNull(organization.OrgId));
            sp.Add(param);
            param = new SqlParameter("@ORGCODE", ToDBNull(organization.OrgCode));
            sp.Add(param);
            param = new SqlParameter("@ORGIEC", ToDBNull(organization.OrgIECNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGNAME", ToDBNull(organization.OrgName));
            sp.Add(param);
            param = new SqlParameter("@ORGStTypeId", ToDBNull(organization.OrgStatutoryType));
            sp.Add(param);
            param = new SqlParameter("@OrgAppTaxTypeId", ToDBNull(organization.OrgApplicationTaxType));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTCatId", ToDBNull(organization.OrgGSTCategory));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTNumber", ToDBNull(organization.OrgGSTNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTFileName", ToDBNull(organization.OrgGSTFileName));
            sp.Add(param);
            param = new SqlParameter("@ORGPANNumber", ToDBNull(organization.OrgPanNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGPANFileName", ToDBNull(organization.OrgPanFileName));
            sp.Add(param);
            param = new SqlParameter("@RefKey", ToDBNull(organization.RefKey));
            sp.Add(param);
            param = new SqlParameter("@USERID", ToDBNull(organization.UserId));
            sp.Add(param);
            param = new SqlParameter("@ACTION", ToDBNull(organization.Action));
            sp.Add(param);     
            var sql = "USPAddorEditOrg @ORGID,@ORGCODE,@ORGIEC,@ORGNAME,@ORGStTypeId,@OrgAppTaxTypeId,@ORGGSTCatId,@ORGGSTNumber ,@ORGGSTFileName,@ORGPANNumber," +
                "@ORGPANFileName,@RefKey,@USERID,@ACTION";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode == 1)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
        public async Task<OrganizationModel> GetOrganizationByRefKey(Guid refKey)
        {               
            var param = new SqlParameter("@REFKEY", refKey);
            var sql = "GetOrganization @REFKEY";
            return await _context.Database.SqlQuery<OrganizationModel>(sql, param).SingleOrDefaultAsync();
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