// <copyright file="RouteConfig.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$
{
    using BIA.Net.Authentication.WebAPI;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route configuration
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Web API Session Enabled Route Configurations
            routes.MapHttpRoute(
                name: "SessionsRoute",
                routeTemplate: "api/sessions/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }).RouteHandler = new SessionEnabledHttpControllerRouteHandler();

            routes.MapHttpRoute(
                name: "SessionsRouteAction",
                routeTemplate: "api/sessions/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }).RouteHandler = new SessionEnabledHttpControllerRouteHandler();

            // Web API Stateless Route Configurations
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // MVC Route Configurations
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}