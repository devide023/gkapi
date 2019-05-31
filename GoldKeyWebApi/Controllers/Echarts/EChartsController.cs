using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GK.Model;
using GK.Service.Report;

namespace GoldKeyWebApi.Controllers.Echarts
{
    [RoutePrefix("api/echarts")]
    public class EChartsController : ApiController
    {
        [Route("cruisesrc")]
        [HttpGet]
        public IHttpActionResult Get_CruisesRc(string ksrq, string jsrq, string cruisesno)
        {
            try
            {
                CruisesReport rep = new CruisesReport();
                var list = rep.CruisesRc(ksrq, jsrq, cruisesno).ToList().OrderBy(t=>t.rcno);
                var cruises_list = list.Select(t => new { rcno = t.rcno, je = t.curr }).ToList();
                string no = list.FirstOrDefault().rcno;
                return Json(new
                {
                    code = 1,
                    msg = "ok",
                    option = new
                    {
                        title = new { text = "黄金"+no.Substring(1,1)+"号各航次销售" },
                        tooltip = new { },
                        legend = new { data = new string[] { "销售额" } },
                        xAxis = new { data = cruises_list.Select(t => t.rcno) },
                        yAxis = new { },
                        series = new { name = "销售额", type = "bar", data = cruises_list.Select(t => t.je) }
                    }
                });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }

        }
        [Route("chartcruises")]
        [HttpGet]
        public IHttpActionResult Echart_CruisesRc(string ksrq,string jsrq)
        {
            try
            {
                CruisesReport rep = new CruisesReport();
                var list = rep.CruisesRc(ksrq, jsrq, "").ToList().OrderBy(t => t.rcno);
                return Json(new {
                    code = 1,
                    msg = "ok",
                    list = list.ToList()
                });

            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("cruisesincome")]
        [HttpGet]
        public IHttpActionResult Echart_Cruises_Date(string ksrq,string jsrq)
        {
            try
            {
                CruisesReport rep = new CruisesReport();
                var list = rep.CruisesIncome(ksrq, jsrq).OrderBy(t=>t.cruisesno);
                return Json(new { code = 1, msg = "ok",list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("cruisesdic")]
        [HttpGet]
        public IHttpActionResult Echart_Cruises_Class(string ksrq,string jsrq)
        {
            try
            {
                CruisesReport rep = new CruisesReport();
                var list = rep.Cruises_Rc_Class(ksrq, jsrq);
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
