using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service;
using GK.Model;
using GK.Utils;
using System.Configuration;
namespace GoldKeyWebApi.Controllers.BaseInfo
{
    /// <summary>
    /// 基础信息
    /// </summary>
    [RoutePrefix("api/baseinfo")]
    public class BaseInfoController : ApiController
    {
        public BaseInfoController()
        {

        }
        /// <summary>
        /// 邮轮信息
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 图标库
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 系统菜单类型
        /// </summary>
        /// <returns></returns>
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
        [Route("dllinfo")]
        [HttpGet]
        public IHttpActionResult GetDLLInfo()
        {
            try
            {
                Tool tool = new Tool();
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string dllpath = path + "bin\\GoldKeyWebApi.dll";
                var list = tool.GetDLLInfo(dllpath);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
