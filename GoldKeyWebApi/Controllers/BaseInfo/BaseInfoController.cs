using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service;
using GK.Model;
using GK.Utils;
namespace GoldKeyWebApi.Controllers.BaseInfo
{
    [RoutePrefix("api/baseinfo")]
    public class BaseInfoController : ApiController
    {
        public BaseInfoController()
        {

        }
        [Route("cruises")]
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
        [Route("icons")]
        [HttpGet]
        public IHttpActionResult Icons()
        {
            try
            {
                Tool tool = new Tool();
                var list = tool.IconList();
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message});
            }
        }
        [Route("menutypes")]
        [HttpGet]
        public IHttpActionResult SysMenutypes()
        {
            try
            {
                BaseInfo_Service bis = new BaseInfo_Service();
                var list = bis.Get_MenuType();
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
