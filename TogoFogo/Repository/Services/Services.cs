using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Filters;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class Services : IServices
    {

        private readonly ApplicationDbContext _context;
        public Services()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<ServiceModel> GetService(FilterModel filterModel)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@SERVICEID", ToDBNull(filterModel.ServiceId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", DBNull.Value);
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceModel>("GETSERVICESBYREFKEY @SERVICEID,@REFKEY", sp.ToArray()).FirstOrDefaultAsync();
        }

        public async Task<ServiceOfferedModel> GetServiceOfferd(FilterModel filterModel)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@SERVICEID", ToDBNull(filterModel.ServiceId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", DBNull.Value);
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceOfferedModel>("GETSERVICESBYREFKEY @SERVICEID,@REFKEY", sp.ToArray()).FirstOrDefaultAsync();
        }

        public async Task<ServiceOfferedModel> GetServiceAreaPin(FilterModel filterModel)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@SERVICEAREAID", ToDBNull(filterModel.ServiceAreaId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", DBNull.Value);
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceOfferedModel>("GETSERVICEAREAPINS @SERVICEAREAID,@REFKEY", sp.ToArray()).FirstOrDefaultAsync();
        }
        public async Task<List<ServiceOfferedModel>> GetServiceAreaPins(FilterModel filterModel)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@SERVICEAREAID", DBNull.Value);
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(filterModel.ServiceId));
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceOfferedModel>("GETSERVICEAREAPINS @SERVICEAREAID,@REFKEY", sp.ToArray()).ToListAsync();
        }
        public async Task<ResponseModel> AddEditServices(ServiceModel service)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@SERVICEID", ToDBNull(service.ServiceId));
            param.SqlDbType = SqlDbType.UniqueIdentifier;
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(service.RefKey));
            param.SqlDbType = SqlDbType.UniqueIdentifier;
            sp.Add(param);
            param = new SqlParameter("@CATEGORYID", ToDBNull(service.CategoryId));
            sp.Add(param);
            param = new SqlParameter("@SUBCATEGORYID", ToDBNull(service.SubCategoryId));
            sp.Add(param);
            param = new SqlParameter("@SERVICETYPEID", ToDBNull(service.ServiceTypeId));
            sp.Add(param);
            param = new SqlParameter("@DELIVERYTYPEID", ToDBNull(service.DeliveryTypeId));
            sp.Add(param);
            param = new SqlParameter("@SERVICECHARGES", ToDBNull(service.ServiceCharges));
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", ToDBNull(service.IsActive));
            sp.Add(param);
            param = new SqlParameter("@REMARKS", ToDBNull(service.Remarks));
            sp.Add(param);
            param = new SqlParameter("@ACTION", ToDBNull(service.EventAction));
            sp.Add(param);
            var sql = "USPAddOrEditServiceOpted @SERVICEID, @REFKEY,@CATEGORYID,@SUBCATEGORYID,@SERVICETYPEID,@DELIVERYTYPEID, @SERVICECHARGES ,@ISACTIVE ,@REMARKS,@ACTION";

            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;


        }

        public async Task<ResponseModel> AddOrEditServiceableAreaPin(ServiceOfferedModel service)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ServiceAreaId", ToDBNull(service.ServiceAreaId));
            sp.Add(param);
            param = new SqlParameter("@ServiceId", ToDBNull(service.ServiceId));
            sp.Add(param);
            param = new SqlParameter("@CountryId", ToDBNull(service.CountryId));
            sp.Add(param);
            param = new SqlParameter("@StateId", ToDBNull(service.StateId));
            sp.Add(param);
            param = new SqlParameter("@pincode", ToDBNull(service.PinCode));
            sp.Add(param);
            param = new SqlParameter("@City", ToDBNull(service.City));
            sp.Add(param);
            param = new SqlParameter("@IsActive", ToDBNull(service.IsActive));
            sp.Add(param);         
            param = new SqlParameter("@User", ToDBNull(service.UserId));
            sp.Add(param);
            param = new SqlParameter("@ACTION", ToDBNull(service.EventAction));
            sp.Add(param);
            var sql = "ADDOrEditServiceableAreaPins @ServiceAreaId, @ServiceId,@CountryId,@StateId,@pincode,@City, @IsActive ,@User ,@ACTION";
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