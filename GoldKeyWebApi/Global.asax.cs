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
            var connection = new SolrConnection(solrurl);
            SolrNet.Startup.Init<sys_user>(connection);
        }
    }
}
