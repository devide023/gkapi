using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.Model.Parms;
using GK.Model.public_db;
using GK.DAO;
using Dapper;
using Webdiyer.WebControls.Mvc;
namespace GK.Service.UserManager
{
    public class UserService : IService<sys_user>
    {
        public int Add(sys_user entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO dbo.sys_user \n");
                sql.Append("        ( status , \n");
                sql.Append("          code , \n");
                sql.Append("          sex , \n");
                sql.Append("          username , \n");
                sql.Append("          userpwd , \n");
                sql.Append("          company_id , \n");
                sql.Append("          department_id , \n");
                sql.Append("          login_way , \n");
                sql.Append("          login_date , \n");
                sql.Append("          logout_date , \n");
                sql.Append("          address , \n");
                sql.Append("          tel , \n");
                sql.Append("          birthday , \n");
                sql.Append("          modify_date, \n");
                sql.Append("          add_time \n");
                sql.Append("        ) \n");
                sql.Append("VALUES  ( @status , -- status - int \n");
                sql.Append("          @code , -- code - nvarchar(50) \n");
                sql.Append("          @sex , -- sex - nvarchar(50) \n");
                sql.Append("          @username , -- username - nvarchar(50) \n");
                sql.Append("          @userpwd , -- userpwd - nvarchar(50) \n");
                sql.Append("          @company_id , -- company_id - int \n");
                sql.Append("          @department_id , -- department_id - int \n");
                sql.Append("          @login_way , -- login_way - int \n");
                sql.Append("          @login_date , -- login_date - datetime \n");
                sql.Append("          @logout_date , -- logout_date - datetime \n");
                sql.Append("          @address , -- add_time - datetime \n");
                sql.Append("          @tel , -- add_time - datetime \n");
                sql.Append("          @birthday , -- add_time - datetime \n");
                sql.Append("          @modify_date,  -- modify_date - datetime \n");
                sql.Append("          GETDATE() -- add_time - datetime \n");
                sql.Append("        )");

                return db.Current_Conn.Execute(sql.ToString(),
                    new
                    {
                        status = entry.status,
                        code=entry.code,
                        sex=entry.sex,
                        username = entry.username,
                        userpwd = entry.userpwd,
                        company_id = entry.company_id,
                        department_id = entry.department_id,
                        login_way = entry.login_way,
                        login_date = entry.login_date,
                        logout_date = entry.logout_date,
                        address=entry.address,
                        tel=entry.tel,
                        birthday=entry.birthday,
                        modify_date = entry.modify_date
                    });
            }
        }

        public int Del(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("delete from sys_user where id = @id");
                return db.Current_Conn.Execute(sql.ToString(), new { id = id });
            }
        }

        public sys_user Find(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("select * from sys_user where id = @id");
                return db.Current_Conn.Query<sys_user>(sql.ToString(), new { id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<sys_user> List()
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat("select * from sys_user");
                return db.Current_Conn.Query<sys_user>(sql.ToString());
            }
        }

        public int Update(sys_user entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE dbo.sys_user \n");
                sql.Append("SET    status = @status, \n");
                sql.Append("       username = @username, \n");
                sql.Append("       company_id = @company_id, \n");
                sql.Append("       department_id = @department_id, \n");
                sql.Append("       login_way = @login_way, \n");
                sql.Append("       login_date = @login_date, \n");
                sql.Append("       logout_date = @logout_date, \n");
                sql.Append("       modify_date = @modify_date, \n");
                sql.Append("       add_time = @add_time \n");
                sql.Append("WHERE  id = @id");
                return db.Current_Conn.Execute(sql.ToString(),
                    new
                    {
                        id = entry.id,
                        status = entry.status,
                        username = entry.username,
                        company_id = entry.company_id,
                        department_id = entry.department_id,
                        login_way = entry.login_way,
                        login_date = entry.login_date,
                        logout_date = entry.logout_date,
                        modify_date = entry.modify_date,
                        add_time = entry.add_time
                    });
            }
        }

        public int UserRoles(int userid, List<int> roleids)
        {
            using (LocalDB db = new LocalDB())
            {
                List<dynamic> list = new List<dynamic>();
                foreach (var item in roleids)
                {
                    list.Add(new { user_id =userid, role_id =item});
                }
                StringBuilder sql = new StringBuilder();
                db.Current_Conn.Execute("DELETE FROM dbo.sys_user_role WHERE user_id=@userid;\n",new {userid=userid});
                sql.Append("INSERT INTO dbo.sys_user_role \n");
                sql.Append("        ( user_id, role_id ) \n");
                sql.Append("VALUES  ( @user_id, -- user_id - int \n");
                sql.Append("          @role_id  -- role_id - int \n");
                sql.Append("          ); \n");
                var cnt = db.Current_Conn.Execute(sql.ToString(), list);
                return cnt;

            }
        }

        public IEnumerable<sys_user> List(userparm parm,out int recordcount)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from sys_user where 1=1 ");
                DynamicParameters q = new DynamicParameters();
                if (!string.IsNullOrEmpty(parm.key))
                {
                    sql.AppendFormat(" and username=@username ");
                    q.Add("username", parm.key);
                }
                if (!string.IsNullOrEmpty(parm.company_id))
                {
                    sql.AppendFormat(" and company_id=@company_id ");
                    q.Add("company_id", parm.company_id);
                }
                var list = db.Current_Conn.Query<sys_user>(sql.ToString(), q).OrderByDescending(t => t.id).ToPagedList(parm.pageindex, parm.pagesize);
                recordcount = list.TotalItemCount;
                return list;
            }
        }
    }
}
