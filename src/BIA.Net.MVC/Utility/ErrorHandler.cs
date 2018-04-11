
namespace BIA.Net.MVC.Utility
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;

    /// <summary>
    /// Enables support for CustomErrors ResponseRewrite mode in MVC.
    /// </summary>
    public class ErrorHandler : IHttpModule
    {

        private HttpContext HttpContext { get { return HttpContext.Current; } }

        public static bool IsAjaxRequest(HttpRequest Request)
        {
            return Request.Headers["X-Requested-With"] != null && HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public static CustomErrorsSection CustomErrors { get; set; }

        public void Init(HttpApplication application)
        {
            InitCustomError();
            application.EndRequest += Application_EndRequest;
        }

        public static void InitCustomError()
        {
            System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            CustomErrors = (CustomErrorsSection)configuration.GetSection("system.web/customErrors");
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Response.StatusCode != (int)HttpStatusCode.Unauthorized)
            {
                int statusCode = (int) HttpStatusCode.OK;

                Exception ex = HttpContext.Server.GetLastError();

                var httpException = ex as HttpException;

                if (httpException != null)
                {
                    statusCode = httpException.GetHttpCode();
                }
                else
                {
                    statusCode = HttpContext.Response.StatusCode;
                }

                if (statusCode != (int)HttpStatusCode.OK)
                {
                    if (ErrorHandler.CustomErrors != null)
                    {
                        if ((ErrorHandler.CustomErrors.Mode == CustomErrorsMode.On)
                            ||
                            ((ErrorHandler.CustomErrors.Mode == CustomErrorsMode.RemoteOnly) && (!HttpContext.Current.Request.IsLocal)))
                        {
                            // clear error on server
                            if (ErrorHandler.CustomErrors.Errors.AllKeys.Contains("" + statusCode))
                            {
                                HttpContext.Response.StatusCode = statusCode;
                                var url = ErrorHandler.CustomErrors.Errors["" + statusCode].Redirect;
                                if (ErrorHandler.IsAjaxRequest(HttpContext.Current.Request))
                                {
                                    HttpContext.Server.ClearError();
                                    HttpContext.Response.AddHeader("BIANetDialogRedirectedUrl", url);
                                }
                                else
                                {
                                    HttpContext.Server.ClearError();
                                    HttpContext.Response.Redirect(url);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (ex != null)
                    {
                        TraceManager.Error("Not catched error : " + ex.Message);
                    }
                }
            }

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {

        }
    }
}