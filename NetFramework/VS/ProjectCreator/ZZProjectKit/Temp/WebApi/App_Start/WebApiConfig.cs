namespace $safeprojectname$
{
    using BIA.Net.Api.Utility;
    using BIA.Net.Authentication.WebAPI;
    using BIA.Net.Common.Helpers;
    using System.Web.Http;
    using Unity;
    using Unity.Lifetime;
    using Business.DTO;
    using Business.Helpers;
    using Model.Helpers;
    using MVC;
    using WebApi.Helpers;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Register Unity with Web API.
            BIAUnity.RootContainer = new UnityContainer();
            UnityConfig.RegisterTypes();

            config.DependencyResolver = new WebApiUnityResolver(BIAUnity.RootContainer);
            config.Filters.Add(new BIAAuthorizationFilterWebAPI<UserInfoWebApi, UserDTO>(roles: "User,Admin,Service,Internal"));
        }
    }
}
