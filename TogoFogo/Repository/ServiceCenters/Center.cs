
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
using TogoFogo.Filters;
using TogoFogo.Models;
using TogoFogo.Models.Customer_Support;
using TogoFogo.Models.ServiceCenter;

namespace TogoFogo.Repository.ServiceCenters
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
            var param = new SqlParameter("@ProviderId", ToDBNull(filterModel.ProviderId));
            sp.Add(param);
             param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            return await _context.Database.SqlQuery<ServiceCenterModel>("USPGetAllCenters @ProviderId,@CompanyId",sp.ToArray()).ToListAsync();
        }
        public async Task<CallDetailsModel> GetCallsDetailsById(string CRN)
        {

            SqlParameter callDetails = new SqlParameter("@CallId", CRN);
            return await _context.Database.SqlQuery<CallDetailsModel>("GetCallDetailsByCallId @CallId", callDetails).SingleOrDefaultAsync();
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
                    CenterModel.CurrentUserName = CenterModel.UserName;
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

                }
            }

            return CenterModel;

        }

        private List<ContactPersonModel> ReadPersons(DbDataReader reader)
        {
            List<ContactPersonModel> contacts = new List<ContactPersonModel>();

            while (reader.Read())
            {
                var person = new ContactPersonModel
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
                    CityId = Convert.ToInt32(reader["CityId"].ToString()),
                    CountryId = Convert.ToInt32(reader["CountryId"].ToString()),
                    StateId = Convert.ToInt32(reader["StateId"].ToString()),
                    AddressTypeId = Convert.ToInt32(reader["AddressTypeId"].ToString()),
                    Locality = reader["Locality"].ToString(),
                    NearLocation = reader["NearLocation"].ToString(),
                    PinNumber = reader["PinNumber"].ToString(),
                    Address = reader["Address"].ToString(),
                    City = reader["City"].ToString()
                };

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
            string cat = "";
            if (center.Activetab.ToLower() == "tab-1")
            {
                foreach (var item in center.DeviceCategories)
                {
                    cat = cat + "," + item;

                }
                cat = cat.TrimStart(',');
                cat = cat.TrimEnd(',');
            }
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@CENTERID", ToDBNull(center.CenterId));
            sp.Add(param);
            param = new SqlParameter("@PROVIDERID", ToDBNull(center.ProviderId));
            sp.Add(param);
            param = new SqlParameter("@PROCESSID", ToDBNull(center.ProcessId));
            sp.Add(param);
            param = new SqlParameter("@CENTERCODE", ToDBNull(center.CenterCode));
            sp.Add(param);
            param = new SqlParameter("@CENTERNAME", ToDBNull(center.CenterName));
            sp.Add(param);
            param = new SqlParameter("@DEVICECATEGORIES", ToDBNull(cat));
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
            param = new SqlParameter("@SERVICETYPE", ToDBNull(center.ServiceTypes));
            sp.Add(param);
            param = new SqlParameter("@SERVICEDELIVERYTYPE", ToDBNull(center.ServiceDeliveryTypes));
            sp.Add(param);
            param = new SqlParameter("@tab", ToDBNull(center.Activetab));
            sp.Add(param);
            param = new SqlParameter("@ISUSER", ToDBNull(center.IsUser));
            sp.Add(param);
            param = new SqlParameter("@USERNAME", ToDBNull(center.UserName));
            sp.Add(param);
            param = new SqlParameter("@Password", ToDBNull(center.Password));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(center.CompanyId));
            sp.Add(param);
            var sql = "USPInsertUpdateDeleteCenter @CENTERID,@PROVIDERID, @PROCESSID,@CENTERCODE,@CENTERNAME,@DEVICECATEGORIES,@ORGNAME ,@ORGCODE ,@ORGIECNUMBER ,@ORGSTATUTORYTYPE,@ORGAPPLICATIONTAXTYPE," +
                        "@ORGGSTCATEGORY,@ORGGSTNUMBER,@ORGGSTFILEPATH,@ORGPANNUMBER,@ORGPANFILEPATH, @ISACTIVE ,@REMARKS , @ACTION , @USER,@SERVICETYPE" +
                        ",@SERVICEDELIVERYTYPE,@tab,@ISUSER,@USERNAME,@Password,@CompId";


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
                using (var reader = await command.ExecuteReaderAsync())
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
            var sql = "UpdateCallStatus @Status,@AllocateXML,@Reasion,@USER";


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
            SqlParameter param = new SqlParameter("@StatusId", ToDBNull(callStatusDetails.StatusId));
            sp.Add(param);
            param = new SqlParameter("@RejectReasion", ToDBNull(callStatusDetails.RejectionReason));
            sp.Add(param);
            param = new SqlParameter("@DeviceId", ToDBNull(callStatusDetails.DeviceId));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(callStatusDetails.UserId));
            sp.Add(param);
            var sql = "UpdateCallStatusDetails @StatusId,@RejectReasion,@DeviceId,@USER";


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

        public async Task<ResponseModel> SaveTechnicianDetails(CallStatusDetailsModel callStatusDetails)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@EMPId", ToDBNull(callStatusDetails.EmpId));
            sp.Add(param);            
            param = new SqlParameter("@DeviceId", ToDBNull(callStatusDetails.DeviceId));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(callStatusDetails.UserId));
            sp.Add(param);
            var sql = "USPAssignTechinical @EMPId,@DEVICEID,@USER";


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