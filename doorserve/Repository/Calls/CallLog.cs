using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.ClientData;
using doorserve.Models.ServiceCenter;
using doorserve.Models;

namespace doorserve.Repository
{
    public class CallLog:ICallLog
    {
        private readonly ApplicationDbContext _context;
        public CallLog()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<PreviousCallModel> GetPreviousCall(FilterModel filterModel)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@SN", ToDBNull(filterModel.DeviceSN));
            sp.Add(param);
             param = new SqlParameter("@CategoryId", ToDBNull(filterModel.CategoryId));
            sp.Add(param);
            param = new SqlParameter("@IMEI", ToDBNull(filterModel.IMEI));
            sp.Add(param);
            return await _context.Database.SqlQuery<PreviousCallModel>("GetRepeatCallDetails @SN,@CategoryId,@IMEI", sp.ToArray()).FirstOrDefaultAsync();
        }
        public async Task<ResponseModel> AddOrEditCallLog(CallDetailsModel Call)
        {
            var sp = new List<SqlParameter>();
            var pararm = new SqlParameter("@ID", ToDBNull(Call.Id));
            sp.Add(pararm);
            pararm = new SqlParameter("@CLIENTID", ToDBNull(Call.ClientId));
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
            pararm = new SqlParameter("@LocationId", ToDBNull(Call.LocationId));
            sp.Add(pararm);
            pararm = new SqlParameter("@City", ToDBNull(Call.District));
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
            //pararm = new SqlParameter("@DOP", ToDBNull(DateTime.ParseExact(Call.DOP, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
            pararm = new SqlParameter("@DOP", ToDBNull((Call.DOP != null) ? DateTime.ParseExact(Call.DOP, "dd/MM/yyyy", CultureInfo.InvariantCulture) : Call.AppointmentDate));
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
            pararm = new SqlParameter("@SubCategoryId", ToDBNull(Call.DeviceSubCategoryId));
            sp.Add(pararm);
            pararm = new SqlParameter("@ModelNumber", ToDBNull(Call.DeviceModelNo));
            sp.Add(pararm);
            pararm = new SqlParameter("@Remarks", ToDBNull(Call.Remarks));
            sp.Add(pararm);
            pararm = new SqlParameter("@StatusId", ToDBNull(Call.AppointmentStatus));
            sp.Add(pararm);
            pararm = new SqlParameter("@AppointmentDateTime", ToDBNull(Call.AppointmentDate));
            sp.Add(pararm);
            pararm = new SqlParameter("@ProblemDescription", ToDBNull(Call.ProblemDescription));
            sp.Add(pararm);
            //pararm = new SqlParameter("@IssueOcurringSinceDate", ToDBNull(DateTime.ParseExact(Call.IssueOcurringSinceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
            pararm = new SqlParameter("@IssueOcurringSinceDate", ToDBNull((Call.IssueOcurringSinceDate != null) ? DateTime.ParseExact(Call.IssueOcurringSinceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) : Call.AppointmentDate));
            sp.Add(pararm);
            pararm = new SqlParameter("@PreviousCallId", ToDBNull(Call.PreviousCallId));
            sp.Add(pararm);
            pararm = new SqlParameter("@DataSourceId", ToDBNull(Call.DataSourceId));
            sp.Add(pararm);
            var sql = "AddEditCallLog " +
                "@ID,@CLIENTID,@isExistingCustomer,@CustMobileNubmer,@CustType,@CustName,@CustAltCont,@CustEmail,@AddressTypeId,@Address," +
                "@Landmark,@PinCode,@LocationId,@City,@DEVICECATEGORYID,@DEVICEBRANDID,@DEVICEMODELID,@SLN,@IMEI1,@IMEI2,@DEVICEPURCHASEFROM,@DOP," +
                "@BILLNUBMER,@BILLAMOUNT,@DEVICECONDITIONID,@SERVICETYPEID,@DELIVERYTYPEID,@ACTION,@USERID,@CompanyId,@DEVICEID,@CUSTOMERID,@SubCategoryId,@ModelNumber,@Remarks," +
                "@StatusId,@AppointmentDateTime,@ProblemDescription,@IssueOcurringSinceDate,@PreviousCallId,@DataSourceId";


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
        public List<UploadedExcelModel> GetClientCalls(FilterModel filter)
        {
            var sp = new List<SqlParameter>();
            var param = new SqlParameter("@ClientId", ToDBNull(filter.ClientId));
            sp.Add(param);
            param = new SqlParameter("@IsExport", ToDBNull(false));
            sp.Add(param);
            param = new SqlParameter("@Type", ToDBNull(filter.Type));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(filter.CompId));
            sp.Add(param);
            return _context.Database.SqlQuery<UploadedExcelModel>("GETDATAUPLOADEDBYCLIENT @ClientId,@IsExport,@Type,@CompId", sp.ToArray()).ToList();
        }
        public List<FileDetailModel> GetFileList(FilterModel filter)
        {
            var sp = new List<SqlParameter>();
            var param = new SqlParameter("@ClientId", ToDBNull(filter.ClientId));
            sp.Add(param);
            param = new SqlParameter("@IsExport", ToDBNull(false));
            sp.Add(param);
            param = new SqlParameter("@Type", ToDBNull(filter.Type));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(filter.CompId));
            sp.Add(param);
            return _context.Database.SqlQuery<FileDetailModel>("GETDATAUPLOADEDBYCLIENT @ClientId,@IsExport,@Type,@CompId", sp.ToArray()).ToList();
        }
        public async Task<List<UploadedExcelModel>> GetExclatedCalls(FilterModel filter)
        {
            var param = new SqlParameter("@CompID", ToDBNull(filter.CompId));
            var sp = new List<SqlParameter>();
            sp.Add(param);
            param = new SqlParameter("@Type", ToDBNull(filter.Type));
            sp.Add(param);
            param = new SqlParameter("@USERId", ToDBNull(filter.UserId));
            sp.Add(param);
            return await _context.Database.SqlQuery<UploadedExcelModel>("GETPendingExCalls @CompId, @Type,@USERId", sp.ToArray()).ToListAsync();
        }     
        public async Task<List<UploadedExcelModel>> GetCancelRequestedData(FilterModel filter)
        {
            var param = new SqlParameter("@CompID", ToDBNull(filter.ClientId));
            return await _context.Database.SqlQuery<UploadedExcelModel>("GetCallsForCancelledRequest @CompId", param).ToListAsync();
        }
        public async Task<List<CallHistory>> GetCallHistory(FilterModel filter)
        {
            var param = new SqlParameter("@DeviceId", ToDBNull(filter.RefKey));
            return await _context.Database.SqlQuery<CallHistory>("GetOrderHisotry @DeviceId", param).ToListAsync();
        }       
    }
}