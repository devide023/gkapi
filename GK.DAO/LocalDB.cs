using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace GK.DAO
{
    public class LocalDB : IDAO,IDisposable
    {
        private string str_conn = string.Empty;
        private SqlConnection conn;

        public LocalDB()
        {
            string config_str = "local_conn";
            str_conn = ConfigurationManager.ConnectionStrings[config_str] != null ? ConfigurationManager.ConnectionStrings[config_str].ToString() : "";
            conn = new SqlConnection(str_conn);
        }

        public LocalDB(string config_str)
        {
            str_conn = ConfigurationManager.ConnectionStrings[config_str] != null ? ConfigurationManager.ConnectionStrings[config_str].ToString() : "";
            conn = new SqlConnection(str_conn);
        }


        public SqlConnection Current_Conn
        {
            get
            {
                return conn;
            }
        }

        public SqlConnection Conn(string config_str)
        {
            str_conn = ConfigurationManager.ConnectionStrings[config_str] != null ? ConfigurationManager.ConnectionStrings[config_str].ToString() : "";
            conn = new SqlConnection(str_conn);
            return conn;
        }

        public void Dispose()
        {
            
        }
    }
}
