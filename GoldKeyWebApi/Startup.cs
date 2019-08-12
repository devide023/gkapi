using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Threading.Tasks;
using System.IO;

[assembly: OwinStartup(typeof(GoldKeyWebApi.Startup))]

namespace GoldKeyWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            //静态文件托管
            //app.Use((context, fun) =>
            //{
            //    return myhandle(context, fun);
            //});
        }
        public Task myhandle(IOwinContext context, Func<Task> next)
        {
            //获取物理文件路径
            var path = GetFilePath(context.Request.Path.Value);

            //验证路径是否存在
            if (File.Exists(path))
            {
                return SetResponse(context, path);
            }
            //不存在返回下一个请求
            return next();
        }
        public static string GetFilePath(string relPath)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory
                , "..\\..\\"
                , relPath.TrimStart('/').Replace('/', '\\'));
        }
        public Task SetResponse(IOwinContext context, string path)
        {
            var perfix = Path.GetExtension(path);
            if (perfix == ".html")
                context.Response.ContentType = "text/html; charset=utf-8";
            else if (perfix == ".js")
                context.Response.ContentType = "application/x-javascript";
            else if (perfix == ".js")
                context.Response.ContentType = "atext/css";
            return context.Response.WriteAsync(File.ReadAllText(path));
        }
    }
}
