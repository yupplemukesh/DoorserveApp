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

        public async Task<ResponseModel> NewCallLog(UploadedExcelModel newCall)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ID", DBNull.Value);
            sp.Add(pararm);
            pararm = new SqlParameter("@CLIENTID", newCall.ClientId);
            sp.Add(pararm);
            pararm = new SqlParameter("@isExistingCustomer", newCall.IsExistingCustomer);
            sp.Add(pararm);
            pararm = new SqlParameter("@CustMobileNubmer", ToDBNull(newCall.CustomerContactNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CustType", newCall.CustomerTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@CustName", newCall.CustomerName);
            sp.Add(pararm);
            pararm = new SqlParameter("@CustAltCont", ToDBNull(newCall.CustomerAltConNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CustEmail", ToDBNull(newCall.CustomerEmail));
            sp.Add(pararm);
            pararm = new SqlParameter("@AddressTypeId", newCall.AddressTypeId);
            sp.Add(pararm);
            pararm = new SqlParameter("@Address",  ToDBNull(newCall.Address));
            sp.Add(pararm);
            pararm = new SqlParameter("@Landmark", ToDBNull( newCall.NearLocation));
            sp.Add(pararm);
            pararm = new SqlParameter("@PinCode", ToDBNull( newCall.PinNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CountyId", ToDBNull(newCall.CountryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@StateId", ToDBNull(newCall.StateId));
            sp.Add(pararm);
            pararm = new SqlParameter("@CityId", ToDBNull(newCall.CityId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICECATEGORYID", ToDBNull(newCall.DeviceCategoryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEBRANDID", ToDBNull(newCall.DeviceBrandId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEMODELID", ToDBNull(newCall.DeviceModalId));
            sp.Add(pararm);
            pararm = new SqlParameter("@SLN", ToDBNull(newCall.DeviceSn));
            sp.Add(pararm);
            pararm = new SqlParameter("@IMEI1", ToDBNull(newCall.DeviceIMEIOne));
            sp.Add(pararm);
            pararm = new SqlParameter("@IMEI2", ToDBNull(newCall.DeviceIMEISecond));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEPURCHASEFROM", ToDBNull(newCall.PurchaseFrom));
            sp.Add(pararm);
            pararm = new SqlParameter("@DOP", ToDBNull(newCall.DOP));
            sp.Add(pararm);
            pararm = new SqlParameter("@BILLNUBMER", ToDBNull(newCall.BillNo));
            sp.Add(pararm);
            pararm = new SqlParameter("@BILLAMOUNT", ToDBNull(newCall.BillAmount));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICECONDITIONID", ToDBNull(newCall.DeviceConditionId));
            sp.Add(pararm);
            pararm = new SqlParameter("@SERVICETYPEID", ToDBNull(newCall.ServiceTypeId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DELIVERYTYPEID", ToDBNull(newCall.DeliveryTypeId));
            sp.Add(pararm);
            pararm = new SqlParameter("@ACTION", 'I');
            sp.Add(pararm);
            pararm = new SqlParameter("@USERID", newCall.UserId);
            sp.Add(pararm);
            pararm = new SqlParameter("@CompanyId", ToDBNull(newCall.CompanyId));
            sp.Add(pararm);
            var sql = "NewCallLog " +
                "@ID,@CLIENTID,@isExistingCustomer,@CustMobileNubmer,@CustType,@CustName,@CustAltCont,@CustEmail,@AddressTypeId,@Address,"+
                "@Landmark,@PinCode,@CountyId,@StateId,@CityId,@DEVICECATEGORYID,@DEVICEBRANDID,@DEVICEMODELID,@SLN,@IMEI1,@IMEI2,@DEVICEPURCHASEFROM,@DOP,"+
                "@BILLNUBMER,@BILLAMOUNT,@DEVICECONDITIONID,@SERVICETYPEID,@DELIVERYTYPEID,@ACTION,@USERID,@CompanyId ";
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