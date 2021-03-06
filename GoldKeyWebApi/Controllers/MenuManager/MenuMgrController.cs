﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service.MenuManager;
using GK.Model.public_db;
using Webdiyer.WebControls.Mvc;
using GK.Model.Parms.Menu;
using System.Text;
using Newtonsoft.Json;
using GK.Utils;
using System.Configuration;
namespace GoldKeyWebApi.Controllers.MenuManager
{
    [RoutePrefix("api/menumgr")]
    public class MenuMgrController : BaseApiSecurity
    {
        [Route("add")]
        [HttpPost]
        public IHttpActionResult Add_Menu(sys_menu entry)
        {
            try
            {
                var route = Request.GetRouteData();
                MenuService svc = new MenuService();
                int cnt = svc.Add(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "编码重复！" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("batadd")]
        [HttpPost]
        public IHttpActionResult BatAdd_Menu(List<sys_menu> entrys)
        {
            int items = 0;
            int okcnt = 0;
            MenuService svc = new MenuService();
            foreach (var entry in entrys)
            {
                svc.List(new menuparm { url = entry.path }, out items);
                if (items == 0)
                {
                    int cnt = svc.Add(entry);
                    if (cnt > 0)
                    {
                        okcnt++;
                    }
                }
            }
            if (okcnt == entrys.Count())
            {
                return Json(new { code = 1, msg = "数据保存成功！" });
            }
            else
            {
                return Json(new { code = 0, msg = "数据保存失败！" });
            }
        }
        [Route("update")]
        [HttpPost]
        public IHttpActionResult Modify_Menu(sys_menu entry)
        {
            try
            {
                MenuService svc = new MenuService();
                int cnt = svc.Update(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("del")]
        [HttpGet]
        public IHttpActionResult Remove_Menu(int id)
        {
            try
            {
                MenuService svc = new MenuService();
                int cnt = svc.Del(id);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("find")]
        [HttpGet]
        public IHttpActionResult Find_Menu(int id)
        {
            try
            {
                MenuService svc = new MenuService();
                sys_menu entry = svc.Find(id);
                return Json(new { code = 1, msg = "ok", entry = entry });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("list")]
        [HttpPost]
        public IHttpActionResult MenuList(menuparm parm)
        {
            try
            {
                int recordcount = 0;
                MenuService ms = new MenuService();
                var list = ms.List(parm, out recordcount);
                return Json(new { code = 1, msg = "ok", list = list, recordcount = recordcount });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("rootlist")]
        [HttpGet]
        public IHttpActionResult RootList(int pid = 0)
        {
            try
            {
                MenuService ms = new MenuService();
                var list = ms.List(pid);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("upitem")]
        [HttpGet]
        public IHttpActionResult GetUpItem(int id)
        {
            try
            {
                MenuService ms = new MenuService();
                sys_menu entry = ms.UpItem(id);
                return Json(new { code = 1, msg = "ok", entry = entry });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("menutree")]
        [HttpGet]
        public IHttpActionResult MenuTree()
        {
            try
            {
                MenuService ms = new MenuService();
                StringBuilder json = new StringBuilder();
                IEnumerable<sys_menu> root = ms.MenuTree(0);
                json.Append("[");
                foreach (var item in root.Where(t => t.pid == 0))
                {
                    json.Append("{\"id\":" + item.id + ",\"pid\":" + item.pid + ",\"title\":\"" + item.title + "\",\"subitems\":[" + SubMenu(root, item).ToString() + "]},");
                }
                if (root.Count(t => t.pid == 0) > 0)
                {
                    json.Remove(json.Length - 1, 1);
                }
                json.Append("]");
                return Json(new { code = 1, msg = "ok", data = json.ToString() });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("menucode")]
        [HttpGet]
        public IHttpActionResult MenuCode(int pid = 0)
        {
            try
            {
                MenuService ms = new MenuService();
                string menucode = ms.MenuCode(pid);
                return Json(new { code = 1, msg = "ok", menucode= menucode });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("vuecom")]
        [HttpGet]
        public IHttpActionResult VueComponent()
        {
            try
            {
                Tool tool = new Tool();
                string vuedir = ConfigurationManager.AppSettings["vuedir"];
                var list = tool.VueComponents(vuedir).OrderBy(t=>t);
                return Json(new { code = 1, msg = "ok", comlist = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        private StringBuilder SubMenu(IEnumerable<sys_menu> list, sys_menu item)
        {
            StringBuilder json = new StringBuilder();
            if (list.Count(t => t.pid == item.id) > 0)
            {
                foreach (var sitem in list.Where(t => t.pid == item.id))
                {
                    json.Append("{\"id\":" + sitem.id + ",\"pid\":" + sitem.pid + ",\"title\":\"" + sitem.title + "\",\"subitems\":[" + SubMenu(list, sitem).ToString() + "]},");
                }
                json.Remove(json.Length - 1, 1);
            }
            return json;
        }
    }

}
