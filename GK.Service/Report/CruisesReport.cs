using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.Model.Parms;
using Dapper;
using GK.DAO;
using System.Data;
namespace GK.Service.Report
{
    public class CruisesReport
    {
        public CruisesReport()
        {

        }
        public IEnumerable<dynamic> CruisesIncome(string ksrq, string jsrq)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select ta.*,tb.name from ");
            sql.Append("(");
            sql.AppendFormat( "select cruisesno,sum(CURR0) as total_amount from MENUBILLH where convert(date,notedate) between '{0}' and '{1}'  group by CRUISESNO ",ksrq,jsrq);
            sql.Append(" ) ta,CRUISES tb");
            sql.Append(" where ta.CRUISESNO = tb.CODE");

            using (GoldKey_DB db = new GoldKey_DB())
            {
               return db.Get_Con.Query(sql.ToString());
            }
        }
    }
}
