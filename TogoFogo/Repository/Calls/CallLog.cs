using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;

namespace TogoFogo.Repository.Calls
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
            pararm = new SqlParameter("@CustEmail", ToDBNull(newCall.CustomerEmail));
            sp.Add(pararm);
            pararm = new SqlParameter("@AddressTypeId", newCall.address.AddresssId);
            sp.Add(pararm);
            pararm = new SqlParameter("@Address",  ToDBNull(newCall.address.Address));
            sp.Add(pararm);
            pararm = new SqlParameter("@Landmark", ToDBNull( newCall.address.NearLocation));
            sp.Add(pararm);
            pararm = new SqlParameter("@PinCode", ToDBNull( newCall.address.PinNumber));
            sp.Add(pararm);
            pararm = new SqlParameter("@CountyId", ToDBNull(newCall.address.CountryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@StateId", ToDBNull(newCall.address.StateId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICECATEGORYID", ToDBNull(newCall.DeviceCategoryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEBRANDID", ToDBNull(newCall.DeviceBrandId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DEVICEMODELID", ToDBNull(newCall.DeviceModalId));
            sp.Add(pararm);
            pararm = new SqlParameter("@SLN", ToDBNull(newCall.ServiceTypeList));
            sp.Add(pararm);

            var sql = "NewCallLog @ID, @CLIENTID,@isExistingCustomer,@CustMobileNubmer, @CustType,@CustName,@User";
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