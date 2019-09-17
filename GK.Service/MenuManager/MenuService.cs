using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model.public_db;
using GK.Model.Parms.Menu;
using Dapper;
using GK.DAO;
using Webdiyer.WebControls.Mvc;
using CommonServiceLocator;
using SolrNet;
namespace GK.Service.MenuManager
{
    public class MenuService : IService<sys_menu>
    {
        private ISolrOperations<sys_menu> solr = null;
        public MenuService()
        {
            solr = ServiceLocator.Current.GetInstance<ISolrOperations<sys_menu>>();
        }
        public int Add(sys_menu entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO dbo.sys_menu \n");
                sql.Append("        ( status , \n");
                sql.Append("          pid , \n");
                sql.Append("          title , \n");
                sql.Append("          code , \n");
                sql.Append("          menucode , \n");
                sql.Append("          icon , \n");
                sql.Append("          path , \n");
                sql.Append("          menutype , \n");
                sql.Append("          seq ,viewpath, \n");
                sql.Append("          add_time \n");
                sql.Append("        ) \n");
                sql.Append("select @status , -- status - int \n");
                sql.Append("          @pid , -- pid - int \n");
                sql.Append("          @title , -- title - nvarchar(100) \n");
                sql.Append("          @code , -- code - nvarchar(50) \n");
                sql.Append("          @menucode , -- code - nvarchar(50) \n");
                sql.Append("          @icon , -- icon - nvarchar(50) \n");
                sql.Append("          @path , -- path - nvarchar(50) \n");
                sql.Append("          @menutype , -- menutype - nvarchar(50) \n");
                sql.Append("          @seq ,@viewpath, -- seq - nvarchar(50) \n");
                sql.Append("          GETDATE()  -- add_time - datetime \n");
                sql.Append("where NOT EXISTS(select * from sys_menu where code=@code)\n select SCOPE_IDENTITY();\n");
                int menuid = db.Current_Conn.ExecuteScalar<int>(sql.ToString(), entry);
                entry.id = menuid;
                entry.entitytype = "sys_menu";
                if (menuid > 0)
                {
                    solr.Add(entry);
                    solr.Commit();
                }
                return menuid;
            }
        }

