namespace $safeprojectname$.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Cors;
    using $safeprojectname$.Jwt;

    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]

    public class TokensController : ApiController
    {
        AppJwtManager appJwtManager;

        public TokensController()
        {
            this.appJwtManager = new AppJwtManager();
        }

        [HttpGet, HttpPost]
        public IHttpActionResult Get()
        {
            string tokenString = this.appJwtManager.GenerateToken();

            if (!string.IsNullOrWhiteSpace(tokenString))
            {
                return this.Ok(new { Token = tokenString });
            }
            else
            {
                return this.Unauthorized();
            }
        }


        // [HttpGet]
        // [BiaJwtAuthorize(Roles = "Admin")]
        // [Route("api/Tokens/TestAdmin")]
        // public IHttpActionResult TestAdmin()
        // {
        //     List<SiteDTO> sites = BIA.Net.Common.Helpers.BIAUnity.Resolve<Business.Services.ServiceSite>().GetAll();
        //     return Ok(sites);
        // }

        // [HttpGet]
        // [BiaJwtAuthorize]
        // [Route("api/Tokens/TestUser")]
        // public IHttpActionResult TestUser()
        // {
        //     List<SiteDTO> sites = BIA.Net.Common.Helpers.BIAUnity.Resolve<Business.Services.ServiceSite>().GetAll();
        //     return Ok(sites);
        // }
    }
}