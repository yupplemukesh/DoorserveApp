using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;

namespace TogoFogo.Repository
{
    public class CallLog:ICallLog
    {
        private readonly ApplicationDbContext _context;
        public CallLog()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<ResponseModel> AddOrEditCallLog(UploadedExcelModel Call)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ID", ToDBNull(Call.Id));
            sp.Add(pararm);
            pararm = new SqlParameter("@CLIENTID", Call.ClientId);
            sp.Add(pararm);
            pararm = new SqlParameter("@isExistingCustomer", Call.IsExistingCustomer);
            sp.Add(pararm);
            pararm = new SqlParameter("@CustMobileNubmer", ToDBNull(Call.CustomerContactNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CustType", Call.CustomerTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@CustName", Call.CustomerName);
            sp.Add(pararm);
            pararm = new SqlParameter("@CustAltCont", ToDBNull(Call.CustomerAltConNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CustEmail", ToDBNull(Call.CustomerEmail));
            sp.Add(pararm);
            pararm = new SqlParameter("@AddressTypeId", Call.AddressTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@Address",  ToDBNull(Call.Address));
            sp.Add(pararm);
            pararm = new SqlParameter("@Landmark", ToDBNull( Call.NearLocation));
            sp.Add(pararm);
            pararm = new SqlParameter("@PinCode", ToDBNull( Call.PinNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CountyId", ToDBNull(Call.CountryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@StateId", ToDBNull(Call.StateId));
            sp.Add(pararm);
            pararm = new SqlParameter("@CityId", ToDBNull(Call.CityId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICECATEGORYID", ToDBNull(Call.DeviceCategoryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEBRANDID", ToDBNull(Call.DeviceBrandId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEMODELID", ToDBNull(Call.DeviceModalId));
            sp.Add(pararm);
            pararm = new SqlParameter("@SLN", ToDBNull(Call.DeviceSn));
            sp.Add(pararm);
            pararm = new SqlParameter("@IMEI1", ToDBNull(Call.DeviceIMEIOne));
            sp.Add(pararm);
            pararm = new SqlParameter("@IMEI2", ToDBNull(Call.DeviceIMEISecond));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEPURCHASEFROM", ToDBNull(Call.PurchaseFrom));
            sp.Add(pararm);
            pararm = new SqlParameter("@DOP", ToDBNull(Call.DOP));
            sp.Add(pararm);
            pararm = new SqlParameter("@BILLNUBMER", ToDBNull(Call.BillNo));
            sp.Add(pararm);
            pararm = new SqlParameter("@BILLAMOUNT", ToDBNull(Call.BillAmount));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICECONDITIONID", ToDBNull(Call.DeviceConditionId));
            sp.Add(pararm);
            pararm = new SqlParameter("@SERVICETYPEID", ToDBNull(Call.ServiceTypeId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DELIVERYTYPEID", ToDBNull(Call.DeliveryTypeId));
            sp.Add(pararm);
            pararm = new SqlParameter("@ACTION", Call.EventAction);
            sp.Add(pararm);
            pararm = new SqlParameter("@USERID", Call.UserId);
            sp.Add(pararm);
            pararm = new SqlParameter("@CompanyId", ToDBNull(Call.CompanyId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEID", ToDBNull(Call.DeviceId));
            sp.Add(pararm);
            pararm = new SqlParameter("@CUSTOMERID", ToDBNull(Call.CustomerId));
            sp.Add(pararm);
            var sql = "AddEditCallLog " +
                "@ID,@CLIENTID,@isExistingCustomer,@CustMobileNubmer,@CustType,@CustName,@CustAltCont,@CustEmail,@AddressTypeId,@Address,"+
                "@Landmark,@PinCode,@CountyId,@StateId,@CityId,@DEVICECATEGORYID,@DEVICEBRANDID,@DEVICEMODELID,@SLN,@IMEI1,@IMEI2,@DEVICEPURCHASEFROM,@DOP,"+
                "@BILLNUBMER,@BILLAMOUNT,@DEVICECONDITIONID,@SERVICETYPEID,@DELIVERYTYPEID,@ACTION,@USERID,@CompanyId,@DEVICEID,@CUSTOMERID";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
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