        public int Del(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("WITH cte_name \n");
                sql.Append("     AS (SELECT id, \n");
                sql.Append("                pid, \n");
                sql.Append("                title \n");
                sql.Append("         FROM   dbo.sys_menu \n");
                sql.Append("         WHERE  id = @id \n");
                sql.Append("         UNION ALL \n");
                sql.Append("         SELECT ta.id, \n");
                sql.Append("                ta.pid, \n");
                sql.Append("                ta.title \n");
                sql.Append("         FROM   dbo.sys_menu ta \n");
                sql.Append("                JOIN cte_name tb \n");
                sql.Append("                  ON ta.pid = tb.id) \n");
                sql.Append("SELECT id \n");
                sql.Append("FROM   cte_name");

                IEnumerable<int> delids = db.Current_Conn.Query<int>(sql.ToString(), new { id = id });
                StringBuilder sql1 = new StringBuilder();
                sql1.Append("begin tran \n");
                sql1.Append("delete from sys_menu where id in @ids\n");
                sql1.Append("delete from sys_role_menu where menu_id in @ids\n");
                sql1.Append("commit \n");
                int cnt = db.Current_Conn.Execute(sql1.ToString(), new { ids = delids });
                if (cnt > 0)
                {
                    List<ISolrQuery> qs = new List<ISolrQuery>();
                    foreach (var delid in delids)
                    {
                        qs.Add(new SolrQuery("entitytype:sys_menu") && new SolrQuery("id: " + delid.ToString()));
                    }
                    solr.Delete(new SolrMultipleCriteriaQuery(qs,"OR"));
                    solr.Commit();
                }
                return cnt;
            }
        }

        public int Del(List<int> ids)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("DELETE FROM dbo.sys_menu WHERE id in @ids");
                int cnt = db.Current_Conn.Execute(sql.ToString(), new { ids = ids });
                if (cnt > 0)
                {
                    List<ISolrQuery> qs = new List<ISolrQuery>();
                    foreach (var id in ids)
                    {
                        qs.Add(new SolrQueryByField("id", id.ToString()));
                    }
                    solr.Delete(new SolrMultipleCriteriaQuery(qs, "OR"));
                    solr.Commit();
                }
                return cnt;
            }
        }

        public sys_menu Find(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * FROM dbo.sys_menu WHERE id=@id");
                return db.Current_Conn.Query<sys_menu>(sql.ToString(), new { id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<sys_menu> List()
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * FROM dbo.sys_menu ");
                return db.Current_Conn.Query<sys_menu>(sql.ToString());
            }
        }
        public IEnumerable<sys_menu> List(int pid)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * FROM dbo.sys_menu where pid=@pid");
                return db.Current_Conn.Query<sys_menu>(sql.ToString(), new { pid = pid });
            }
        }

        public IEnumerable<sys_menu> List(menuparm parm, out int recordcount)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                DynamicParameters p = new DynamicParameters();
                sql.Append("SELECT * FROM dbo.sys_menu WHERE 1=1 \n");
                if (!string.IsNullOrEmpty(parm.pid))
                {
                    sql.AppendFormat(" and pid = @pid ");
                    p.Add("pid", parm.pid);
                }
                if (!string.IsNullOrEmpty(parm.key))
                {
                    sql.AppendFormat(" and title like @title ");
                    p.Add("title", "%" + parm.key + "%");
                }
                if (parm.menutype > 0)
                {
                    sql.Append(" and menutype=@menutype \n");
                    p.Add("menutype", parm.menutype);
                }
                if (!string.IsNullOrEmpty(parm.url))
                {
                    sql.Append(" and path=@url \n");
                    p.Add("url", parm.url);
                }
                var list = db.Current_Conn.Query<sys_menu>(sql.ToString(), p).OrderByDescending(t => t.id).ToPagedList(parm.pageindex, parm.pagesize);
                recordcount = list.TotalItemCount;
                return list;
            }
        }

        public int Update(sys_menu entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE dbo.sys_menu SET status=@status,pid=@pid,title=@title,code=@code,icon=@icon,path=@path,menutype=@menutype,seq=@seq,viewpath=@viewpath WHERE id=@id");

                int cnt = db.Current_Conn.Execute(sql.ToString(), new { id = entry.id, status = entry.status, pid = entry.pid, title = entry.title, code = entry.code, icon = entry.icon, path = entry.path,menutype=entry.menutype,seq=entry.seq,viewpath=entry.viewpath });
                if (cnt > 0)
                {
                    solr.Delete(new SolrQuery("entitytype:sys_menu && id:" + entry.id.ToString()));
                    entry.entitytype = "sys_menu";
                    solr.Add(entry);
                    solr.Commit();
                }
                return cnt;
            }
        }

        public sys_menu UpItem(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * \n");
                sql.Append("             FROM   dbo.sys_menu \n");
                sql.Append("             WHERE  id = @id");
                return db.Current_Conn.Query<sys_menu>(sql.ToString(), new { id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<sys_menu> MenuTree(int pid)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("WITH mtree \n");
                sql.Append("     AS (SELECT id, \n");
                sql.Append("                pid, \n");
                sql.Append("                title,menutype,code,status \n");
                sql.Append("         FROM   dbo.sys_menu \n");
                sql.Append("         WHERE  pid = @pid \n");
                sql.Append("         UNION ALL \n");
                sql.Append("         SELECT ta.id, \n");
                sql.Append("                ta.pid, \n");
                sql.Append("                ta.title,ta.menutype,ta.code,ta.status \n");
                sql.Append("         FROM   dbo.sys_menu ta \n");
                sql.Append("                INNER JOIN mtree tb \n");
                sql.Append("                        ON ta.pid = tb.id) \n");
                sql.Append("SELECT * \n");
                sql.Append("FROM   mtree");
                return db.Current_Conn.Query<sys_menu>(sql.ToString(), new { pid = pid });
            }
        }

        public string MenuCode(int pid)
        {
            string code = string.Empty;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT COUNT(id)+1 as no FROM dbo.sys_menu WHERE pid=@pid");
            using (LocalDB db = new LocalDB())
            {
                int number = db.Current_Conn.Query<int>(sql.ToString(), new { pid = pid }).FirstOrDefault();
                code = number.ToString().PadLeft(3, '0');
            }
            return code;
        }
    }
}
