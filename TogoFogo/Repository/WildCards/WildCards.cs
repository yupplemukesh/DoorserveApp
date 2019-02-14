﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;

namespace TogoFogo.Repository.WildCards
{
    public class WildCards:IWildCards
    {
        private readonly ApplicationDbContext _context;
        public WildCards()
        {
            _context = new ApplicationDbContext();

        }
        public async Task<List<WildCardModel>> GetWildCards()
        {
            return await _context.Database.SqlQuery<WildCardModel>("USPGetAllWildCards").ToListAsync();
        }

        public async Task<WildCardModel> GetActionByWildCardId(int WildCardId)
        {
            SqlParameter wildCard = new SqlParameter("@WildCardId", WildCardId);
            return await _context.Database.SqlQuery<WildCardModel>("USPGetActionByWildCardId @WildCardId", wildCard).SingleOrDefaultAsync();
        }

        public async Task<bool> AddUpdateDeleteWildCards(WildCardModel wildCardModel, char action)
        {
            bool result = false;
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@WildCardId", wildCardModel.WildCardId);
            sp.Add(param);
            param = new SqlParameter("@WildCard", (object)wildCardModel.WildCard);
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)wildCardModel.IsActive);
            sp.Add(param);
            param = new SqlParameter("@ACTION", (object)action);
            sp.Add(param);
            param = new SqlParameter("@USER", (object)wildCardModel.AddedBy);
            sp.Add(param);
            var sql = "USPInsertUpdateDeleteWildCards @WildCardId,@WildCard,@IsActive,@ACTION,@USER";


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