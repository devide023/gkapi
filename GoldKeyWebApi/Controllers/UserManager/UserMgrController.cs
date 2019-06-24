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
                int exist = 0;
                Tool tool = new Tool();
                entry.rkey = tool.RandNum();
                entry.userpwd = tool.Md5(entry.userpwd);
                UserService us = new UserService();
                var list = us.List(new userparm { user_code = entry.usercode, pageindex = 1, pagesize = int.MaxValue }, out exist);
                if (exist == 0)
                {
                    int cnt = us.Add(entry);
                    return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "erroe" });
                }
                else
                {
                    return Json(new { code = 0, msg = "用户代号重复！" });
                }
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
        [HttpPost]
        public IHttpActionResult UserList(userparm parm)
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
        public IHttpActionResult CheckLogin(string usercode, string userpwd)
        {
            try
            {
                UserService us = new UserService();
                sys_user entry = us.Check_UserLogin(usercode, userpwd);
                return entry.id > 0 ? Json(new { code = 1, msg = "ok", user = entry }) : Json(new { code = 1, msg = "用户名或密码错误！", user = new sys_user() });
            }
            catch (Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }
        }

        [Route("del")]
        [HttpPost]
        public IHttpActionResult RemoveUsers(int[] ids)
        {
            try
            {
                UserService us = new UserService();
                int cnt = us.Del(ids.ToList());
                return cnt > 0 ? Json(new { code=1,msg="ok"}):Json(new { code=0,msg="error"});
            }
            catch (Exception e)
            {
                return Json(new { code=0,msg=e.Message});
            }
        }
        [Route("disabel")]
        [HttpPost]
        public IHttpActionResult DisabelUsers(dynamic obj)
        {
            try
            {
                UserService us = new UserService();
                List<int> ids = new List<int>();
                foreach (var item in obj.ids)
                {
                    ids.Add((int)item);
                }
                int cnt = us.Disabel_User(ids,(int)obj.status);
                return cnt > 0 ? Json(new { code = 1, msg = "数据操作成功" }) : Json(new { code = 0, msg = "数据操作失败" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
                throw;
            }
        }
    }
}
