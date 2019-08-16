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
        [ApiSecurity]
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
        [ApiSecurity]
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
                Tool tool = new Tool();
                string token = tool.Encryption(entry.id.ToString()+"&"+userpwd, "apitoken");
                return entry.id > 0 ? Json(new { code = 1, msg = "ok", user = entry,token=token }) : Json(new { code = 0, msg = "用户名或密码错误！", user = new sys_user(),token=string.Empty });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [ApiSecurity]
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
        [ApiSecurity]
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
            }
        }
        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    Tool tool = new Tool();
                    string destoken = tool.Decrypt(token, "apitoken");
                    var index = destoken.IndexOf("&");
                    string struid = destoken.Substring(0, index);
                    int uid = 0;
                    int.TryParse(struid, out uid);
                    UserService us = new UserService();
                    int cnt = us.Logout(uid);
                    return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code=0,msg="error"});
                }
                else
                {
                    return Json(new { code = 0, msg = "无效的token!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [ApiSecurity]
        [Route("edit")]
        [HttpPost]
        public IHttpActionResult ModifyUser(sys_user entry)
        {
            try
            {
                UserService us = new UserService();
                int cnt = us.Update(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [ApiSecurity]
        [Route("rolebyuids")]
        [HttpPost]
        public IHttpActionResult GetRoleByUids(List<int> uids)
        {
            try
            {
                UserService us = new UserService();
                var list = us.GetRoleByUid(uids);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [ApiSecurity]
        [Route("saveuserroles")]
        [HttpPost]
        public IHttpActionResult SaveUserRoles(dynamic data)
        {
            try
            {
                int uid = Convert.ToInt32(data.userid);
                List<int> roleids = new List<int>();
                foreach (var item in data.roleids)
                {
                    roleids.Add(Convert.ToInt32(item));
                }
                UserService us = new UserService();
                int cnt = us.SaveUserRole(uid, roleids);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [ApiSecurity]
        [Route("usermenus")]
        [HttpGet]
        public IHttpActionResult GetMenusByUid(string token)
        {
            try
            {
                Tool tool = new Tool();
                string strTicket = tool.Decrypt(token, "apitoken");
                var index = strTicket.IndexOf("&");
                string struid = strTicket.Substring(0, index);
                int uid = 0;
                int.TryParse(struid, out uid);
                if (uid > 0)
                {
                    UserService us = new UserService();
                    var list = us.GetUserMenus(uid);
                    return Json(new { code = 1, msg = "ok", list = list });
                }
                else
                {
                    return Json(new { code = 0, msg = "error"});
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        
        [Route("info")]
        [HttpGet]
        public IHttpActionResult GetUserInfo(string token)
        {
            try
            {
                Tool tool = new Tool();
                string strTicket = tool.Decrypt(token, "apitoken");
                var index = strTicket.IndexOf("&");
                string struid = strTicket.Substring(0, index);
                int uid = 0;
                int.TryParse(struid, out uid);
                if (uid > 0)
                {
                    UserService us = new UserService();
                    sys_user u = us.Find(uid);
                    IEnumerable<sys_menu> m = us.GetUserMenus(uid);
                    //List<dynamic> menulist = new List<dynamic>();
                    //foreach (var item in m.Where(t=>t.pid==0))
                    //{
                    //    if(m.Count(t=>t.pid==item.id)==0)
                    //    { 
                    //        menulist.Add(new { path = item.path, name = item.code, meta = new { title = item.title, icon = item.icon }, component = item.viewpath });
                    //    }
                    //    else
                    //    {
                    //        var subitem = SubMenus(m, item);
                    //        menulist.Add(new { path = item.path, name = item.code, meta = new { title = item.title, icon = item.icon }, children=subitem, component = item.viewpath });
                    //    }
                    //}
                    return Json(new { code = 1, msg = "ok", user = u, menulist = m });
                }
                else
                {
                    return Json(new { code = 0, msg = "error" });
                }
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        private IEnumerable<dynamic> SubMenus(IEnumerable<sys_menu> list,sys_menu item)
        {
            List<dynamic> menulist = new List<dynamic>();
            foreach (var sitem in list.Where(t=>t.pid==item.id))
            {
                if (list.Count(t => t.pid == sitem.id) == 0)
                {
                    menulist.Add(new { path = sitem.path, name = sitem.code, meta = new { title = sitem.title, icon = sitem.icon }, component = sitem.viewpath });
                }
                else
                {
                    var subitem = SubMenus(list, sitem);
                    menulist.Add(new { path = sitem.path, name = sitem.code, meta = new { title = sitem.title, icon = sitem.icon }, children = subitem, component = sitem.viewpath });
                }
            }
            return menulist;
        }
    }
}
