using BIA.Net.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            TraceManager.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
