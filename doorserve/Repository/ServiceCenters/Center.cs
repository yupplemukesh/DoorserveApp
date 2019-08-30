﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Customer_Support;
using doorserve.Models.ServiceCenter;

namespace doorserve.Repository.ServiceCenters
{
    public class Center : ICenter
    {

        private readonly ApplicationDbContext _context;
        public Center()
        {
            _context = new ApplicationDbContext();

        }

        public async Task<List<ServiceCenterModel>> GetCenters(FilterModel filterModel)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@CenterId", ToDBNull(filterModel.RefKey));
            sp.Add(param);
             param = new SqlParameter("@ProviderId", ToDBNull(filterModel.ProviderId));
            sp.Add(param);
             param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceCenterModel>("USPGetAllCenters @CenterId, @ProviderId,@CompanyId", sp.ToArray()).ToListAsync();
        }
        public async Task<CallDetailsModel> GetCallsDetailsById(string CRN)
        {

            var call = new CallDetailsModel();
            using (var connection = _context.Database.Connection)
            {
                SqlParameter param = new SqlParameter("@CallId", ToDBNull(CRN));
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GetCallDetailsByCallId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(param);
                using (var reader = await command.ExecuteReaderAsync())
                {
                     call =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<CallDetailsModel>(reader)
                            .SingleOrDefault();
                    //Add This
                    reader.NextResult();

                    call.Parts =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<PartsDetailsModel>(reader)
                            .ToList();                  
                }
            }

            return call;

            //SqlParameter callDetails = new SqlParameter("@CallId", CRN);
            //return await _context.Database.SqlQuery<CallDetailsModel>("GetCallDetailsByCallId @CallId", callDetails).SingleOrDefaultAsync();
        }


