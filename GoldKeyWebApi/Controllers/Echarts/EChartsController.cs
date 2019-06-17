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
                return Json(new
                {
                    code = 1,
                    msg = "ok",
                    list = list
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
        [Route("cruisesdic/{cruisesno?}")]
        [HttpGet]
        public IHttpActionResult Echart_Cruises_Class(string ksrq,string jsrq,string cruisesno="")
        {
            try
            {
                CruisesReport rep = new CruisesReport();
                var list = rep.Cruises_Rc_Class(ksrq, jsrq,cruisesno).OrderByDescending(t=>t.je);
                var cruiseslist = list.Select(t => new { cruisesno = t.cruisesno, cruisesname = t.cruisesname}).Distinct();
                var placelist = list.Select(t => new { placeno = t.placeno, placename = t.placename }).Distinct();
                List<dynamic> jelist = new List<dynamic>();
                foreach (var sitem in placelist)
                {
                    var datalist = new List<decimal>();
                    var tempdata = new { name = sitem.placename, type = "bar", label="labelOption", data= datalist };
                    foreach (var item in cruiseslist)
                    {
                       var je = list.Where(t => t.cruisesno == item.cruisesno && t.placeno == sitem.placeno).Sum(t => (decimal)t.je);
                        datalist.Add(je);
                    }
                    jelist.Add(tempdata);
                }
                return Json(new { code = 1, msg = "ok", list = list, cruiseslist= cruiseslist, placelist= placelist, jelist= jelist });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
    }
}
