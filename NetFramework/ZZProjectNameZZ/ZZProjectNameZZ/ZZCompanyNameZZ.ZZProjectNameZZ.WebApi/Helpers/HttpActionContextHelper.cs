namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Helpers
{
    using System.Web.Http.Controllers;

    public static class HttpActionContextHelper
    {
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>the name of the controller</returns>
        public static string GetControllerName(HttpActionContext actionContext)
        {
            return actionContext.ControllerContext.ControllerDescriptor.ControllerType.Name;
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>the name of the action</returns>
        public static string GetActionName(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.ActionName;
        }
    }
}