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

namespace TogoFogo.Repository.ManageBanners
{
    public class Banner:IBanner
    {
        private readonly ApplicationDbContext _context;
        public Banner()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<List<ManageBannersModel>> GetBanner(FilterModel filterModel)
        {

            List<SqlParameter> sp = new List<SqlParameter>();
            var param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            param = new SqlParameter("@BannerId", ToDBNull(filterModel.RefKey));
            sp.Add(param);
            return await _context.Database.SqlQuery<ManageBannersModel>("USPGetAllBanners @CompanyId,@BannerId", sp.ToArray()).ToListAsync();
        }


        public async Task<ManageBannersModel> GetBannerById(Guid BannerId)
        {          

            var Banner = new ManageBannersModel();      
         
            SqlParameter Ban = new SqlParameter("@BannerId", ToDBNull(BannerId));            
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPGetBannersById";
                command.CommandType = CommandType.StoredProcedure;               
                command.Parameters.Add(Ban);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    Banner =
                       ((IObjectContextAdapter)_context)
                           .ObjectContext
                           .Translate<ManageBannersModel>(reader)
                           .SingleOrDefault();                    
                    reader.NextResult();


                    Banner.ImgDetails = ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<ManageBannerUploadModel>(reader)
                            .ToList();
                }
            }

            return Banner;
        }

        public async Task<ResponseModel> AddUpdateBanner(ManageBannersModel Banner)
        {
            string xml = "";
            if (Banner.ImgDetails != null)
            {
                XmlSerializer ImgDetails = new XmlSerializer(Banner.ImgDetails.GetType());

                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        ImgDetails.Serialize(writer, Banner.ImgDetails);
                        xml = sww.ToString();
                    }
                }
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
            }
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@BannerId", ToDBNull(Banner.BannerId));
            sp.Add(param);
            param = new SqlParameter("@CompId", ToDBNull(Banner.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@Title", ToDBNull(Banner.Title));
            sp.Add(param);           
            param = new SqlParameter("@Name", ToDBNull(Banner.Name));
            sp.Add(param);
            param = new SqlParameter("@Extra1", ToDBNull(Banner.Extra1));
            sp.Add(param);
            param = new SqlParameter("@Extra2", ToDBNull(Banner.Extra2));
            sp.Add(param);
            param = new SqlParameter("@PageId", ToDBNull(Banner.PageId));
            sp.Add(param);
            param = new SqlParameter("@SectionId", ToDBNull(Banner.SectionId));
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)(Banner.IsActive));
            sp.Add(param);           
            param = new SqlParameter("@User", ToDBNull(Banner.UserId));
            sp.Add(param);
            param = new SqlParameter("@ImgUpload", ToDBNull(xml));
            param.SqlDbType = SqlDbType.Xml;
            sp.Add(param);
            param = new SqlParameter("@Action", (object)Banner.EventAction);
            sp.Add(param);
            var sql = "USPInsertUpdateBanners @BannerId,@CompId,@Title,@Name,@Extra1,@Extra2,@PageId,@SectionId,@IsActive,@User,@ImgUpload,@Action";
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