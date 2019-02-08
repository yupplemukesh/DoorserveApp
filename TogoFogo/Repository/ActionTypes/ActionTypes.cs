using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class ActionTypes: IActionTypes
    {
        private readonly ApplicationDbContext _context;
        public ActionTypes()
        {
            _context = new ApplicationDbContext();

        }
        public async Task<List<ActionTypeModel>> GetActiontypes()
        {
            return await _context.Database.SqlQuery<ActionTypeModel>("USPGetAllActionTypes").ToListAsync();
        }

        public async Task<ActionTypeModel> GetActionByActionId(int ActionTypeId)
        {
            SqlParameter actionTypes = new SqlParameter("@ActionTypeId", ActionTypeId);
            return await _context.Database.SqlQuery<ActionTypeModel>("USPGetActionByActionId @ActionTypeId", actionTypes).SingleOrDefaultAsync();
        }
        public async Task<bool> AddUpdateDeleteActionTypes(ActionTypeModel actionTypeModel, char action)
        {
            bool result = false;
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@ACTIONTYPEID", actionTypeModel.ActionTypeId);
            sp.Add(param);
            param = new SqlParameter("@Name", (object)actionTypeModel.Name);
            sp.Add(param);        
            param = new SqlParameter("@ISACTIVE", (object)actionTypeModel.IsActive);
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)actionTypeModel.AddeddBy);
            sp.Add(param);
            param = new SqlParameter("@Comments", (object)actionTypeModel.Comments);
            sp.Add(param);


            var sql = "USPInsertUpdateDeleteActionType @ACTIONTYPEID,@Name,@ISACTIVE,@ACTION,@USER,@Comments";


            var res = await _context.Database.SqlQuery<string>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res == "Ok")
                result = true;

            return result;
        }

        public static object ToDBNull(object value)
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