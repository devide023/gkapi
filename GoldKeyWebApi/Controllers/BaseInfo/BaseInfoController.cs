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
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.Net.Http.Headers;

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
        [Route("authoritycodes")]
        [HttpGet]
        public IHttpActionResult GetAuthoritycodes()
        {
            try
            {
                BaseInfo_Service bis = new BaseInfo_Service();
                var list = bis.Get_Authority_Codes();
                return Json(new { code = 1, msg = "ok", list = list });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("webapis")]
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
        [Route("uploadfile")]
        [HttpPost]
        public IHttpActionResult Uploadfile()
        {
            try
            {
                List<string> filenames = new List<string>();
                string savedir = HttpContext.Current.Server.MapPath("~/Files");
                if (!Directory.Exists(savedir)) Directory.CreateDirectory(savedir);
                var files = HttpContext.Current.Request.Files.GetMultiple("file");
                foreach (var file in files)
                {
                    String timestampFileName = Guid.NewGuid().ToString();
                    string fileExt = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    string fullpath = Path.Combine(savedir, timestampFileName + fileExt);
                    file.SaveAs(fullpath);
                    filenames.Add(timestampFileName + fileExt);
                }
                return Json(new { code = 1, msg = "文件上传成功！",filenames= filenames });
            }
            catch (Exception e)
            {
                return Json(new { code = 0, msg = e.Message });
            }
        }
        [Route("download")]
        [HttpGet]
        public HttpResponseMessage DownloadFile(string file)
        {
            try
            {
                var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/files/"+ file);
                var stream = new FileStream(FilePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = file
                };
                return response;
            }
            catch (Exception e)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);
                response.Content = new StringContent(e.Message);
                return response;
            }

        }
    }
}
