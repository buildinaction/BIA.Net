// <copyright file="BaseController.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Controllers
{
    using BIA.Net.Business.DTO;
    using BIA.Net.Business.Services;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using BIA.Net.Dialog.MVC.Controllers;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        /// Retrieve a specific view
        /// </summary>
        /// <param name="viewId">View Id specified</param>
        /// <returns>return the view in JSON format</returns>
        public JsonResult GetViewPreference(int viewId = 0)
        {
            ViewDTO viewSelected = BIAUnity.Resolve<ServiceView>().GetView(viewId);
            if (viewSelected == null)
            {
                // Return the preference empty
                return Json(new { viewId = 0, preference = "{\"dataTableOption\":null,\"advancedFilterValues\":\"\",\"headerFilterValues\":\"\"}" }, JsonRequestBehavior.AllowGet);
            }

            // Return the preference for a view specified
            return Json(new { viewId = viewSelected.Id, preference = viewSelected.Preference }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            beginDate = DateTime.Now;
        }

        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            TraceManager.Debug(GetControllerName(filterContext), GetActionName(filterContext), "Execution time = " + (DateTime.Now - beginDate).Milliseconds.ToString() + "ms");
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

        /// <summary>
        /// Get the differents filter from the advanced panel
        /// </summary>
        /// <param name="currentTableId">Current table Id</param>
        /// <typeparam name="FilterAdvanced">Type of the advancedFilter</typeparam>
        protected void PrepareDefaultView<FilterAdvanced>(string currentTableId)
        {
            FilterAdvanced advancedFilter = default;
            PrepareView<FilterAdvanced>(currentTableId, ref advancedFilter, true);
        }

        /// <summary>
        /// Get the differents filter from the advanced panel
        /// </summary>
        /// <param name="currentTableId">Current table Id</param>
        protected void PrepareDefaultView(string currentTableId)
        {
            PrepareView(currentTableId, true);
        }

        /// <summary>
        /// Get the differents filter from the advanced panel
        /// </summary>
        /// <param name="currentTableId">Current table Id</param>
        /// <param name="advancedFilter">List of the title to filter</param>
        /// <param name="useDefaultView">boolean to force default view</param>
        /// <typeparam name="FilterAdvanced">Type of the advancedFilter</typeparam>
        protected void PrepareView<FilterAdvanced>(string currentTableId, ref FilterAdvanced advancedFilter, bool useDefaultView)
        {
            ViewDTO viewApplied = null;
            if (useDefaultView)
            {
                viewApplied = BIAUnity.Resolve<ServiceView>().GetDefaultView(currentTableId);
            }

            if (viewApplied != null)
            {
                if (!string.IsNullOrEmpty(viewApplied.Preference))
                {
                    advancedFilter = JsonConvert.DeserializeObject<Preference<FilterAdvanced>>(viewApplied.Preference).AdvancedFilterValues;
                }

                if (ViewBag.ViewApplied == null)
                {
                    ViewBag.ViewApplied = new Dictionary<string, ViewDTO>();
                }

                // Set in the ViewBag the list of the table preference
                ViewBag.ViewApplied[currentTableId] = viewApplied;
            }
        }

        /// <summary>
        /// Get the differents filter from the advanced panel
        /// </summary>
        /// <param name="currentTableId">Current table Id</param>
        /// <param name="useDefaultView">boolean to force default view</param>
        protected void PrepareView(string currentTableId, bool useDefaultView)
        {
            ViewDTO viewApplied = null;
            if (useDefaultView)
            {
                viewApplied = BIAUnity.Resolve<ServiceView>().GetDefaultView(currentTableId);
            }

            if (viewApplied != null)
            {
                if (ViewBag.ViewApplied == null)
                {
                    ViewBag.ViewApplied = new Dictionary<string, ViewDTO>();
                }

                // Set in the ViewBag the list of the table preference
                ViewBag.ViewApplied[currentTableId] = viewApplied;
            }
        }

        /// <summary>
        /// Render a Partial view as string to be send as Html string in JSON object for example
        /// </summary>
        /// <param name="viewName"> name of the view</param>
        /// <param name="model">the model</param>
        /// <returns>Html string</returns>
        protected string RenderPartialToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewDataDictionary viewData = new ViewDataDictionary();
            TempDataDictionary tempData = new TempDataDictionary();
            viewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
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