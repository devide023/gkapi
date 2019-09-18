using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model;
using GK.DAO;
using Webdiyer.WebControls.Mvc;
using Dapper;
using GK.Model.public_db;
namespace GK.Service
{
    public class BaseInfo_Service
    {
        public BaseInfo_Service()
        {

        }

        public IEnumerable<cruises> Get_Cruises()
        {
            using (GoldKey_DB db = new GoldKey_DB())
            {
                return db.Get_Con.Query<cruises>("select * from cruises order by code");
            }
        }

        public IEnumerable<sys_menutype> Get_MenuType()
        {
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_menutype>("select * from sys_menutype where status=1 order by id asc");
            }
        }

        public IEnumerable<sys_authority_code> Get_Authority_Codes()
        {
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Query<sys_authority_code>("select * from sys_authority_code order by code asc");
            }
        }

        public int Save_Authority_Codes(sys_authority_code entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO dbo.sys_authority_code \n");
            sql.Append("        ( code, title, status, add_time ) \n");
            sql.Append("SELECT @code, -- code - nvarchar(50) \n");
            sql.Append("          @title, -- title - nvarchar(500) \n");
            sql.Append("          @status, -- status - int \n");
            sql.Append("          GETDATE()  -- add_time - datetime \n");
            sql.Append("WHERE NOT EXISTS (SELECT * FROM dbo.sys_authority_code WHERE code=@code)");
            using (LocalDB db = new LocalDB())
            {
               return db.Current_Conn.Execute(sql.ToString(), entity);
            }
        }

        public int Remove_Authority_Codes(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.sys_authority_code  WHERE id =@id");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Execute(sql.ToString(), new { id=id});
            }
        }

        public int Edit_Authority_Codes(sys_authority_code entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE dbo.sys_authority_code SET code=@code,title=@title,status=@status WHERE id =@id");
            using (LocalDB db = new LocalDB())
            {
                return db.Current_Conn.Execute(sql.ToString(), entity);
            }
        }

    }
}