        public async Task<ServiceCenterModel> GetCenterById(Guid? serviceCenterId)
        {
            var CenterModel = new ServiceCenterModel();
            SqlParameter client = new SqlParameter("@CenterId", ToDBNull(serviceCenterId));
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPGetCenterById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(client);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    CenterModel =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ServiceCenterModel>(reader)
                            .SingleOrDefault();
                    //Add This
                    if (CenterModel == null)
                        CenterModel = new ServiceCenterModel();
                    CenterModel.CurrentCenterName = CenterModel.CenterName;
                    reader.NextResult();

                    CenterModel.Organization =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<OrganizationModel>(reader)
                            .SingleOrDefault();
                    //Add This
                    if (CenterModel.Organization != null)
                    { 
                    CenterModel.Organization.OrgGSTFileUrl = "/UploadedImages/Centers/Gsts/" + CenterModel.Organization.OrgGSTFileName;
                    CenterModel.Organization.OrgPanFileUrl = "/UploadedImages/Centers/PANCards/" + CenterModel.Organization.OrgPanFileName;
                    }
                    reader.NextResult();
                    CenterModel.ContactPersons = ReadPersons(reader);
                    reader.NextResult();

                    CenterModel.BankDetails = ReadBanks(reader);
                    reader.NextResult();
                    CenterModel.Services=
                         ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ServiceOfferedModel>(reader)
                            .ToList();
                }
            }
            return CenterModel;

        }

        private List<OtherContactPersonModel> ReadPersons(DbDataReader reader)
        {
            List<OtherContactPersonModel> contacts = new List<OtherContactPersonModel>();

            while (reader.Read())
            {
                var person = new OtherContactPersonModel
                {
                    ContactId = new Guid(reader["ContactId"].ToString()),
                    RefKey = new Guid(reader["RefKey"].ToString()),
                    ConFirstName = reader["ConFirstName"].ToString(),
                    ConLastName = reader["ConLastName"].ToString(),
                    ConMobileNumber = reader["ConMobileNumber"].ToString(),
                    ConEmailAddress = reader["ConEmailAddress"].ToString(),
                    ConAdhaarNumber = reader["ConAdhaarNumber"].ToString(),
                    ConPanNumber = reader["ConPanNumber"].ToString(),
                    ConVoterId = reader["ConVoterId"].ToString(),
                    ConAdhaarFileName = reader["ConAdhaarFileName"].ToString(),
                    ConPanFileName = reader["ConPanFileName"].ToString(),
                    ConVoterIdFileName = reader["ConVoterIdFileName"].ToString(),        
                    AddresssId = new Guid(reader["AddresssId"].ToString()),                   
              
                    AddressTypeId = Convert.ToInt32(reader["AddressTypeId"].ToString()),
                    Locality = reader["Locality"].ToString(),
                    NearLocation = reader["NearLocation"].ToString(),
                    PinNumber = reader["PinNumber"].ToString(),
                    Address = reader["Address"].ToString(),
                    LocationName = reader["LocationName"].ToString()
                };
                if (!string.IsNullOrEmpty(reader["LocationId"].ToString()))
                    person.LocationId = Convert.ToInt32(reader["LocationId"].ToString());
                person.CurrentEmail = person.ConEmailAddress;
                person.ConVoterIdFileUrl = "/UploadedImages/Centers/VoterIds/" + person.ConVoterIdFileName;
                person.ConAdhaarFileUrl = "/UploadedImages/Centers/ADHRS/" + person.ConAdhaarFileName;
                person.ConPanFileUrl = "/UploadedImages/Centers/PANCards/" + person.ConPanFileName;
                contacts.Add(person);
            }

            return contacts;

        }
        private List<BankDetailModel> ReadBanks(DbDataReader reader)
        {
            List<BankDetailModel> banks = new List<BankDetailModel>();

            while (reader.Read())
            {
                var bank = new BankDetailModel
                {
                    bankId = new Guid(reader["BANKID"].ToString()),
                    RefKey = new Guid(reader["REFKEY"].ToString()),
                    BankName = reader["BankName"].ToString(),
                    BankNameId = Convert.ToInt32(reader["BankNameId"].ToString()),
                    BankIFSCCode = reader["BankIFSCCode"].ToString(),
                    BankAccountNumber = reader["BankAccountNumber"].ToString(),
                    BankCancelledChequeFileName = reader["CancelledChequeFileName"].ToString(),
                    BankCompanyName = reader["BankCompanyname"].ToString(),
                    BankBranchName = reader["bankBranchName"].ToString(),
                    IsActive = bool.Parse(reader["isActive"].ToString())
                };

                bank.BankCancelledChequeFileUrl = "/UploadedImages/Centers/Banks/" + bank.BankCancelledChequeFileName;
                banks.Add(bank);
            }

            return banks;

        }
        public async Task<ResponseModel> AddUpdateDeleteCenter(ServiceCenterModel center)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CENTERID", ToDBNull(center.CenterId));
            sp.Add(param);
            param = new SqlParameter("@PROVIDERID", ToDBNull(center.ProviderId));
            sp.Add(param);												  
            param = new SqlParameter("@CENTERCODE", ToDBNull(center.CenterCode));
            sp.Add(param);
            param = new SqlParameter("@CENTERNAME", ToDBNull(center.CenterName));
            sp.Add(param);
            param = new SqlParameter("@ORGNAME", ToDBNull(center.Organization.OrgName));
            sp.Add(param);
            param = new SqlParameter("@ORGCODE", ToDBNull(center.Organization.OrgCode));
            sp.Add(param);
            param = new SqlParameter("@ORGIECNUMBER", ToDBNull(center.Organization.OrgIECNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGSTATUTORYTYPE", ToDBNull(center.Organization.OrgStatutoryType));
            sp.Add(param);
            param = new SqlParameter("@ORGAPPLICATIONTAXTYPE", ToDBNull(center.Organization.OrgApplicationTaxType));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTCATEGORY", ToDBNull(center.Organization.OrgGSTCategory));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTNUMBER", ToDBNull(center.Organization.OrgGSTNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGGSTFILEPATH", ToDBNull(center.Organization.OrgGSTFileName));
            sp.Add(param);
            param = new SqlParameter("@ORGPANNUMBER", ToDBNull(center.Organization.OrgPanNumber));
            sp.Add(param);
            param = new SqlParameter("@ORGPANFILEPATH", ToDBNull(center.Organization.OrgPanFileName));
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)center.IsActive);
            sp.Add(param);
            param = new SqlParameter("@REMARKS", ToDBNull(center.Remarks));
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)center.action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)center.CreatedBy);
            sp.Add(param);
																					
																									
            param = new SqlParameter("@tab", ToDBNull(center.Activetab));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(center.CompanyId));
            sp.Add(param);

            var sql = "USPInsertUpdateDeleteCenter @CENTERID,@PROVIDERID,@CENTERCODE,@CENTERNAME,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER" +
                        ",@tab,@CompId";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
        public async Task<ServiceCenterCallsModel> GetCallDetails(FilterModel filter)
        {
            ServiceCenterCallsModel calls = new ServiceCenterCallsModel();
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "GETCenterAllocatedCalls";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@companyId", ToDBNull(filter.CompId)));
                command.Parameters.Add(new SqlParameter("@IsExport", ToDBNull(filter.IsExport)));
                command.Parameters.Add(new SqlParameter("@type", ToDBNull(filter.tabIndex)));
                command.Parameters.Add(new SqlParameter("@providerId", ToDBNull(filter.ProviderId)));
                command.Parameters.Add(new SqlParameter("@RefKey", ToDBNull(filter.RefKey)));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (filter.IsExport)
                    {
                        if (filter.tabIndex == 'P')
                            calls.PendingCalls =
                             ((IObjectContextAdapter)_context)
                                 .ObjectContext
                                 .Translate<CallDetailsModel>(reader)
                                 .ToList();
                        else if (filter.tabIndex == 'A')
                            calls.AcceptedCalls =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<CallDetailsModel>(reader)
                            .ToList();
                        else
                            calls.AssignedCalls =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<CallDetailsModel>(reader)
                           .ToList();

                    }

                    else
                    {
                        calls.PendingCalls =
                           ((IObjectContextAdapter)_context)
                               .ObjectContext
                               .Translate<CallDetailsModel>(reader)
                               .ToList();
                        reader.NextResult();

                        calls.AcceptedCalls =
                     ((IObjectContextAdapter)_context)
                         .ObjectContext
                         .Translate<CallDetailsModel>(reader)
                         .ToList();
                        reader.NextResult();

                        calls.AssignedCalls =
                            ((IObjectContextAdapter)_context)
                                .ObjectContext
                                .Translate<CallDetailsModel>(reader)
                                .ToList();
                    }

                }
            }
            return calls;
        }
        public async Task<ResponseModel> UpdateCallsStatus(CallStatusModel callStatus)
        {
            XmlSerializer xsSubmit = new XmlSerializer(callStatus.SelectedDevices.GetType());

            string xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, callStatus.SelectedDevices);
                    xml = sww.ToString(); // Your XML
                }
            }

            //Remove the title element.


            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@Status", ToDBNull(callStatus.StatusId));
            sp.Add(param);

            param = new SqlParameter("@AllocateXML", SqlDbType.Xml) { Value = xml };
            sp.Add(param);
            param = new SqlParameter("@Reasion", ToDBNull(callStatus.RejectionReason));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(callStatus.UserId));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(callStatus.CompId));
            sp.Add(param);
            param = new SqlParameter("@REFKEY", ToDBNull(callStatus.RefKey));
            sp.Add(param);
            var sql = "UpdateCallStatus @Status,@AllocateXML,@Reasion,@USER,@CompId,@REFKEY";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
        public async Task<ResponseModel> AssignCallsDetails(EmployeeModel assignCalls)
        {
            XmlSerializer xsSubmit = new XmlSerializer(assignCalls.SelectedDevices.GetType());

            string xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, assignCalls.SelectedDevices);
                    xml = sww.ToString(); // Your XML
                }
            }

            //Remove the title element.


            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@EmployeeId", ToDBNull(assignCalls.EmpId));
            sp.Add(param);

            param = new SqlParameter("@AllocateXML", SqlDbType.Xml) { Value = xml };
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(assignCalls.UserId));
            sp.Add(param);
            var sql = "UpdateAssignCallsDetails @EmployeeId,@AllocateXML,@USER";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;

            return res;


        }
        public async Task<ResponseModel> UpdateCallsStatusDetails(CallStatusDetailsModel callStatusDetails)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@StatusId", ToDBNull(callStatusDetails.CStatus));
            sp.Add(param);
            param = new SqlParameter("@RejectReasion", ToDBNull(callStatusDetails.RejectionReason));
            sp.Add(param);
            param = new SqlParameter("@DeviceId", ToDBNull(callStatusDetails.DeviceId));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(callStatusDetails.UserId));
            sp.Add(param);
            param = new SqlParameter("@Type", ToDBNull(callStatusDetails.Type));
            sp.Add(param);
            var sql = "UpdateCallStatusDetails @StatusId,@RejectReasion,@DeviceId,@USER,@Type";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;

            return res;


        }
        public async Task<EmployeeModel> GetTechnicianDetails(string EmpId)
        {

            SqlParameter techDetails = new SqlParameter("@EmpId", EmpId);
            return await _context.Database.SqlQuery<EmployeeModel>("USPTechnicianDetails @EmpId", techDetails).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> UpdateCallCenterCall(CallStatusDetailsModel callStatusDetails)
        {
            string xml = "";
            if (callStatusDetails.Parts != null)
            {
                XmlSerializer parts = new XmlSerializer(callStatusDetails.Parts.GetType());

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        parts.Serialize(writer, callStatusDetails.Parts);
                        xml = sww.ToString();
                    }
                }
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@DeviceId", ToDBNull(callStatusDetails.DeviceId));
            sp.Add(param);            
            param = new SqlParameter("@EMPId", ToDBNull(callStatusDetails.EmpId));
            sp.Add(param);
            param = new SqlParameter("@ProviderId", ToDBNull(callStatusDetails.ProviderId));
            sp.Add(param);
            param = new SqlParameter("@CenterId", ToDBNull(callStatusDetails.CenterId));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(callStatusDetails.UserId));
            sp.Add(param);
            param = new SqlParameter("@StatusId", ToDBNull(callStatusDetails.AppointmentStatus));
            sp.Add(param);
            param = new SqlParameter("@AppointmentDate", ToDBNull(callStatusDetails.AppointmentDate));
            sp.Add(param);
            param = new SqlParameter("@Remarks", ToDBNull(callStatusDetails.Remarks));
            sp.Add(param);
            param = new SqlParameter("@Type", ToDBNull(callStatusDetails.Type));
            sp.Add(param);
            param = new SqlParameter("@parts", ToDBNull(xml));
            param.SqlDbType = SqlDbType.Xml;
            sp.Add(param);
            param = new SqlParameter("@InvoiceFile", ToDBNull(callStatusDetails.InvoiceFileName));
            sp.Add(param);
            param = new SqlParameter("@JobsheetFileName", ToDBNull(callStatusDetails.JobSheetFileName));
            sp.Add(param);
            param = new SqlParameter("@ProblemObserved", ToDBNull(callStatusDetails.ProblemObserved));
            sp.Add(param);
            param = new SqlParameter("@ServiceCharges", ToDBNull(callStatusDetails.ServiceCharges));
            sp.Add(param);
            param = new SqlParameter("@PartCharges", ToDBNull(callStatusDetails.PartCharges));
            sp.Add(param);
            param = new SqlParameter("@EngName", ToDBNull(callStatusDetails.TechnicianName));
            sp.Add(param);
            param = new SqlParameter("@EngContactNumber", ToDBNull(callStatusDetails.TechnicianContactNumber));
            sp.Add(param);
            param = new SqlParameter("@CancelReason", ToDBNull(callStatusDetails.CancelReason));
            sp.Add(param);
            param = new SqlParameter("@IsServiceApproval", ToDBNull(callStatusDetails.IsServiceApproved));
            sp.Add(param);
            var sql = "UPDATECenterAndUnAssingedCall @DEVICEID,@EMPId,@ProviderId,@CenterId,@USER,@StatusId, @AppointmentDate,@Remarks,@Type,@parts,@InvoiceFile,@JobsheetFileName,@ProblemObserved,@ServiceCharges,@PartCharges,@EngName,@EngContactNumber,@CancelReason,@IsServiceApproval";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 1)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
        public async Task<ResponseModel> EditCallAppointment(CallDetailsModel cam)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@DeviceId", ToDBNull(cam.DeviceId));
            sp.Add(param);            
            param = new SqlParameter("@altcontactnumber", ToDBNull(cam.CustomerAltConNumber));
            sp.Add(param);
            param = new SqlParameter("@appointmentdate", ToDBNull(cam.AppointmentDate));
            sp.Add(param);
            param = new SqlParameter("@remark", ToDBNull(cam.Remarks));
            sp.Add(param);
            param = new SqlParameter("@StatusId", ToDBNull(cam.StatusId));
            sp.Add(param);
            param = new SqlParameter("@UserId", ToDBNull(cam.UserId));
            sp.Add(param);
            var sql = "UpdateAppointmentDetail @DeviceId,@altcontactnumber,@appointmentdate,@remark,@StatusId,@UserId";
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