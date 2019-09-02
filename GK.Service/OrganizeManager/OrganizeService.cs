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
using SolrNet;
using CommonServiceLocator;
namespace GK.Service.OrganizeManager
{
    public class OrganizeService : IService<sys_organize>, IListService<sys_organize, organizeparm>
    {
        private ISolrOperations<sys_organize> solr = null;
        public OrganizeService()
        {
            this.solr = ServiceLocator.Current.GetInstance<ISolrOperations<sys_organize>>();
        }
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
            sql.Append(" WHERE NOT EXISTS (SELECT * FROM dbo.sys_organize WHERE code=@code)\n select SCOPE_IDENTITY();\n");
            using (LocalDB db = new LocalDB())
            {
                int nodeid = db.Current_Conn.ExecuteScalar<int>(sql.ToString(), new
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
                if (nodeid>0)
                {
                    entry.id = nodeid;
                    entry.entitytype = "sys_organize";
                    solr.Add(entry);
                    solr.Commit();
                }
                return nodeid;
            }

        }

        public int Del(List<int> ids)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.sys_organize WHERE id IN @ids");
            using (LocalDB db = new LocalDB())
            {
                int cnt = db.Current_Conn.Execute(sql.ToString(), new { ids = ids });
                List<ISolrQuery> qs = new List<ISolrQuery>();
                foreach (int id in ids)
                {
                    qs.Add(new SolrQuery("entitytype:sys_organize && id:" + id.ToString()));  
                }
                solr.Delete(new SolrMultipleCriteriaQuery(qs, "OR"));
                solr.Commit();
                return cnt;
            }
        }

        public int Del(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.sys_organize WHERE id =@id");
            using (LocalDB db = new LocalDB())
            {
                int cnt = db.Current_Conn.Execute(sql.ToString(), new { id = id });
                solr.Delete(new SolrQuery("entitytype:sys_organize && id:"+id.ToString()));
                solr.Commit();
                return cnt;
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
            sql.Append("          code=@code , -- pid - int \n");
            sql.Append("          orgtype=@orgtype , -- orgtype - int \n");
            sql.Append("          title=@title , -- title - nvarchar(max) \n");
            sql.Append("          tel=@tel , -- tel - nvarchar(50) \n");
            sql.Append("          fax=@fax , -- fax - nvarchar(50) \n");
            sql.Append("          email=@email , -- email - nvarchar(50) \n");
            sql.Append("          leader=@leader , -- leader - nvarchar(50) \n");
            sql.Append("          logo=@logo , -- logo - nvarchar(max) \n");
            sql.Append("          address=@address , -- address - nvarchar(max) \n");
            sql.Append("          modify_time=getdate()  -- modify_time - datetime \n");
            sql.Append(" where id = @id \n");
            using (LocalDB db = new LocalDB())
            {
                int cnt = db.Current_Conn.Execute(sql.ToString(), new
                {
                    id = entry.id,
                    status = entry.status,
                    pid = entry.pid,
                    code = entry.code,
                    orgtype = entry.orgtype,
                    title = entry.title,
                    tel = entry.tel,
                    fax = entry.fax,
                    email = entry.email,
                    leader = entry.leader,
                    logo = entry.logo,
                    address = entry.address
                });
                if (cnt>0)
                {
                    solr.Delete(new SolrQuery("entitytype:sys_organize && id:" + entry.id.ToString()));
                    entry.entitytype = "sys_organize";
                    solr.Add(entry);
                    solr.Commit();
                }
                return cnt;
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

        public int SaveOrganizeTree(List<sys_organize> data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("declare @isexsit int=0 \n ");
            sql.Append("set IDENTITY_INSERT sys_organize on \n");
            foreach (var item in data)
            {
                sql.AppendFormat("select @isexsit=count(id) from sys_organize where id = {0} \n", item.id);
                sql.Append(" if @isexsit=0 \n");
                sql.Append(" begin \n");
                sql.Append("INSERT INTO dbo.sys_organize \n");
                sql.Append("        ( id,status , \n");
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
                sql.AppendFormat("VALUES  ( {0},{1}, -- status - int \n",item.id,item.status);
                sql.AppendFormat("          {0}, -- pid - int \n", item.pid);
                sql.AppendFormat("          {0}, -- orgtype - int \n", item.orgtype);
                sql.AppendFormat("          '{0}', -- code - nvarchar(50) \n", item.code);
                sql.AppendFormat("          '{0}', -- title - nvarchar(max) \n", item.title);
                sql.AppendFormat("          '{0}', -- tel - nvarchar(50) \n", item.tel);
                sql.AppendFormat("          '{0}', -- fax - nvarchar(50) \n", item.fax);
                sql.AppendFormat("          '{0}', -- email - nvarchar(50) \n", item.email);
                sql.AppendFormat("          '{0}', -- leader - nvarchar(50) \n", item.leader);
                sql.AppendFormat("          '{0}', -- logo - nvarchar(max) \n", item.logo);
                sql.AppendFormat("          '{0}', -- address - nvarchar(max) \n", item.address);
                sql.Append("          GETDATE(), -- add_time - datetime \n");
                sql.Append("          NULL  -- modify_time - datetime \n");
                sql.Append("        ) \n");
                sql.Append(" end \n");
                sql.Append(" else \n");
                sql.Append(" begin \n");
                sql.AppendFormat(" update sys_organize set title='{0}' where id = {1} \n", item.title, item.id);
                sql.Append(" end \n");
            }
            sql.Append("set IDENTITY_INSERT sys_organize off \n");
            using (LocalDB db = new LocalDB())
            {
                db.Current_Conn.Open();
                var transaction = db.Current_Conn.BeginTransaction();
                List<int> delids = db.Current_Conn.Query<int>("select id from sys_organize", null, transaction).Except(data.Select(t => t.id)).ToList();
                try
                {
                    db.Current_Conn.Execute("delete from sys_organize where id in @ids", new { ids = delids }, transaction);
                    int cnt = db.Current_Conn.Execute(sql.ToString(),null,transaction);
                    transaction.Commit();
                    if (cnt>0)
                    {
                        solr.Delete(new SolrQuery("entitytype:sys_organize"));
                        foreach (var entry in data)
                        {
                            entry.entitytype = "sys_organize";
                            solr.Add(entry);
                        }
                        solr.Commit();
                    }
                    return cnt;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}
