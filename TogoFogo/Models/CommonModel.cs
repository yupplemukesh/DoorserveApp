using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TogoFogo.Models;

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
                var _serviceProviders = await _context.Database.SqlQuery<CheckBox>("SELECT Id Value,ProviderName Text FROM MstServiceProvider where IsActive=1").ToListAsync();
                return _serviceProviders;
            }
        }

        public static async Task<List<CheckBox>> GetServiceCenters()
        {
            using (var _context = new ApplicationDbContext())
            {
                var _serviceCeters = await _context.Database.SqlQuery<CheckBox>("SELECT TRC_ID Value,TRC_NAME Text FROM msttrc where IsActive=1").ToListAsync();
                return _serviceCeters;
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