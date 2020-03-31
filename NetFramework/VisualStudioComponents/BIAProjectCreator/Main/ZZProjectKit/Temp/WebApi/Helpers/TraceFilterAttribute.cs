namespace $safeprojectname$.Helpers
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using BIA.Net.Common;

    public class TraceFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The begin date
        /// </summary>
        private DateTime beginDate;

        /// <inheritdoc/>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            this.beginDate = DateTime.Now;
        }

        /// <inheritdoc/>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            HttpActionContext actionContext = actionExecutedContext.ActionContext;
            TraceManager.Info(HttpActionContextHelper.GetControllerName(actionContext), HttpActionContextHelper.GetActionName(actionContext), "Execution time = " + Math.Round((DateTime.Now - this.beginDate).TotalMilliseconds, 0) + "ms");
        }
    }
}