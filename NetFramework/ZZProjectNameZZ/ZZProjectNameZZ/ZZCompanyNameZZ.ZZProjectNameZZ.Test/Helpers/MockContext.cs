namespace ZZCompanyNameZZ.ZZProjectNameZZ.Test.Helpers
{
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;

    public class MockContext
    {
        public Mock<RequestContext> RoutingRequestContext { get; private set; }
        public Mock<HttpContextBase> Http { get; private set; }
        public Mock<HttpServerUtilityBase> Server { get; private set; }
        public Mock<HttpResponseBase> Response { get; private set; }
        public Mock<HttpRequestBase> Request { get; private set; }
        public Mock<NameValueCollection> Params { get; private set; }
        public Mock<HttpSessionStateBase> Session { get; private set; }
        public Mock<ActionExecutingContext> ActionExecuting { get; private set; }
        public HttpCookieCollection Cookies { get; private set; }

        public MockContext()
        {
            this.RoutingRequestContext = new Mock<RequestContext>(MockBehavior.Loose);
            this.ActionExecuting = new Mock<ActionExecutingContext>(MockBehavior.Loose);
            this.Http = new Mock<HttpContextBase>(MockBehavior.Loose);
            this.Server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            this.Response = new Mock<HttpResponseBase>(MockBehavior.Loose);
            this.Request = new Mock<HttpRequestBase>(MockBehavior.Loose);
            this.Params = new Mock<NameValueCollection>(MockBehavior.Loose);
            this.Session = new Mock<HttpSessionStateBase>(MockBehavior.Loose);
            this.Cookies = new HttpCookieCollection();
            this.RoutingRequestContext.SetupGet(c => c.HttpContext).Returns(this.Http.Object);
            this.ActionExecuting.SetupGet(c => c.HttpContext).Returns(this.Http.Object);
            this.Http.SetupGet(c => c.Request).Returns(this.Request.Object);
            this.Http.SetupGet(c => c.Response).Returns(this.Response.Object);
            this.Http.SetupGet(c => c.Server).Returns(this.Server.Object);
            this.Http.SetupGet(c => c.Session).Returns(this.Session.Object);
            this.Request.Setup(c => c.Cookies).Returns(this.Cookies);
            this.Request.Setup(c => c.Params).Returns(this.Params.Object);
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));

        }

    }
}