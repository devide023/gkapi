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
namespace GK.Service.MenuManager
{
    public class MenuService : IService<sys_menu>
    {
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
                sql.Append("          seq , \n");
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
                sql.Append("          @seq , -- seq - nvarchar(50) \n");
                sql.Append("          GETDATE()  -- add_time - datetime \n");
                sql.Append("where NOT EXISTS(select * from sys_menu where code=@code)\n");
                return db.Current_Conn.Execute(sql.ToString(), new { status = entry.status, pid = entry.pid, title = entry.title, code = entry.code, menucode=entry.menucode, icon = entry.icon, path = entry.path, menutype = entry.menutype, seq = entry.seq });
            }
        }

        public int Del(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("DELETE FROM dbo.sys_menu WHERE id=@id");
                return db.Current_Conn.Execute(sql.ToString(), new { id = id });
            }
        }

        public int Del(List<int> ids)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("DELETE FROM dbo.sys_menu WHERE id in @ids");
                return db.Current_Conn.Execute(sql.ToString(), new { ids = ids });
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
                sql.Append("select * FROM dbo.sys_menu where 1=1 \n");
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
                sql.Append("UPDATE dbo.sys_menu SET status=@status,pid=@pid,title=@title,code=@code,icon=@icon,path=@path,menutype=@menutype,seq=@seq WHERE id=@id");

                return db.Current_Conn.Execute(sql.ToString(), new { id = entry.id, status = entry.status, pid = entry.pid, title = entry.title, code = entry.code, icon = entry.icon, path = entry.path,menutype=entry.menutype,seq=entry.seq });
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
    }
}
