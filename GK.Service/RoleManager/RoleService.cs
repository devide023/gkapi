﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.DAO;
using GK.Model;
using GK.Model.Parms;
using GK.Model.public_db;
using Dapper;
using Webdiyer.WebControls.Mvc;
using System.Data.SqlClient;
using CommonServiceLocator;
using SolrNet;
namespace GK.Service.RoleManager
{
    public class RoleService : IService<sys_role>
    {
        private ISolrOperations<sys_role> solr = null;
        public RoleService()
        {
            solr = ServiceLocator.Current.GetInstance<ISolrOperations<sys_role>>();
        }

        public int Add(sys_role entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("INSERT INTO dbo.sys_role \n");
                sql.Append("        ( status, code, title, add_time ) \n");
                sql.Append("VALUES  ( @status, -- status - int \n");
                sql.Append("          @code, -- code - nvarchar(50) \n");
                sql.Append("          @title, -- title - nvarchar(50) \n");
                sql.Append("          GETDATE()  -- add_time - datetime \n");
                sql.Append("          );select SCOPE_IDENTITY();");
                int roleid = db.Current_Conn.ExecuteScalar<int>(sql.ToString(), new
                {
                    status = entry.status,
                    code = entry.code,
                    title = entry.title
                });
                entry.id = roleid;
                solr.Add(entry);
                solr.Commit();
                return roleid;
            }
        }

