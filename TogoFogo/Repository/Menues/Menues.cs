using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository.Menues
{
    public class Menues:IMenues
    {
        private readonly ApplicationDbContext _context;
        public Menues()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<MenuMasterModel>> GetMenues()
        {
       
            SqlParameter param = new SqlParameter("@MenuId", DBNull.Value);
            var menues= await _context.Database.SqlQuery<MenuMasterModel>("GETMENULIST @MenuId", param).ToListAsync();
            return GetChieldMenu(menues);
        }

        public async Task<MenuMasterModel> GetMenuById(string menuID)
        {
            SqlParameter param = new SqlParameter("@MenuId", menuID);
            return await _context.Database.SqlQuery<MenuMasterModel>("GETMENULIST @MenuId", param).SingleOrDefaultAsync();
        }
        private List<MenuMasterModel> GetChieldMenu(List<MenuMasterModel> menues)
        {
            var menus = new List<MenuMasterModel>();
            var perentMenues =  menues.Where(x => x.ParentMenuId == 0).ToList();
            
            foreach (var item in perentMenues)
            {
                string path = "/UploadedImages/icon-img/";
                item.IconFileNameUl = path + item.IconFileName;
                var items = menues.Where(x => x.ParentMenuId == item.MenuCapId).Select(x=>new MenuMasterModel { MenuCapId= x.MenuCapId, Menu_Name=x.Menu_Name, ParentMenuId= x.ParentMenuId, IsActive=x.IsActive, CapName=x.CapName, PagePath=x.PagePath, IconFileName=x.IconFileName, IconFileNameUl= path + x.IconFileName  }).ToList();
                item.SubMenuList = items;
                menus.Add(item);                 
            }
            return menus;

        }
        public async Task<ResponseModel> AddUpdateMenu(MenuMasterModel menu,char action)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@MenuId", ToDBNull(menu.MenuCapId));
            sp.Add(param);
            param = new SqlParameter("@Menu_name", ToDBNull(menu.Menu_Name));
            sp.Add(param);
            param = new SqlParameter("@CapName", ToDBNull(menu.CapName));
            sp.Add(param);
            param = new SqlParameter("@IconFileName", ToDBNull(menu.IconFileName));
            sp.Add(param);
            param = new SqlParameter("@PerentMenuId", ToDBNull(menu.ParentMenuId));
            sp.Add(param);
            param = new SqlParameter("@order", ToDBNull(menu.shortOrder));
            sp.Add(param);
            param = new SqlParameter("@IsActive", ToDBNull(menu.IsActive));
            sp.Add(param);
            param = new SqlParameter("@user", ToDBNull(menu.CreatedBy));
            sp.Add(param);
            param = new SqlParameter("@action", ToDBNull(action));
            sp.Add(param);
            param = new SqlParameter("@ServiceTypeIds", ToDBNull(menu.ServiceTypeIds));
            sp.Add(param);
            var sql = "Add_Modify_Menu @MenuId,@Menu_name,@CapName,@IconFileName,@PerentMenuId,@order,@IsActive,@user,@action,@ServiceTypeIds";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
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