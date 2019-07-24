using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service.OrganizeManager;
using GK.Model.Parms;
using GK.Model.public_db;
using System.Text;

namespace GoldKeyWebApi.Controllers.OrganizeManager
{
    [RoutePrefix("api/organize")]
    public class OrganizeController : ApiController
    {
        [Route("list")]
        [HttpPost]
        public IHttpActionResult List(organizeparm parm)
        {
            try
            {
                int recordcount = 0;
                OrganizeService os = new OrganizeService();
                var list = os.List(parm, out recordcount);
                return Json(new { code = 1, msg = "ok", list = list, recordcount = recordcount });
            }
            catch (Exception e)
            {
                return Json(new {code=0,msg=e.Message });
            }
        }

        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddOrg(sys_organize entry)
        {
            try
            {
                OrganizeService os = new OrganizeService();
                entry.status = 1;
                entry.add_time = DateTime.Now;
                int cnt = os.Add(entry);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }

        [Route("tree")]
        [HttpGet]
        public IHttpActionResult OrgTree()
        {
            try
            {
                StringBuilder json = new StringBuilder();
                OrganizeService os = new OrganizeService();
                var list = os.Tree(0);
                json.Append("[");
                foreach (var item in list.Where(t => t.pid == 0))
                {
                    json.Append("{\"nodeid\":" + item.id + ",\"parentid\":" + item.pid + ",\"label\":\"" + item.title + "\",\"isedit\":false,\"children\":[" + SubOrg(list, item).ToString() + "]},");
                }
                if (list.Count(t => t.pid == 0) > 0)
                {
                    json.Remove(json.Length - 1, 1);
                }
                json.Append("]");
                return Json(new { code = 1, msg = "ok", data = json.ToString(),nodeid=list.Max(t=>t.id)+1 });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        private StringBuilder SubOrg(IEnumerable<sys_organize> list, sys_organize item)
        {
            StringBuilder json = new StringBuilder();
            if (list.Count(t => t.pid == item.id) > 0)
            {
                foreach (var sitem in list.Where(t => t.pid == item.id))
                {
                    json.Append("{\"nodeid\":" + sitem.id + ",\"parentid\":" + sitem.pid + ",\"label\":\"" + sitem.title + "\",\"isedit\":false,\"children\":[" + SubOrg(list, sitem).ToString() + "]},");
                }
                json.Remove(json.Length - 1, 1);
            }
            return json;
        }

        [Route("saveorg")]
        [HttpPost]
        public IHttpActionResult AddOrganizeTree(dynamic data)
        {
            try
            {
                OrganizeService os = new OrganizeService();
                List<sys_organize> tree = new List<sys_organize>();
                foreach (var item in data)
                {
                    tree.Add(new sys_organize {
                        id = Convert.ToInt32(item.nodeid),
                        pid = Convert.ToInt32(item.parentid),
                        status =1,
                        title=item.label.ToString(),
                        code= string.Empty,
                        address=string.Empty,
                        email= string.Empty,
                        fax= string.Empty,
                        leader= string.Empty,
                        logo= string.Empty,
                        tel= string.Empty,
                        add_time=DateTime.Now,
                        orgtype=1
                    });
                    if (item.children!=null)
                    {
                        GetChildrenData(tree, item.children);
                    }
                }
                int cnt = os.SaveOrganizeTree(tree);
                return cnt > 0 ? Json(new { code = 1, msg = "ok" }) : Json(new { code = 0, msg = "error" });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        private void GetChildrenData(List<sys_organize> targetlist,dynamic children)
        {
            foreach (var item in children)
            {
                targetlist.Add(new sys_organize
                {
                    id = Convert.ToInt32(item.nodeid),
                    pid = Convert.ToInt32(item.parentid),
                    status = 1,
                    title = item.label.ToString(),
                    code = string.Empty,
                    address = string.Empty,
                    email = string.Empty,
                    fax = string.Empty,
                    leader = string.Empty,
                    logo = string.Empty,
                    tel = string.Empty,
                    add_time = DateTime.Now,
                    orgtype = 1
                });
                if (item.children != null)
                {
                    GetChildrenData(targetlist, item.children);
                }
            }
        }
        [Route("nodeinfo")]
        [HttpGet]
        public IHttpActionResult GetOrganizeInfo(int id)
        {
            try
            {
                OrganizeService os = new OrganizeService();
                sys_organize entry = os.Find(id);
                return Json(new { code = 1, msg = "ok",node = entry });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("savenodeinfo")]
        [HttpPost]
        public IHttpActionResult GetOrganizeInfo(sys_organize entry)
        {
            try
            {
                OrganizeService os = new OrganizeService();
                int cnt = os.Update(entry);
                return cnt>0? Json(new { code = 1, msg = "ok"}): Json(new { code = 0, msg = "error"});
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
