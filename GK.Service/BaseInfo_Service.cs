using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.DAO;
using Webdiyer.WebControls.Mvc;
using Dapper;
using GK.Model.public_db;
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

        public IEnumerable<sys_menutype> Get_MenuType()
        {
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_menutype>("select * from sys_menutype where status=1 order by id asc");
            }
        }

    }
}
