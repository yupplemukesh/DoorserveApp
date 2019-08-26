using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.ClientData;
using doorserve.Models.Customer_Support;

namespace doorserve.Repository.Customer_Support
{
    public class CustomerSupport : ICustomerSupport
    {
        private readonly ApplicationDbContext _context;

        public CustomerSupport()
        {
            _context = new ApplicationDbContext();
        }
     

        public async Task<ResponseModel> AllocateCall(AllocateCallModel allocateCalls)
        {

            XmlSerializer xsSubmit = new XmlSerializer(allocateCalls.SelectedDevices.GetType());

            string xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, allocateCalls.SelectedDevices);
                    xml = sww.ToString(); // Your XML
                }
            }

          


            //Remove the title element.


            xml= xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@AllocateId", ToDBNull(allocateCalls.AllocateId));
            sp.Add(param);
            param = new SqlParameter("@AllocateXML", SqlDbType.Xml) {Value=xml };
            sp.Add(param);
            param = new SqlParameter("@AllocateTo", ToDBNull(allocateCalls.AllocateTo));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(allocateCalls.UserId));
            sp.Add(param);
            var sql = "ALLOCATECALL @AllocateId,@AllocateXML,@AllocateTo,@USER";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;

            return res;


        }

        public async Task<CallToASPModel> GetASPCalls(FilterModel filter)
        {
            CallToASPModel calls = new CallToASPModel();
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GETREQUESTFORASP";
                command.CommandType = CommandType.StoredProcedure;               
                command.Parameters.Add(new SqlParameter("@Status", ToDBNull(filter.tabIndex)));
                command.Parameters.Add(new SqlParameter("@Export",  ToDBNull(filter.IsExport)));
                command.Parameters.Add(new SqlParameter("@CompId", ToDBNull(filter.CompId)));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    calls.PendingCalls =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<UploadedExcelModel>(reader)
                           .ToList();
                    reader.NextResult();

                    calls.AllocatedCalls =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<CallAllocatedToASPModel>(reader)
                            .ToList();
                }
            }
            return calls;
        }

        public async Task<CallToASCModel> GetASCCalls(FilterModel filter)
        {
            CallToASCModel calls = new CallToASCModel();
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
             
                command.CommandText = "GETREQUESTFORASC";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Status", ToDBNull(filter.tabIndex)));
                command.Parameters.Add(new SqlParameter("@Export", ToDBNull(filter.IsExport)));
                command.Parameters.Add(new SqlParameter("@ProviderId", ToDBNull(filter.ProviderId)));
                command.Parameters.Add(new SqlParameter("@CompId", ToDBNull( filter.CompId)));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (filter.IsExport)
                    {
                        if(filter.tabIndex=='P')
                        calls.PendingCalls =
                           ((IObjectContextAdapter)_context)
                               .ObjectContext
                               .Translate<CallAllocatedToASPModel>(reader)
                               .ToList();
                        else
                            calls.AllocatedCalls =
                           ((IObjectContextAdapter)_context)
                               .ObjectContext
                               .Translate<CallAllocatedToASCModel>(reader)
                               .ToList();

                    }
                    else
                    {
                        calls.PendingCalls =
                         ((IObjectContextAdapter)_context)
                             .ObjectContext
                             .Translate<CallAllocatedToASPModel>(reader)
                             .ToList();
                        reader.NextResult();

                        calls.AllocatedCalls =
                            ((IObjectContextAdapter)_context)
                                .ObjectContext
                                .Translate<CallAllocatedToASCModel>(reader)
                                .ToList();
                    }
                }
            }
            return calls;
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