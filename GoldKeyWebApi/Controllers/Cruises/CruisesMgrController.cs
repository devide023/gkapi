using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Model;
using GK.Model.Parms;
using GK.Service;
using Dapper;

namespace GoldKeyWebApi.Controllers.Cruises
{
    [RoutePrefix("api/cruises")]
    public class CruisesMgrController : BaseApiSecurity

    {
        /// <summary>
        /// 邮轮航次列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [Route("shipclass")]
        [HttpPost]
        public IHttpActionResult Get_ShipClass_List(shipclass_parm parm)
        {
            try
            {
                int resultcount = 0;
                ShipClass_Service svc = new ShipClass_Service();
                var list = svc.Get_List(parm,out resultcount);
                return Json(new { code=1, list = list, msg = "ok", resultcount= resultcount });
            }
            catch (Exception e)
            {
                return Json(new { code=0, msg = e.Message });
            }
        }
        /// <summary>
        /// 邮轮列表
        /// </summary>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public IHttpActionResult Get_Cruises_List()
        {
            try
            {
                int resultcount = 0;
                Cruises_Service svc = new Cruises_Service();
                var list = svc.Cruises_List(new baseparm() { pagesize=int.MaxValue,pageindex=1},out resultcount);
                return Json(new { code = 1, msg = "ok",list=list,resultcount= resultcount });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
