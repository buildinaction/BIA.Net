// <copyright file="BaseController.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Controllers
{
    using BIA.Net.Common;
    using BIA.Net.Dialog.MVC.Controllers;
    using BIA.Net.Web.Utility;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Base class for controller in $companyName$.$saferootprojectname$ application.
    /// </summary>
    public abstract class BaseController : DialogBaseContoller
    {
        #region Fields

        /// <summary>
        /// Store the begin date.
        /// </summary>
        private DateTime beginDate;

        #endregion Fields

        #region Properties
        #endregion Properties

        #region Methods

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.beginDate = DateTime.Now;
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            TraceManager.Debug(GetControllerName(filterContext), GetActionName(filterContext), "Execution time = " + (DateTime.Now - this.beginDate).Milliseconds.ToString() + "ms");
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            TraceManager.Error(GetControllerName(filterContext), GetActionName(filterContext), null, filterContext.Exception);
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