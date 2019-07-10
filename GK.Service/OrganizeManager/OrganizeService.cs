using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model.public_db;
using Webdiyer.WebControls.Mvc;
using GK.Utils;
using GK.DAO;
using Dapper;
using GK.Model.Parms;
namespace GK.Service.OrganizeManager
{
    public class OrganizeService : IService<sys_organize>, IListService<sys_organize, organizeparm>
    {
        public int Add(sys_organize entry)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.sys_organize \n");
            sql.Append("        ( status , \n");
            sql.Append("          pid , \n");
            sql.Append("          orgtype , \n");
            sql.Append("          code , \n");
            sql.Append("          title , \n");
            sql.Append("          tel , \n");
            sql.Append("          fax , \n");
            sql.Append("          email , \n");
            sql.Append("          leader , \n");
            sql.Append("          logo , \n");
            sql.Append("          address , \n");
            sql.Append("          add_time , \n");
            sql.Append("          modify_time \n");
            sql.Append("        ) \n");
            sql.Append("SELECT @status , -- status - int \n");
            sql.Append("          @pid , -- pid - int \n");
            sql.Append("          @orgtype , -- orgtype - int \n");
            sql.Append("          @code , -- code - nvarchar(50) \n");
            sql.Append("          @title , -- title - nvarchar(max) \n");
            sql.Append("          @tel , -- tel - nvarchar(50) \n");
            sql.Append("          @fax , -- fax - nvarchar(50) \n");
            sql.Append("          @email , -- email - nvarchar(50) \n");
            sql.Append("          @leader , -- leader - nvarchar(50) \n");
            sql.Append("          @logo , -- logo - nvarchar(max) \n");
            sql.Append("          @address , -- address - nvarchar(max) \n");
            sql.Append("          @add_time , -- add_time - datetime \n");
            sql.Append("          @modify_time  -- modify_time - datetime \n");
            sql.Append(" WHERE NOT EXISTS (SELECT * FROM dbo.sys_organize WHERE code=@code)");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Execute(sql.ToString(), new
                {
                    status = entry.status,
                    pid = entry.pid,
                    orgtype = entry.orgtype,
                    code = entry.code,
                    title = entry.title,
                    tel = entry.tel,
                    fax = entry.fax,
                    email = entry.email,
                    leader = entry.leader,
                    logo = entry.logo,
                    address = entry.address,
                    add_time = entry.add_time,
                    modify_time = entry.modify_time
                });
            }

        }

        public int Del(List<int> ids)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.sys_organize WHERE id IN @ids");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Execute(sql.ToString(), new { ids = ids });
            }
        }

        public int Del(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.sys_organize WHERE id =@id");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Execute(sql.ToString(), new { id = id });
            }
        }

        public sys_organize Find(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * FROM dbo.sys_organize WHERE id =@id");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_organize>(sql.ToString(), new { id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<sys_organize> List()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * FROM dbo.sys_organize");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_organize>(sql.ToString());
            }
        }

        public IEnumerable<sys_organize> List(organizeparm parm, out int recordcount)
        {
            DynamicParameters p = new DynamicParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM dbo.sys_organize WHERE 1=1");
            if (!string.IsNullOrEmpty(parm.code))
            {
                sql.Append(" and code like @code ");
                p.Add("code", '%' + parm.code + '%');
            }
            if (!string.IsNullOrEmpty(parm.key))
            {
                sql.Append(" and title like @key ");
                p.Add("key", '%' + parm.key + '%');
            }
            if (!string.IsNullOrEmpty(parm.pid))
            {
                sql.Append(" and pid = @pid ");
                p.Add("pid", parm.pid);
            }
            using (LocalDB db = new LocalDB())
            {
                var list = db.Current_Conn.Query<sys_organize>(sql.ToString(), p).OrderByDescending(t => t.id).ToPagedList(parm.pageindex, parm.pagesize);
                recordcount = list.TotalItemCount;
                return list;
            }

        }

        public int Update(sys_organize entry)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update sys_organize set status = @status , -- status - int \n");
            sql.Append("          pid=@pid , -- pid - int \n");
            sql.Append("          orgtype=@orgtype , -- orgtype - int \n");
            sql.Append("          title=@title , -- title - nvarchar(max) \n");
            sql.Append("          tel=@tel , -- tel - nvarchar(50) \n");
            sql.Append("          fax=@fax , -- fax - nvarchar(50) \n");
            sql.Append("          email=@email , -- email - nvarchar(50) \n");
            sql.Append("          leader=@leader , -- leader - nvarchar(50) \n");
            sql.Append("          logo=@logo , -- logo - nvarchar(max) \n");
            sql.Append("          address=@address , -- address - nvarchar(max) \n");
            sql.Append("          modify_time=@modify_time  -- modify_time - datetime \n");
            sql.Append(" where id = @id \n");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Execute(sql.ToString(), new
                {
                    id = entry.id,
                    status = entry.status,
                    pid = entry.pid,
                    orgtype = entry.orgtype,
                    title = entry.title,
                    tel = entry.tel,
                    fax = entry.fax,
                    email = entry.email,
                    leader = entry.leader,
                    logo = entry.logo,
                    address = entry.address,
                    modify_time = entry.modify_time
                });
            }
        }

        public IEnumerable<sys_organize> Tree(int pid)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("WITH tree AS \n");
            sql.Append("(SELECT * FROM dbo.sys_organize WHERE pid=@pid \n");
            sql.Append("UNION ALL \n");
            sql.Append("SELECT ta.* FROM dbo.sys_organize ta INNER JOIN tree tb ON ta.pid =  tb.id \n");
            sql.Append(") \n");
            sql.Append("SELECT * FROM tree \n");
            sql.Append(" \n");
            sql.Append("");
            using (LocalDB db = new LocalDB())
            {
               return db.Current_Conn.Query<sys_organize>(sql.ToString(), new { pid = pid });
            }
        }
    }
}
