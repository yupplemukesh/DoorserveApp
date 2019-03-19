using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;
using TogoFogo.Models.Template;

namespace TogoFogo
{
    public static class CommonModel
    {

        public static async Task<List<CheckBox>> GetActionTypes()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _actionTypes = await _context.Database.SqlQuery<CheckBox>("select actionTypeID value,name text from ActionType where isActive=1").ToListAsync();
                return _actionTypes;
            }
        }
        public static async Task<List<CheckBox>> GetHeaderFooter()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _headerTypes = await _context.Database.SqlQuery<CheckBox>("select emailHeaderFooterId value,name text from EmailHeaderFooter where isActive=1").ToListAsync();
                return _headerTypes;
            }
        }
        public static async Task<List<BindGateway>> GetMailerGatewayList(Int64 GatewayTypeId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _Gateways = await _context.Database.SqlQuery<BindGateway>("select GatewayId, GatewayName from MSTGateway where GatewayTypeId = " + GatewayTypeId + " AND IsActive=1").ToListAsync();
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
        public static async Task<List<CheckBox>> GetClientData()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _clientData = await _context.Database.SqlQuery<CheckBox>("select clientId as name,clientName as text from MstClients where isActive=1").ToListAsync();
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
        public static async Task<List<CheckBox>> GetServiceType()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _servicetype = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Service Type' and isActive=1").ToListAsync();
                return _servicetype;
            }
        }
        public static async Task<List<CheckBox>> GetDeliveryServiceType()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _deliveryType = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Delivery Type' and isActive=1").ToListAsync();
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
        public static async Task<List<CheckBox>> GetProcesses()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _processes = await _context.Database.SqlQuery<CheckBox>("SELECT ProcessId value,processName text FROM MSTProcesses where IsActive=1").ToListAsync();
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
        public static async Task<List<CheckBox>> GetServiceEngineerAction()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _reverseAWBStatus = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Service Engineer Action' and isActive=1").ToListAsync();
                return _reverseAWBStatus;
            }
        }
        public static async Task<List<CheckBox>> GetActionList()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _ActionList = await _context.Database.SqlQuery<CheckBox>("select ActionId as Value, ActionName Text from MstAction where IsActive = 1 ").ToListAsync();
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
        public static async Task<List<CheckBox>> GetServiceProviders()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _serviceProvider = await _context.Database.SqlQuery<CheckBox>("SELECT ProviderId Name,ProviderName Text FROM MstServiceProviders where IsActive=1").ToListAsync();
                return _serviceProvider;
            }
        }
        public static async Task<List<CheckBox>> GetServiceCenters(Guid? providerId)
        {
            using (var _context = new ApplicationDbContext())
            {
                string query = "SELECT CenterId Name ,CenterName Text FROM MSTServiceCenters where IsActive=1";
                SqlParameter param =new SqlParameter();
                if (providerId != null)
                {
                    query = query + " and ProviderId=@ProviderId";
                    param.ParameterName= "@ProviderId";
                    param.Value = providerId;
                }
                var _serviceCeters = await _context.Database.SqlQuery<CheckBox>(query,param).ToListAsync();
                return _serviceCeters;
            }
        }

        public static async Task<Guid> GetProviderIdByUser(int userId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _serviceProvider = await _context.Database.SqlQuery<Guid>("SELECT ProviderId FROM MstServiceProviders where IsActive=1 and Userid=@userId",
                    new SqlParameter("userId",userId)).SingleOrDefaultAsync();
                return _serviceProvider;
            }
        }

        public static async Task<Guid> GetCenterIdByUser(int userId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _centerId = await _context.Database.SqlQuery<Guid>("SELECT CenterId FROM MSTServiceCenters where IsActive=1 and Userid=@userId",
                    new SqlParameter("userId", userId)).SingleOrDefaultAsync();
                return _centerId;
            }
        }
        public static async Task<Guid> GetClientIdByUser(int userId)
        {
            using (var _context = new ApplicationDbContext())
            {
                var _clientId = await _context.Database.SqlQuery<Guid>("select ClientId  from MstClients where USERID=@userId and isActive=1",
                    new SqlParameter("userId", userId)).SingleOrDefaultAsync();
                return _clientId;
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
        public string Text { get; set; }
        public int Value { get; set; }
        public bool IsChecked { get; set; }
        public Guid Name { get; set; }

    }
}