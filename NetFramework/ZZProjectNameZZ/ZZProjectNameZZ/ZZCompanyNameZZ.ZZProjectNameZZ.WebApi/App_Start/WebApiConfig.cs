namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi
{
    using BIA.Net.Api.Utility;
    using BIA.Net.Authentication.WebAPI;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using MVC;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Unity;
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
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());
            GlobalConfiguration.Configuration.Filters.Add(new TraceFilterAttribute());

            // Register Unity with Web API.
            BIAUnity.RootContainer = new UnityContainer();
            UnityConfig.RegisterTypes();

            config.DependencyResolver = new WebApiUnityResolver(BIAUnity.RootContainer);

            // Authentication Windows
            config.Filters.Add(new BIAAuthorizationFilterWebAPI<UserInfoWebApi, UserDTO>(roles: "User,Admin,Service,Internal,AllowVerbOptions"));

            // Authentication JWT
            // config.Filters.Add(new BIAAuthorizationFilterWebAPI<UserInfoWebApi, UserDTO>(roles: "User,Admin,Service,Internal," + Jwt.BiaJwtManager.ALLOW_GET_TOKEN));
        }
    }
}
