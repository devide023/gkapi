using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SolrNet;
using SolrNet.Impl;
using GK.Model.public_db;
using System.Configuration;
using System.Net.Http;
using HttpWebAdapters;
using System.Net;
using HttpWebAdapters.Adapters;

namespace GoldKeyWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            string solrurl = ConfigurationManager.AppSettings["solrurl"];
            string solrUser = ConfigurationManager.AppSettings["solruser"];
            string solrPwd = ConfigurationManager.AppSettings["solrpassword"];
            var connection = new SolrNet.Impl.SolrConnection(solrurl)
            {
                HttpWebRequestFactory = new HttpWebAdapters.BasicAuthHttpWebRequestFactory(solrUser, solrPwd)
            };
            SolrNet.Startup.Init<sys_user>(connection);
            SolrNet.Startup.Init<sys_role>(connection);
            SolrNet.Startup.Init<sys_menu>(connection);
            SolrNet.Startup.Init<sys_organize>(connection);

        }
    }

    public class DemoHttpWebRequestFactory : HttpWebRequestFactory
    {
        public DemoHttpWebRequestFactory(string url)
        {
            Create(url);
        }         

        public IHttpWebRequest Create(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("admin:123456"));
            req.Headers.Add("Authorization", "Basic " + credentials);
            return new HttpWebRequestAdapter((HttpWebRequest)req);
        }
    }
}
