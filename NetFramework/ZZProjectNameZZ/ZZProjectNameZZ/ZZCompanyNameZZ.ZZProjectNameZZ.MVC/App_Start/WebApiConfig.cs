namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC
{
    using BIA.Net.Api.Utility;
    using BIA.Net.Authentication.WebAPI;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Helpers;
    using System.Web.Http;

    /// <summary>
    /// Configure WebApi
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register service and filter
        /// </summary>
        /// <param name="config">the config</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new BIAAuthorizationFilterWebAPI<UserInfoMVC, UserDTO>(roles: "User,Admin,Service,Internal"));

            // Register Unity with Web API.
            config.DependencyResolver = new WebApiUnityResolver(BIAUnity.RootContainer);
        }
    }
}