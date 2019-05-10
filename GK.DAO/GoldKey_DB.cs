using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
namespace GK.DAO
{
    public class GoldKey_DB:IDisposable
    {
        private string str_conn = string.Empty;
        private SqlConnection conn;
        public GoldKey_DB()
        {
            str_conn = ConfigurationManager.ConnectionStrings["default_conn"] != null ? ConfigurationManager.ConnectionStrings["default_conn"].ToString() : "";
            conn = new SqlConnection(str_conn);
        }
        public GoldKey_DB(string conn_str)
        {
            str_conn = ConfigurationManager.ConnectionStrings[conn_str] != null ? ConfigurationManager.ConnectionStrings[conn_str].ToString() : "";
            conn = new SqlConnection(str_conn);
        }
        public void Dispose()
        {
        }

        public SqlConnection Get_Con
        {
            get
            {
                return conn;
            }
        }
    }
}
