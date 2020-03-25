

namespace BIA.Net.Authentication.WebAPI
{
    using System.Web.Http.WebHost;
    using System.Web.Routing;
    using System.Web.SessionState;
    public class SessionEnabledControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionEnabledControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }
}