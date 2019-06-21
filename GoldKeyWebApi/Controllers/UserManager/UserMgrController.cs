using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Model.public_db;
using Webdiyer.WebControls.Mvc;
using GK.Service.UserManager;
using GK.Model.Parms;
using GK.Utils;
namespace GoldKeyWebApi.Controllers.UserManager
{
    [RoutePrefix("api/usermgr")]
    public class UserMgrController : ApiController
    {
        [Route("add")]
        [HttpPost]
        public IHttpActionResult Add_User(sys_user entry)
        {
            try
            {
                Tool tool = new Tool();
                entry.rkey = tool.RandNum();
                entry.userpwd = tool.Encryption(entry.userpwd,entry.rkey.ToString());
                UserService us = new UserService();
                int cnt = us.Add(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "erroe" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("userrole")]
        [HttpPost]
        public IHttpActionResult Add_UserRole(dynamic entry)
        {
            try
            {
                UserService us = new UserService();
                int userid = Convert.ToInt32(entry.userid);
                List<int> ids = new List<int>();
                foreach (var item in entry.roleids)
                {
                    ids.Add((Int32)item);
                }
                int cnt = us.UserRoles(userid, ids);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "erroe" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("list")]
        [HttpGet]
        public IHttpActionResult UserList([FromUri]userparm parm)
        {
            int cnt = 0;
            try
            {
                UserService us = new UserService();
                var list = us.List(parm, out cnt);
                return Json(new { code = 1, msg = "ok", list = list, recordcount = cnt });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message, recordcount = cnt });
            }
        }
        [Route("check")]
        [HttpGet]
        public IHttpActionResult CheckLogin(string usercode,string userpwd)
        {
            try
            {
                UserService us = new UserService();
                bool ret = us.Check_UserLogin(usercode, userpwd);
                return ret ? Json(new { code = 1, msg = "ok" }) : Json(new { code=1,msg="用户名或密码错误！"});
            }
            catch (Exception e)
            {
                return Json(new { code=1,msg=e.Message});
            }
        }
    }
}
