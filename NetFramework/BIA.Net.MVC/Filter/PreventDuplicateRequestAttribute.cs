using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIA.Net.MVC.Filter
{
    internal class Http400Result : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            // Set the response code to 403.
            context.HttpContext.Response.StatusCode = 404;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PreventDuplicateRequestAttribute : ActionFilterAttribute
    {
        public const string DuplicatedRequest = "__DuplicatedRequest__";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Request["__RequestVerificationToken"] == null)
                return;

            var currentToken = HttpContext.Current.Request["__RequestVerificationToken"].ToString();

            if (HttpContext.Current.Session["LastProcessedToken"] == null)
            {
                HttpContext.Current.Session["LastProcessedToken"] = currentToken;
                return;
            }

            lock (HttpContext.Current.Session["LastProcessedToken"])
            {
                var lastToken = HttpContext.Current.Session["LastProcessedToken"].ToString();

                if (lastToken == currentToken)
                {
                    filterContext.Result = new Http400Result();
                    return;
                }

                HttpContext.Current.Session["LastProcessedToken"] = currentToken;
            }
        }
    }
}