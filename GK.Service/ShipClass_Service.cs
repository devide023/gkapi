using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GK.Model;
using GK.DAO;
using Dapper;
namespace GK.Service
{
    public class ShipClass_Service
    {
        public ShipClass_Service()
        {

        }

        public IEnumerable<shipclass> Get_List()
        {
            using (GoldKey_DB db = new GoldKey_DB())
            {
                var list = db.Get_Con.Query<shipclass>("select * from shipclass");
                return list;
            }
        }
    }
}
