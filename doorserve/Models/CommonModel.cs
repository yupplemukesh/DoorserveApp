using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.Template;

namespace doorserve
{
    public static class CommonModel
    {

        public static string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public static async Task<List<CheckBox>> GetEmployeeList(Guid? RefKey)
        {
            using (var _context = new ApplicationDbContext())
            {
                var param = new SqlParameter("@RefKey", DBNull.Value);

                var query = "select EMPId Name ,p.FirstName+' '+p.LastName Text from MSTEMPLOYEES emp join tblContactPersons p on p.RefKey = emp.EMPId where emp.IsActive = 1";
                if (RefKey != null)
                {
                    query = query + " and emp.RefKey = @RefKey";
                    param.Value = RefKey;

                }
                var _employee = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return _employee;
            }
        }
        public static async Task<List<CheckBox>> GetRegionListByComp(Guid? CompId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var param = new SqlParameter("@CompId", DBNull.Value);

                var query = "Select REGIONID NAME, REGIONNAME Text FROM MSTREGIONS where ISACTIVE=1";
                if (CompId != null)
                {
                    query = query + " and CompanyId=@CompId";
                    param.Value = CompId;

                }

                var _employee = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return _employee;
            }
        }

            public static async Task<List<CheckBox>> GetEmployeeListByCompany(Guid? CompId)
            {
                using (var _context = new ApplicationDbContext())
                {
                    var param = new SqlParameter("@CompId", DBNull.Value);

                    var query = "select EMPId Name ,p.FirstName+' '+p.LastName Text from MSTEMPLOYEES emp join tblContactPersons p on p.RefKey = emp.EMPId where emp.IsActive = 1";
                    if (CompId != null)
                    {
                        query = query + " and emp.companyId= @CompId";
                        param.Value = CompId;

                    }
                    var _employee = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                    return _employee;
                }
            }
            public static async Task<List<CheckBox>> GetEmployeeByProvider(Guid? CompId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var param = new SqlParameter("@providerId", DBNull.Value);

                var query = "select EMPId Name ,p.FirstName+' '+p.LastName Text from MSTEMPLOYEES emp join tblContactPersons p on p.RefKey = emp.EMPId where emp.IsActive = 1";
                if (CompId != null)
                {
                    query = query + " and emp.providerId= @providerId";
                    param.Value = CompId;

                }
                var _employee = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return _employee;
            }
        }
        public static async Task<List<CheckBox>> GetStatusTypes()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _actionTypes = await _context.Database.SqlQuery<CheckBox>("select StatusId value,StatusName text from Status_Master where StatusId in (9,10,11)").ToListAsync();
                return _actionTypes;
            }
        }
        public static async Task<List<CheckBox>> GetStatusTypes(string type)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _actionTypes = await _context.Database.SqlQuery<CheckBox>("select StatusId value,StatusName text from Status_Master where PageRef=@Page",new SqlParameter("@Page",type)).ToListAsync();
                return _actionTypes;
            }
        }
        public static async Task<List<CheckBox>> GetActionTypes()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _actionTypes = await _context.Database.SqlQuery<CheckBox>("GetActionTypeList").ToListAsync();
                return _actionTypes;
            }
        }
        public static List<CheckBox> GetHeaderFooter(Guid? CompId)
        {
            using (var _context = new ApplicationDbContext())
            {

                var param = new SqlParameter("@compId", DBNull.Value);
                var query = "select emailHeaderFooterId value,name text from EmailHeaderFooter where isActive=1";
                if (CompId != null)
                {
                    query = query + " and CompanyId=@compId";
                    param.Value = CompId;    
                }
                else
                    query = query + " and CompanyId is null";
                var _headerTypes =  _context.Database.SqlQuery<CheckBox>(query, param).ToList();
                return _headerTypes;
            }
        }
        public static List<BindGateway> GetMailerGatewayList(Int64 GatewayTypeId,Guid? compId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var sp = new List<SqlParameter>();
                var param = new SqlParameter("@CompId", DBNull.Value);
          
                var query = "select GatewayId, GatewayName from MSTGateway where GatewayTypeId =@gatewayTypeId  AND IsActive=1";
                if (compId != null)
                {
                    param.Value = compId;
                    query = query + " And companyId=@compId";

                }
                else
                    query = query + " And companyId is null";
                sp.Add(param);
                param = new SqlParameter("@gatewayTypeId", GatewayTypeId);
                sp.Add(param);

                var _Gateways =  _context.Database.SqlQuery<BindGateway>(query,sp.ToArray()).ToList();
                return _Gateways;
            }
        }
        public static async Task<List<CheckBox>> GetLookup( string type)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _type = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='"+type+"' and isActive=1").ToListAsync();
                return _type;
            }
        }
        public static async Task<List<CheckBox>> GetSection(int PageId)
        {
            using (var _context = new ApplicationDbContext())
            {

               var param = new SqlParameter("@PageId", DBNull.Value);              
                var query = "select  SectionId Name ,SectionName Text from MSTSECTION where isActive=1 and type='B'";
               if (PageId != null)
                {
                    param.Value = PageId;                    
                    query = query + " and PageId=@PageId";
                }
                var Section = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return Section;
            }
        }
        public static List<CheckBox> GetWildCards(Guid ? compId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var param = new SqlParameter("@compId", DBNull.Value);
                var query = "select WildCardId Value,WildCard Text from WildCard where IsActive=1 ";
                if (compId != null)
                {
                    query = query + " and companyId=@compId";
                    param = new SqlParameter("@compId", compId);
                }
                else                
                    query = query + " and companyId is null"; 
                var _clientData =  _context.Database.SqlQuery<CheckBox>(query, param).ToList();
                return _clientData;
            }
        }
        public static async Task<List<CheckBox>> GetClientData( Guid ?CompId)
        {
            using (var _context = new ApplicationDbContext())
            {
               var param=  new SqlParameter("@companyId", DBNull.Value);
                var query = "select clientId as name,clientName as text from MstClients where isActive=1";
                if (CompId != null)
                {
                    query = query + " and companyId=@companyId";
                    param.Value = CompId;
                }

                var _clientData = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return _clientData;
            }
        }
        public static async Task<List<CheckBox>> GetGatewayType()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _servicetype = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Gateway' and isActive=1").ToListAsync();
                return _servicetype;
            }
        }
        public static async Task<List<CheckBox>> GetServiceType(FilterModel filter)
        {
            using (var _context = new ApplicationDbContext())
            {
                List<SqlParameter> sp = new List<SqlParameter>();
                var param = new SqlParameter("@CompId", DBNull.Value);
                if (filter.CompId != null)
                    param.Value = filter.CompId;
                sp.Add(param);
                param = new SqlParameter("@RefKey", DBNull.Value);
                if (filter.RefKey != null)
                    param.Value = filter.RefKey;
                sp.Add(param);
                var _deliveryType = await _context.Database.SqlQuery<CheckBox>("USPGETSERVICETYPES @CompId,@RefKey", sp.ToArray()).ToListAsync();
                return _deliveryType;
            }
        }
        public static async Task<List<CheckBox>> GetDeliveryServiceType(FilterModel filter)
        {
            using (var _context = new ApplicationDbContext())
            {
                List<SqlParameter> sp = new List<SqlParameter>();
                var param = new SqlParameter("@CompId", DBNull.Value);
                if (filter.CompId != null)
                    param.Value = filter.CompId;
                sp.Add(param);
                param = new SqlParameter("@RefKey", DBNull.Value);              
                if(filter.RefKey !=null)
                    param.Value = filter.RefKey;
                sp.Add(param);
                var _deliveryType = await _context.Database.SqlQuery<CheckBox>("USPGETDELIVERYTYPES @CompId,@RefKey", sp.ToArray()).ToListAsync();
                return _deliveryType;
            }
        }
        public static async Task<List<CheckBox>> GetApplicationTaxType()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _applicationTaxType = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Application Tax Type' and isActive=1").ToListAsync();
                return _applicationTaxType;
            }
        }
        public static  List<CheckBox> GetApplicationTax()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _applicationTaxType =  _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Application Tax Type' and isActive=1").ToList();
                return _applicationTaxType;
            }
        }
        public static async Task<List<CheckBox>> GetStatutoryType()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _statutoryType = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Statutory Type' and isActive=1").ToListAsync();
                return _statutoryType;
            }
        }
        public static async Task<List<CheckBox>> GetGstCategory()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _gstCategory = await _context.Database.SqlQuery<CheckBox>("SELECT GstCategoryId value,GSTCategory text FROM MstGstCategory where IsActive=1").ToListAsync();
                return _gstCategory;
            }
        }
        public static async Task<List<CheckBox>> GetProcesses(Guid? CompId)
        {
            
            using (var _context = new ApplicationDbContext())
            {

                var sql = "SELECT ProcessId value,processName text FROM MSTProcesses where IsActive=1";
                var param = new SqlParameter("@compId", DBNull.Value);
                if (CompId != null)
                {
                    sql += " And companyId=@compId";
                    param.Value = CompId;
                }
                sql += " Order by  processName";
                var _processes = await _context.Database.SqlQuery<CheckBox>(sql, param).ToListAsync();
                return _processes;
            }
        }
        public static  List<CheckBox> GetBanks()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _banksDetails = _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Bank' and isActive=1").ToList();
                return _banksDetails;
            }
        }
        public static  List<CheckBox> GetAddressTypes()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _addressTypes = _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='ADDRESS' and isActive=1").ToList();
                return _addressTypes;
            }
        }
        public static async Task<List<CheckBox>> GetAWBNumberUsedTypes()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _addressTypes =await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='AWB Number Used' and isActive=1").ToListAsync();
                return _addressTypes;
            }
        }
        public static async Task<List<CheckBox>> GetLegalDocumentVerification()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _addressTypes = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Legal Document Verification' and isActive=1").ToListAsync();
                return _addressTypes;
            }
        }
        public static async Task<List<CheckBox>> GetAgreementSignup()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _addressTypes = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Agreement Signup' and isActive=1").ToListAsync();
                return _addressTypes;
            }
        }
        public static async Task<List<CheckBox>> GetReverseAWBStatus()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _reverseAWBStatus = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Reverse AWB Status' and isActive=1").ToListAsync();
                return _reverseAWBStatus;
            }
        }

        public static async Task<CheckBox> GetServiceProviderIdByUserId(int UserId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _reverseAWBStatus = await _context.Database.SqlQuery<CheckBox>("select refkey Name,FirstName text from tblcontactpersons where UserId=@UserId", new SqlParameter("@UserId",UserId)).SingleOrDefaultAsync();
                return _reverseAWBStatus;
            }

        }
        public static async Task<List<CheckBox>> GetServiceEngineerAction()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _reverseAWBStatus = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Service Engineer Action' and isActive=1").ToListAsync();
                return _reverseAWBStatus;
            }
        }
        public static  List<CheckBox> GetActionList()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _ActionList =  _context.Database.SqlQuery<CheckBox>("select ActionId as Value, ActionName Text from MstAction where IsActive = 1 ").ToList();
                return _ActionList;
            }
        }
        public static  List<CheckBox> CTH_NumberList()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _CTH_numberList = _context.Database.SqlQuery<CheckBox>("select distinct  CTH_NUMBER Text from MstSacCodes where IsActive = 1 and  CTH_NUMBER is not null").ToList();
                return _CTH_numberList;
            }
        }
        public static List<CheckBox> SAC_NumberList()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _Sac_numberList = _context.Database.SqlQuery<CheckBox>("select distinct SAC Text from MstSacCodes where IsActive = 1 and SAC is not null").ToList();
                return _Sac_numberList;
            }
        }
        public static async Task<List<CheckBox>> GetServiceProviders(Guid? compId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var param = new SqlParameter("@CompId", DBNull.Value);
                var query = "USPGetServiceProviders @CompId";
                if (compId != null)
                    param.Value = compId;
             
                var _serviceProvider = await _context.Database.SqlQuery<CheckBox>(query,param).ToListAsync();
                return _serviceProvider;
            }
        }
        //
        public static async Task<List<CheckBox>> GetServiceCenters(Guid? providerId)
        {
            using (var _context = new ApplicationDbContext())
            {
                string query = "SELECT CenterId Name ,CenterName Text FROM MSTServiceCenters where IsActive=1";
                SqlParameter param = new SqlParameter();
                if (providerId != null)
                {
                    param.Value = providerId;
                    query = query + " and ProviderId=@ProviderId";
                }
                else
                    param.Value = DBNull.Value;
                param.ParameterName = "@ProviderId";
                var _serviceCeters = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return _serviceCeters;
            }
        }
        public static async Task<List<CheckBox>> GetServiceComp(Guid? compId)
        {
            using (var _context = new ApplicationDbContext())
            {
                string query = "SELECT CenterId Name ,CenterName Text FROM MSTServiceCenters where IsActive=1";
                SqlParameter param = new SqlParameter();
                if (compId != null)
                {
                    param.Value = compId;
                    query = query + " and companyId=@companyId";
                }
                else
                    param.Value = DBNull.Value;
                param.ParameterName = "@companyId";
                var _serviceCeters = await _context.Database.SqlQuery<CheckBox>(query, param).ToListAsync();
                return _serviceCeters;
            }
        }
        public static async Task<List<CheckBox>> GetCompanies()
        {
            using (var _context = new ApplicationDbContext())
            {
                string query = "SELECT Guid Name ,CompanyName Text FROM MSTCompany where IsActive=1 order by CompanyName";
                SqlParameter param = new SqlParameter();
              
                var _companies= await _context.Database.SqlQuery<CheckBox>(query).ToListAsync();
                return _companies;
            }
        }
        public static async Task<List<CheckBox>> GetDepartments()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _departments = await _context.Database.SqlQuery<CheckBox>("select DeptartmentId Value,DeptartmentName Text from MSTDEPARTMENTS where IsActive=1").ToListAsync();
                return _departments;
            }
        }
        public static async Task<List<CheckBox>> GetDesignations()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _desgnations = await _context.Database.SqlQuery<CheckBox>("Select DesignationId Value,DesignationName Text from MSTDesignations where IsActive=1"
                    ).ToListAsync();
                return _desgnations;
            }
        }
    }
    public class CheckBox
    {
        public string Val { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
        public bool IsChecked { get; set; }
        public Guid Name { get; set; }

    }

    public  enum MenuCode
    {        
     System_Admin= 1,
     Manage_Countries= 101,
     Manage_States_UTs= 102,
     Manage_Cities_Locations=103,
     Manage_Device_Category=104,
     Device_Sub_Category=105,
     Manage_Brands=106,
     Manage_Products=108,
     Manage_Device_Problem=109,
     Manage_Problem_Observed=110,
     Manage_Spare_Type=111,
     Manage_Spare_Part_Name=112,
     Manage_Service_Center_TRC=113,
     Manage_Service_Provider=114,
     Color_Master=115,
     Logistics=2,
     Manage_Courier=201,
     Manage_Courier_API=202,
     Manage_Pin_Zip_Code=203,
     Update_Reverse_AWB_Status=204,
     Human_Resourses=3,
     Manage_Engineers=301,
     Purchase_and_Procurement=4,
     Repair_Cost_Estimation=401,
    Spare_Parts_Purchase_List=402,
    Manage_Spare_Parts_Price_and_Stock=403,
    Accounts=5,
    Gst_Category=501,
    GST_HSN_SAC_Codes=502,
    GST_Tax=503,
    Device_Service_Charge=504,
    Confirm_Par_Com_Advance_Payment=505,
    TRC_Service_Center=6,
    Receive_Materials=601,
    Approval_of_Received_Materials=602,
    Pending_Entry_Level_Screening=603,
    Pending_For_Repairing=604,
    Pending_for_Quality_Check=605,
    Pending_For_Packing=606,
    Pending_for_Billing_Invoicing=607,
    Reprint_Invoice_Bill=608,
    Customer_Support=7,
    Pending_Call_Request_Confirmation=701,
    Pending_OOW_Repair_Requests=702,
    Pending_IW_Repair_Requests=703,
    Pending_Repair_Cost_Confirmation=704,
    Receive_Par_Com_Advance_Payment=705,
    Users=8,
    Manage_User_Permission=801,
    Manage_User_Roles=802,
    Manage_Users=803,
    Update_AWB_Status=205,
    Reverse_AWB_Allocation=206,
    Reports=9,
    Repair_Request_Report_Repair=901,
    Update_Reverse_AWB_Status_Biker=207,
    Update_Repair_Status_FE=209,
    Call_Request_Job_Number_Status=706,
    Website_Settings=10,
    EMail_Gateway_Settings=1001,
    EMail_Header_and_Footer_Template=1002,
    EMail_SMS_Notification_IVR_Template=1003,
    SMS_Gateway_Settings=1004,
    Promocode=1005,
    Landing_Report=902,
    Website_User=903,
    Spare_Problem_Price_matrix=116,
    Manage_Clients=117,
    Manage_Action_Types=118,
    Wild_Cards=1006,
    Notification_Gateway=1007,
    Assign_Calls=11,
    Call_Allocate_To_ASP=707,
    Service_Provider=12,
    Template_Part=1008,
    Manage_Menues=1009,
    Manage_company=119,
    Open_Calls=609,
    Schedule_Appointment=13,
    Manage_Process=120,
    Manage_Page_Contents=1011,
    Manage_Regions=121,
    Esclated_Calls = 14

    }
}