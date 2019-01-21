using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TogoFogo.Extension
{
    public class CheckUrl
    {
        public static bool IsValidUser(string username, string url)
        {
            bool _retVal = false;
            string userId = "";
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                
                userId = con.Query<string>("select Id from create_user_Master where UserName=@UserName", new { UserName = username }, commandType: System.Data.CommandType.Text).FirstOrDefault();
                var result = con.Query<string>("select PagePath from menuTable where MenuCap_ID IN(SELECT MenuID from user_rights_Test where UserId=@UserId)", new { UserID = userId }, commandType: System.Data.CommandType.Text).ToList();
                if(result!=null)
                {
                    _retVal= result.Contains(url);
                }
            }
            return _retVal;
        }
    }
}