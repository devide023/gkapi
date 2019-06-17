using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GK.DAO
{
    public interface IDAO
    {
        SqlConnection Conn(string config_str);
        SqlConnection Current_Conn { get; }
    }
}
