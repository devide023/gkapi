using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GK.Model;
using GK.Model.Parms;
using GK.DAO;
using Webdiyer.WebControls.Mvc;
namespace GK.Service
{
    public class Cruises_Service
    {
        public Cruises_Service()
        {

        }

        public IEnumerable<cruises> Cruises_List(baseparm parm,out int resultcount)
        {
            using (GoldKey_DB db = new GoldKey_DB())
            {
                resultcount = 0;
                var list = db.Get_Con.Query<cruises>("select * from CRUISES").OrderBy(t => t.code).ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = list.TotalItemCount;
                return list;
            }
        }
    }
}
