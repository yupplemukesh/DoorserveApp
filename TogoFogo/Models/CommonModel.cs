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
                var _gstCategory = await _context.Database.SqlQuery<CheckBox>("SELECT GstCategoryId value,GSTCategory text FROM MstGstCategory where IsActive='YES'").ToListAsync();
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

      
    }


    public class CheckBox
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public bool IsChecked { get; set; }
        public Guid Name { get; set; }

    }
}