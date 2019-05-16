using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GK.Model;
using GK.Model.Parms;
using GK.DAO;
using Dapper;
using Webdiyer.WebControls.Mvc;
namespace GK.Service
{
    public class ShipClass_Service
    {
        public ShipClass_Service()
        {

        }

        public IEnumerable<shipclass> Get_List(shipclass_parm parms,out int resultcount)
        {
            using (GoldKey_DB db = new GoldKey_DB())
            {
                resultcount = 0;
                DynamicParameters q = new DynamicParameters();
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from shipclass where 1=1");
                if (!string.IsNullOrEmpty(parms.key))
                {
                    sql.Append(" and rcno like @rcno ");
                    q.Add("rcno", '%'+parms.key+'%');
                }
                if (!string.IsNullOrEmpty(parms.ksrq) && !string.IsNullOrEmpty(parms.jsrq))
                {
                    sql.Append(" and convert(date,bdate) between @ksrq and @jsrq ");
                    q.Add("ksrq", parms.ksrq);
                    q.Add("jsrq", parms.jsrq);
                }
                var list = db.Get_Con.Query<shipclass>(sql.ToString(),q).OrderByDescending(t=>t.modifydate).ToPagedList(parms.pageindex,parms.pagesize);
                resultcount = list.TotalItemCount;
                return list;
            }
        }
    }
}
