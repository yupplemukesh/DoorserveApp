﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;

namespace TogoFogo.Repository.SMSGateway
{
    public class Gateway:IGateway
    {
        private readonly ApplicationDbContext _context;
        public Gateway()
        {
            _context = new ApplicationDbContext();

        }

        public async Task<List<GatewayModel>> GetGatewayByType(int GatewayTypeId)
        {

            List<SqlParameter> sp = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@GatewayId",DBNull.Value);
            sp.Add(param);
            param = new SqlParameter("@GatewayTypeId", GatewayTypeId);
            sp.Add(param);
            return await _context.Database.SqlQuery<GatewayModel>("USPGatewayDetail @GatewayId,@GatewayTypeId", sp.ToArray()).ToListAsync();
        }
        public async Task<GatewayModel> GetGatewayById(int GatewayId)
        {
            List<SqlParameter> sp = new List<SqlParameter>();

            SqlParameter param = new SqlParameter("@GatewayId", GatewayId);
            sp.Add(param);
            param = new SqlParameter("@GatewayTypeId", DBNull.Value);
            sp.Add(param);
            return await _context.Database.SqlQuery<GatewayModel>("USPGatewayDetail @GatewayId,@GatewayTypeId", sp.ToArray()).SingleOrDefaultAsync();
        }
        public async Task<ResponseModel> AddUpdateDeleteGateway(GatewayModel gatewayModel, char action)
        {
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@GatewayId", gatewayModel.GatewayId);
            sp.Add(param);
            param = new SqlParameter("@GatewayTypeId", (object)gatewayModel.GatewayTypeId);
            sp.Add(param);
            param = new SqlParameter("@GatewayName", (object)gatewayModel.GatewayName);
            sp.Add(param);
            param = new SqlParameter("@ISACTIVE", (object)gatewayModel.IsActive);
            sp.Add(param);
            //param = new SqlParameter("@ACTION", (object)action);
            //sp.Add(param);
            //param = new SqlParameter("@USER", (object)smsGatewayModel.AddeddBy);
            //sp.Add(param);
            param = new SqlParameter("@IsDefault", (object)gatewayModel.IsDefault);
            sp.Add(param);
            param = new SqlParameter("@IsProcessByAWS", (object)gatewayModel.IsProcessByAWS);
            sp.Add(param);
            param = new SqlParameter("@Name", ToDBNull(gatewayModel.Name));
            sp.Add(param);
            param = new SqlParameter("@Email", ToDBNull(gatewayModel.Email));
            sp.Add(param);
            param = new SqlParameter("@SmtpServerName", ToDBNull(gatewayModel.SmtpServerName));
            sp.Add(param);
            param = new SqlParameter("@SmtpUserName", ToDBNull(gatewayModel.SmtpUserName));
            sp.Add(param);
            param = new SqlParameter("@SmtpPassword", ToDBNull(gatewayModel.SmtpPassword));
            sp.Add(param);
            param = new SqlParameter("@PortNumber", ToDBNull(gatewayModel.PortNumber));
            sp.Add(param);
            param = new SqlParameter("@SSLEnabled", ToDBNull(gatewayModel.SSLEnabled));
            sp.Add(param);
            param = new SqlParameter("@URL", ToDBNull(gatewayModel.URL));
            sp.Add(param);
            param = new SqlParameter("@TransApikey",ToDBNull(gatewayModel.TransApikey));
            sp.Add(param);
            param = new SqlParameter("@OTPApikey", ToDBNull(gatewayModel.OTPApikey));
            sp.Add(param);
            param = new SqlParameter("@SuccessMessage", ToDBNull(gatewayModel.SuccessMessage));
            sp.Add(param);
            param = new SqlParameter("@OtpSender", ToDBNull(gatewayModel.OTPSender));
            sp.Add(param);
            
            
            param = new SqlParameter("@USERID", (object)gatewayModel.AddeddBy);
            sp.Add(param);
            
            
           
            
            //param = new SqlParameter("@GatewayList", ToDBNull(smsGatewayModel.GatewayList));
            //sp.Add(param);
           var sql = "USPGatewayADDUPDATE @GatewayId,@GatewayTypeId,@GatewayName,@ISACTIVE,@IsDefault,@IsProcessByAWS,@Name,@Email,@SmtpServerName,@SmtpUserName,@SmtpPassword,@PortNumber,@SSLEnabled,@URL,@TransApikey,@OTPApikey,@SuccessMessage,@OtpSender,@USERID";


            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).FirstOrDefaultAsync();
            if (res.ResponseCode==0)
                res.IsSuccess = true;

            return res;
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