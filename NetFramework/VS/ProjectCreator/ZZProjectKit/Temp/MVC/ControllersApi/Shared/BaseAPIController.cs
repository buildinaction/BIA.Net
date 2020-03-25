// <copyright file="BaseController.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.ControllersApi
{
    using Newtonsoft.Json;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;

    /// <summary>
    /// Base class for controller in $companyName$.$saferootprojectname$ application.
    /// </summary>
    public abstract class BaseAPIController : ApiController
    {
        #region Methods

        /// <summary>
        /// Return the standard JSonFomat parmeter
        /// </summary>
        /// <returns>the standard JSonFomat parmeter</returns>
        protected static JsonSerializerSettings JSonFormat()
        {
            return new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
        }

        /// <summary>
        ///  Called at the init of the context
        /// </summary>
        /// <param name="controllerContext">current context HTTP</param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Private methods

        /// <summary>
        /// Gets the name of the controller within a filter context.
        /// </summary>
        /// <param name="filterContext">The <see cref="ControllerContext"/> to use.</param>
        /// <returns>The controller name.</returns>
        private static string GetControllerName(ControllerContext filterContext)
        {
            return filterContext.RouteData.Values["controller"].ToString();
        }

        /// <summary>
        /// Gets the name of the action within a filter context.
        /// </summary>
        /// <param name="filterContext">The <see cref="ControllerContext"/> to use.</param>
        /// <returns>The action name.</returns>
        private static string GetActionName(ControllerContext filterContext)
        {
            return filterContext.RouteData.Values["action"].ToString();
        }

        #endregion Private methods

        #endregion Methods

    }
}