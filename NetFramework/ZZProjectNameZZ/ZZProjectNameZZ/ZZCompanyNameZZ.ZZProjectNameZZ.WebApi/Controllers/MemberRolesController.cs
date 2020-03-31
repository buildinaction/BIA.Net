namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Business.DTO;
    using Business.Services;
    
    /// <summary>
    /// MemberRoles Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
    public class MemberRolesController : ApiController
    {
        /// <summary>
        /// The service site
        /// </summary>
        private ServiceMemberRole serviceMemberRole;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberRolesController"/> class.
        /// </summary>
        /// <param name="serviceMemberRole">The service site.</param>
        public MemberRolesController(ServiceMemberRole serviceMemberRole)
        {
            this.serviceMemberRole = serviceMemberRole;
        }

        // GET: api/MemberRoles
        [HttpGet]
        public List<MemberRoleDTO> Get()
        {
            return this.serviceMemberRole.GetAll();
        }

        // GET: api/MemberRoles/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (id > 0)
            {
                MemberRoleDTO obj = this.serviceMemberRole.Find(id);

                if (obj == null)
                {
                    return this.NotFound();
                }
                else
                {
                    return this.Ok(obj);
                }
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}
