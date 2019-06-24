using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model.public_db;
using GK.Model.Parms;
using Dapper;
using GK.DAO;
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
                sql.Append("          icon , \n");
                sql.Append("          add_time \n");
                sql.Append("        ) \n");
                sql.Append("VALUES  ( @status , -- status - int \n");
                sql.Append("          @pid , -- pid - int \n");
                sql.Append("          @title , -- title - nvarchar(100) \n");
                sql.Append("          @code , -- code - nvarchar(50) \n");
                sql.Append("          @icon , -- icon - nvarchar(50) \n");
                sql.Append("          GETDATE()  -- add_time - datetime \n");
                sql.Append("        )");
                return db.Current_Conn.Execute(sql.ToString(), new { status = entry.status, pid = entry.pid, title = entry.title, code = entry.code, icon = entry.icon });
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
                sql.Append("select * FROM dbo.sys_menu");
                return db.Current_Conn.Query<sys_menu>(sql.ToString());
            }
        }

        public int Update(sys_menu entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE dbo.sys_menu SET status=@status,pid=@pid,title=@title,code=@code,icon=@icon WHERE id=@id");

                return db.Current_Conn.Execute(sql.ToString(),new { id=entry.id,status=entry.status,pid=entry.pid,title=entry.title,code=entry.code,icon=entry.icon});
            }
        }
    }
}
