namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Controllers
{
    using BIA.Net.Business.DTO.Infrastructure;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Helpers;
    using ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.DTO;
    using ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Extensions;

    /// <summary>
    /// UserInfos Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
    public class UserInfosController : ApiController
    {
        // GET: api/UserProfile
        [HttpGet]
        public IHttpActionResult Get()
        {
            UserInfo currentUser = User as UserInfo;

            UserInfoDTO userInfoDTO = currentUser?.ToDTO();

            return Ok(userInfoDTO);
        }
    }
}