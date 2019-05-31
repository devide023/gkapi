using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Service.Report;
using GK.Model;
using GK.Model.Parms;

namespace GoldKeyWebApi.Controllers.Report
{
    [RoutePrefix("api/report")]
    public class CruisesDateController : ApiController
    {
        /// <summary>
        /// 邮船时间统计
        /// </summary>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        [Route("cruisesdate")]
        [HttpGet]
        public IHttpActionResult cruisesincome_Report(string ksrq,string jsrq)
        {
            try
            {
                CruisesReport report = new CruisesReport();
                var list = report.CruisesIncome(ksrq, jsrq);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("rcno")]
        [HttpGet]
        public IHttpActionResult RcIncome(string rcno)
        {
            try
            {
                CruisesReport rep = new CruisesReport();
                var list = rep.RcIncome(rcno);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
