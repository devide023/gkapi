using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.DAO;
using GK.Model.Parms;
using Dapper;
using Webdiyer.WebControls.Mvc;

namespace GK.Service
{
    /// <summary>
    /// 消费管理
    /// </summary>
    public class Consume_Service
    {
        public Consume_Service()
        {

        }
        /// <summary>
        /// 获取消费项目
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="resultcount"></param>
        /// <returns></returns>
        public IEnumerable<menucode> Get_Consume_List(consume_parm parm,out int resultcount)
        {
            resultcount = 0;
            using (GoldKey_DB db = new GoldKey_DB())
            {
                StringBuilder sql = new StringBuilder();
                DynamicParameters p = new DynamicParameters();
                sql.Append("select * from menucode where 1=1 ");
                if (!string.IsNullOrEmpty(parm.key))
                {
                    sql.Append(" and menuname like @key ");
                    p.Add("key", "%" + parm.key + "%");
                }
                if (!string.IsNullOrEmpty(parm.cruises_no))
                {
                    sql.Append(" and CRUISESNO = @shipno ");
                    p.Add("shipno", parm.cruises_no);
                }
                if (!string.IsNullOrEmpty(parm.place_no))
                {
                    sql.Append(" and placeno = @placeno ");
                    p.Add("placeno", parm.place_no);
                }
                if (!string.IsNullOrEmpty(parm.xmtype_no))
                {
                    sql.Append(" and xmtypeno = @xmtype_no ");
                    p.Add("xmtype_no", parm.xmtype_no);
                }
                var list = db.Get_Con.Query<menucode>(sql.ToString(), p).OrderBy(t=>t.modifydate).ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = list.TotalItemCount;
                return list;
            }

        }
        /// <summary>
        /// 消费站点
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="resultcount"></param>
        /// <returns></returns>
        public IEnumerable<menuplace> Get_Menuplace_List(menuplace_parm parm, out int resultcount)
        {
            resultcount = 0;
            using (GoldKey_DB db = new GoldKey_DB())
            {
                StringBuilder sql = new StringBuilder();
                DynamicParameters p = new DynamicParameters();
                sql.Append("select * from menuplace where 1=1 ");
                if (!string.IsNullOrEmpty(parm.key))
                {
                    sql.Append(" and placename like @key ");
                    p.Add("key", parm.key);
                }
                if (!string.IsNullOrEmpty(parm.cruises_no))
                {
                    sql.Append(" and cruisesno = @shipno ");
                    p.Add("shipno", parm.cruises_no);
                }
                if (!string.IsNullOrEmpty(parm.dept_no))
                {
                    sql.Append(" and dptno = @deptno ");
                    p.Add("deptno", parm.dept_no);
                }
                var list = db.Get_Con.Query<menuplace>(sql.ToString(), p).OrderBy(t => t.modifydate).ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = list.TotalItemCount;
                return list;
            }
        }

        public IEnumerable<xmtype> Get_Xmtype_List(xmtype_parm parm, out int resultcount)
        {
            resultcount = 0;
            using (GoldKey_DB db = new GoldKey_DB())
            {
                StringBuilder sql = new StringBuilder();
                DynamicParameters p = new DynamicParameters();
                sql.Append("select * from xmtype where 1=1 ");
                if (!string.IsNullOrEmpty(parm.key))
                {
                    sql.Append(" and placename like @key ");
                    p.Add("key", parm.key);
                }
                if (!string.IsNullOrEmpty(parm.cruises_no))
                {
                    sql.Append(" and cruisesno = @shipno ");
                    p.Add("shipno", parm.cruises_no);
                }
                if (!string.IsNullOrEmpty(parm.place_no))
                {
                    sql.Append(" and placeno = @placeno ");
                    p.Add("placeno", parm.place_no);
                }
                var list = db.Get_Con.Query<xmtype>(sql.ToString(), p).OrderBy(t => t.modifydate).ToPagedList(parm.pageindex, parm.pagesize);
                resultcount = list.TotalItemCount;
                return list;
            }
        }
    }
}
