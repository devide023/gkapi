using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.DAO;
using Webdiyer.WebControls.Mvc;
using Dapper;
namespace GK.Service
{
    public class BaseInfo_Service
    {
        public BaseInfo_Service()
        {

        }

        public IEnumerable<cruises> Get_Cruises()
        {
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query<cruises>("select * from cruises order by code");
            }
        }

    }
}