        public int Del(List<int> ids)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from sys_role where id in @ids");
                int cnt = db.Current_Conn.Execute(sql.ToString(), new { ids = ids });
                if (cnt>0)
                {
                    List<ISolrQuery> qs = new List<ISolrQuery>();
                    foreach (var id in ids)
                    {
                        qs.Add(new SolrQuery("entitytype:sys_role && id:" + id.ToString()));
                    }
                    solr.Delete(new SolrMultipleCriteriaQuery(qs, "OR"));
                    solr.Commit();
                }
                return cnt;
            }
        }

        public int Del(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("begin tran delete from sys_role where id=@id \n");
                sql.Append("delete from sys_role_menu where role_id =@id \n");
                sql.Append("delete from sys_roleapi where roleid =@id \n");
                sql.Append("commit \n");
                int cnt = db.Current_Conn.Execute(sql.ToString(), new { id = id });
                if (cnt > 0)
                {
                    solr.Delete(new SolrQuery("id:"+id.ToString()) && new SolrQuery("entitytype:sys_role"));
                    solr.Commit();
                }
                return cnt;
            }
        }

        public sys_role Find(int id)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from sys_role where id=@id");
                return db.Current_Conn.Query<sys_role>(sql.ToString(), new { id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<sys_role> List()
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from sys_role where status = 1");
                return db.Current_Conn.Query<sys_role>(sql.ToString());
            }
        }

        public int Update(sys_role entry)
        {
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("update sys_role set code=@code,title=@title,status=@status where id=@id");
                int cnt = db.Current_Conn.Execute(sql.ToString(), new
                {
                    id = entry.id,
                    code = entry.code,
                    status = entry.status,
                    title = entry.title
                });
                if (cnt>0)
                {
                    solr.Delete(new SolrQuery("entitytype:sys_role && id:" + entry.id.ToString()));
                    entry.entitytype = "sys_role";
                    solr.Add(entry);
                    solr.Commit();
                }

                return cnt;
            }
        }

        public IEnumerable<sys_role> List(roleparm parm, out int recordcount)
        {
            recordcount = 0;
            using (LocalDB db = new LocalDB())
            {
                StringBuilder sql = new StringBuilder();
                DynamicParameters p = new DynamicParameters();
                sql.Append("select * from sys_role where 1=1 \n");
                if (!string.IsNullOrEmpty(parm.key))
                {
                    sql.Append(" and title like @key ");
                    p.Add("key", "%" + parm.key + "%");
                }
                if (!string.IsNullOrEmpty(parm.code))
                {
                    sql.Append(" and code like @code ");
                    p.Add("code", "%" + parm.code + "%");
                }
                var list = db.Current_Conn.Query<sys_role>(sql.ToString(), p).OrderByDescending(t => t.id).ToPagedList(parm.pageindex, parm.pagesize);
                recordcount = list.TotalItemCount;
                return list;
            }
        }

        public int SaveRoleMenus(int roleid, List<int> menusids)
        {
            string tsql = "delete from sys_role_menu where role_id=@roleid";
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.sys_role_menu \n");
            sql.Append("        ( role_id, menu_id ) \n");
            sql.Append("VALUES  ( @roleid, -- role_id - int \n");
            sql.Append("          @menuid  -- menu_id - int \n");
            sql.Append("          )");
            using (LocalDB db = new LocalDB())
            {
                db.Current_Conn.Open();
                SqlTransaction transaction = db.Current_Conn.BeginTransaction();
                try
                {
                    List<dynamic> list = new List<dynamic>();
                    foreach (var item in menusids)
                    {
                        list.Add(new { roleid = roleid, menuid = Convert.ToInt32(item) });
                    }
                    db.Current_Conn.Execute(tsql, new { roleid = roleid }, transaction);
                    int cnt = db.Current_Conn.Execute(sql.ToString(), list, transaction);
                    transaction.Commit();
                    return cnt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int SaveRoleUsers(int roleid, List<int> userids)
        {
            string tsql = "delete from sys_user_role where role_id=@roleid";
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.sys_user_role \n");
            sql.Append("( \n");
            sql.Append("  user_id, \n");
            sql.Append("  role_id \n");
            sql.Append(") \n");
            sql.Append("VALUES \n");
            sql.Append("( \n");
            sql.Append("  @userid,-- user_id - int \n");
            sql.Append("  @roleid -- role_id - int \n");
            sql.Append(")");

            using (LocalDB db = new LocalDB())
            {
                db.Current_Conn.Open();
                SqlTransaction transaction = db.Current_Conn.BeginTransaction();
                try
                {
                    List<dynamic> list = new List<dynamic>();
                    foreach (var item in userids)
                    {
                        list.Add(new { roleid = roleid, userid = Convert.ToInt32(item) });
                    }
                    db.Current_Conn.Execute(tsql, new { roleid = roleid }, transaction);
                    int cnt = db.Current_Conn.Execute(sql.ToString(), list, transaction);
                    transaction.Commit();
                    return cnt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int SaveRoleApi(int roleid, List<string> apis)
        {
            string tsql = "delete from sys_roleapi where roleid=@roleid";
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.sys_roleapi \n");
            sql.Append("        ( roleid, path ) \n");
            sql.Append("VALUES  ( @roleid, -- roleid - int \n");
            sql.Append("          @path  -- path - nvarchar(max) \n");
            sql.Append("          )");
            using (LocalDB db = new LocalDB())
            {
                db.Current_Conn.Open();
                SqlTransaction transaction = db.Current_Conn.BeginTransaction();
                try
                {
                    List<dynamic> list = new List<dynamic>();
                    foreach (var item in apis)
                    {
                        list.Add(new { roleid = roleid, path = item });
                    }
                    db.Current_Conn.Execute(tsql, new { roleid = roleid }, transaction);
                    int cnt = db.Current_Conn.Execute(sql.ToString(), list, transaction);
                    transaction.Commit();
                    return cnt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        public IEnumerable<string> RoleApis(int roleid)
        {
            string sql = "SELECT path FROM dbo.sys_roleapi WHERE roleid=@roleid";
            using (LocalDB db = new LocalDB())
            {
               return db.Current_Conn.Query<string>(sql, new { roleid = roleid });
            }
        }
        public IEnumerable<sys_menu> GetMenusByRoleid(List<int> roleids)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT ta.* \n");
            sql.Append("FROM   dbo.sys_menu ta, \n");
            sql.Append("       dbo.sys_role_menu tb \n");
            sql.Append("WHERE  ta.id = tb.menu_id \n");
            sql.Append("       AND tb.role_id IN @ids");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_menu>(sql.ToString(), new { ids = roleids });
            }
        }

        public IEnumerable<sys_user> GetUsersByRoleid(List<int> roleids)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT DISTINCT tb.* \n");
            sql.Append("FROM   dbo.sys_user_role ta, \n");
            sql.Append("       dbo.sys_user tb \n");
            sql.Append("WHERE  ta.user_id = tb.id \n");
            sql.Append("       AND ta.role_id IN @ids ");

            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_user>(sql.ToString(), new { ids = roleids });
            }
        }
    }
}
