namespace $safeprojectname$.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Business.DTO;
    using Business.Services;
    using $safeprojectname$.Jwt;

    /// <summary>
    /// Users Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
    [BiaJwtAuthorize]
    public class UsersController : ApiController
    {
        /// <summary>
        /// The service User
        /// </summary>
        private ServiceUser serviceUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="serviceUser">The service User.</param>
        public UsersController(ServiceUser serviceUser)
        {
            this.serviceUser = serviceUser;
        }

        // GET: api/Users
        [HttpGet]
        public List<UserDTO> Get()
        {
            return this.serviceUser.GetAll();
        }

        // GET: api/Users/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (id > 0)
            {
                UserDTO obj = this.serviceUser.Find(id);

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
