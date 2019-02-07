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
        private readonly static ApplicationDbContext _context= new ApplicationDbContext();
    
        
        public static async Task<List<CheckBox>> GetServiceType()
        {
            var _servicetype =  await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Service Type' and isActive=1").ToListAsync();          
            return _servicetype;
        }



        public static async Task<List<CheckBox>> GetDeliveryServiceType()
        {

            var _deliveryType = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Delivery Type' and isActive=1").ToListAsync();
            return _deliveryType;          
        }

        public static async Task<List<CheckBox>> GetApplicationTaxType()
        {

            var _applicationTaxType = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Application Tax Type' and isActive=1").ToListAsync();
            return _applicationTaxType;
        }

        public static async Task<List<CheckBox>> GetStatutoryType()
        {

            var _statutoryType = await _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Statutory Type' and isActive=1").ToListAsync();
            return _statutoryType;
        }

        public static async Task<List<CheckBox>> GetGstCategory()
        {

            var _gstCategory = await _context.Database.SqlQuery<CheckBox>("SELECT GstCategoryId value,GSTCategory text FROM MstGstCategory where IsActive='YES'").ToListAsync();
            return _gstCategory;
        }
        public static async Task<List<CheckBox>> GetProcesses()
        {

            var _processes = await _context.Database.SqlQuery<CheckBox>("SELECT ProcessId value,processName text FROM tblProcesses where IsActive=1").ToListAsync();
            return _processes;
        }
        public static  List<CheckBox> GetBanks()
        {
            var _banksDetails =  _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='Bank' and isActive=1").ToList();
            return _banksDetails;
        }

        public static  List<CheckBox> GetAddressTypes()
        {
            var _addressTypes =  _context.Database.SqlQuery<CheckBox>("select id value,name text from tblCommon where Type='ADDRESS' and isActive=1").ToList();
            return _addressTypes;
        }

      
    }


    public class CheckBox
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public bool IsChecked { get; set; }

    }
}