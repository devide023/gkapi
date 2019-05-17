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
    }
}
