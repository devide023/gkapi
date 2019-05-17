using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service;
using GK.Model;
using GK.Model.Parms;
using GK.DAO;
namespace GoldKeyWebApi.Controllers.Consume
{
    [RoutePrefix("api/consume")]
    public class ConsumeMgrController : ApiController
    {
        /// <summary>
        /// 消费项目
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="totalitems"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpPost]
        public IHttpActionResult Get_Consume_List(consume_parm parm)
        {
            int totalitems = 0;
            try
            {
                Consume_Service sv = new Consume_Service();
                var list = sv.Get_Consume_List(parm,out totalitems);
                return Json(new { code = 1, msg = "ok", list = list, totalitems = totalitems });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }

        }
        /// <summary>
        /// 消费大类
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="totalitems"></param>
        /// <returns></returns>
        [Route("xmtype")]
        [HttpPost]
        public IHttpActionResult Get_Xmtype(xmtype_parm parm)
        {
            int totalitems = 0;
            try
            {
                Consume_Service sv = new Consume_Service();
                var list = sv.Get_Xmtype_List(parm, out totalitems);
                return Json(new { code = 1, msg = "ok", list = list, totalitems = totalitems });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }

        }
        /// <summary>
        /// 消费站点
        /// </summary>
        /// <param name="parm"></param>
        /// <param name="totalitems"></param>
        /// <returns></returns>
        [Route("placelist")]
        [HttpPost]
        public IHttpActionResult Get_Placeno(menuplace_parm parm)
        {
            int totalitems = 0;
            try
            {
                Consume_Service sv = new Consume_Service();
                var list = sv.Get_Menuplace_List(parm, out totalitems);
                return Json(new { code = 1, msg = "ok", list = list, totalitems = totalitems });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }

        }
    }
}
