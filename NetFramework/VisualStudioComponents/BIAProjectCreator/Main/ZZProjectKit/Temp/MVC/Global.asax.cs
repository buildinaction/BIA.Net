// <copyright file="Global.asax.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$
{
    using BIA.Net.Common;
    using BIA.Net.Helpers;
    using BIA.Net.MVC.Utility;
    using BIA.Net.Web.Utility;
    using Business.Helpers;
    using Common.Resources;
    using Common.Resources.BIA.Net;
    using MVC.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

#pragma warning disable SA1649 // Elements must be ordered by access
    /// <summary>
    /// MVC Application
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Called when Application start.
        /// </summary>
        protected void Application_Start()
        {
            TraceManager.Configure();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HtmlHelpersTranslate.InitResources(new List<Type> { typeof(AppliResources), typeof(TextResources) });
            ModelBinders.Binders[typeof(float)] = new SingleModelBinder();
            ModelBinders.Binders[typeof(double)] = new DoubleModelBinder();
            ModelBinders.Binders[typeof(decimal)] = new DecimalModelBinder();
            ModelBinders.Binders[typeof(float?)] = new SingleModelBinder();
            ModelBinders.Binders[typeof(double?)] = new DoubleModelBinder();
            ModelBinders.Binders[typeof(decimal?)] = new DecimalModelBinder();
        }

        /// <summary>
        /// Handles the PreRequestHandlerExecute event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            UserInfoMVC userInfo = (UserInfoMVC)UserInfo.GetCurrentUserInfo();
            CultureHelper.SetLangageCode(userInfo.Language);
        }

        /// <summary>
        /// Trace Error in log file.
        /// </summary>
        /// <param name="sender">application sender</param>
        /// <param name="e">argument</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                TraceManager.Error("MvcApplication", "Application_Error", ex.Message);
            }
        }
    }
#pragma warning restore SA1649 // Elements must be ordered by access
}