using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service.RoleManager;
using GK.Model.public_db;
using GK.Model.Parms;
using Webdiyer.WebControls.Mvc;

namespace GoldKeyWebApi.Controllers.RoleManager
{
    [RoutePrefix("api/rolemgr")]
    public class RoleMgrController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult RoleList(roleparm parm)
        {
            try
            {
                int recordcount = 0;
                RoleService rs = new RoleService();
                var list = rs.List(parm, out recordcount);
                return Json(new { code = 1, msg = "ok", list = list, recordcount = recordcount });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddRole(sys_role entry)
        {
            try
            {
                RoleService rs = new RoleService();
                int cnt = rs.Add(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 1, msg = "数据操作失败" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("edit")]
        [HttpPost]
        public IHttpActionResult ModifyRole(sys_role entry)
        {
            try
            {
                RoleService rs = new RoleService();
                int cnt = rs.Update(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 1, msg = "数据操作失败" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("saverolemenus")]
        [HttpPost]
        public IHttpActionResult RoleMenus(dynamic data)
        {
            try
            {
                RoleService rs = new RoleService();
                int roleid = Convert.ToInt32(data.roleid);
                List<int> menuids = new List<int>();
                foreach (var item in data.menuids)
                {
                    menuids.Add(Convert.ToInt32(item));
                }
                int cnt = rs.SaveRoleMenus(roleid, menuids);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("saveroleusers")]
        [HttpPost]
        public IHttpActionResult RoleUsers(dynamic data)
        {
            try
            {
                RoleService rs = new RoleService();
                int roleid = Convert.ToInt32(data.roleid);
                List<int> userids = new List<int>();
                foreach (var item in data.userids)
                {
                    userids.Add(Convert.ToInt32(item));
                }
                int cnt = rs.SaveRoleUsers(roleid, userids);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("menusbyroleids")]
        [HttpPost]
        public IHttpActionResult MenusByRoleid(List<int> roleids)
        {
            try
            {
                RoleService rs = new RoleService();
                var list = rs.GetMenusByRoleid(roleids);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("usersbyroleids")]
        [HttpPost]
        public IHttpActionResult UsersByRoleid(List<int> roleids)
        {
            try
            {
                RoleService rs = new RoleService();
                var list = rs.GetUsersByRoleid(roleids);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
