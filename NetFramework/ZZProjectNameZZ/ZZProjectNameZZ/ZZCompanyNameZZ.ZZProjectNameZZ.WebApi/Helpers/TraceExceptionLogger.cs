namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Helpers
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.ExceptionHandling;
    using BIA.Net.Common;

    public class TraceExceptionLogger : ExceptionLogger
    {
        /// <inheritdoc/>
        public override void Log(ExceptionLoggerContext context)
        {
            ApiController apiController = context?.ExceptionContext?.ControllerContext?.Controller as ApiController;
            if (apiController?.ActionContext != null)
            {
                TraceManager.Error(HttpActionContextHelper.GetControllerName(apiController?.ActionContext), HttpActionContextHelper.GetActionName(apiController?.ActionContext), context.ExceptionContext.Exception.Message, context.ExceptionContext.Exception);
            }

            base.Log(context);
        }
    }
}