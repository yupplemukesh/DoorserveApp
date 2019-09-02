using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace doorserve.Repository.ManageRegion
{
    public class Region:IRegion  
    {
        private readonly ApplicationDbContext _Context;
        public Region()
        {
            _Context = new ApplicationDbContext();
        }

        public async Task<List<ManageRegionModel>> GetAllRegion(FilterModel filterModel)
        {
            var sp = new List<SqlParameter>();
            var param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            return await _Context.Database.SqlQuery<ManageRegionModel>("USPGetAllRegion @CompanyId", sp.ToArray()
                ).ToListAsync();
        }

        public async Task<ManageRegionModel> GetRegionById(Guid RegionId)
        {
            var param = new SqlParameter("@REGIONID", RegionId);
            return await _Context.Database.SqlQuery<ManageRegionModel>("USPGetRegionById @REGIONID", param).SingleOrDefaultAsync();
        }

        public async Task<ResponseModel> AddUpdateRegion(ManageRegionModel Region)
        {
            var statesList = new List<StateModel>();
            if (Region.SelectedStates != null)
            {
                for (int i = 0; i < Region.SelectedStates.Count; i++)
                {
                    var state = new StateModel { St_ID = Region.SelectedStates[i].ToString() };
                    statesList.Add(state);

                }
            }
            string xml = "";
            if (Region.SelectedStates != null)
            {

                XmlSerializer StateDetails = new XmlSerializer(statesList.GetType());

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        StateDetails.Serialize(writer, statesList);
                        xml = sww.ToString();
                    }
                }
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");                
            }

            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@REGIONID", ToDBNull(Region.RegionId));
            sp.Add(param);
            param = new SqlParameter("@REGIONNAME", ToDBNull(Region.RegionName));
            sp.Add(param);
             
            param = new SqlParameter("@REMARKS", ToDBNull(Region.Remarks));
            sp.Add(param);
      
            param = new SqlParameter("@User", ToDBNull(Region.UserId));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(Region.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@STATEXML", ToDBNull(xml));
            sp.Add(param);
            param = new SqlParameter("@Action", (object)Region.EventAction);
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)(Region.IsActive));
            sp.Add(param);
            var sql = "ADDOREDITREGION @REGIONID,@REGIONNAME,@REMARKS,@User,@CompId, @STATEXML,@Action,@IsActive";
            var res = await _Context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
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
           _Context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
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