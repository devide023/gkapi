using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service;
using GK.Model;

namespace GoldKeyWebApi.Controllers.BaseInfo
{
    [RoutePrefix("aip/baseinfo")]
    public class BaseInfoController : ApiController
    {
        public BaseInfoController()
        {

        }
        [HttpGet]
        public IHttpActionResult CruisesInfo()
        {
            try
            {
                BaseInfo_Service sev = new BaseInfo_Service();
                var list = sev.Get_Cruises();
                return Json(new { code = 1, msg = "ok",list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
