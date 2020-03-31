namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Controllers
{
    using Business.DTO;
    using Business.Services;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Linq;

    /// <summary>
    /// Members Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
    public class MembersController : ApiController
    {
        /// <summary>
        /// The service member
        /// </summary>
        private ServiceMember serviceMember;

        /// <summary>
        /// Initializes a new instance of the <see cref="MembersController"/> class.
        /// </summary>
        /// <param name="serviceMember">The service member.</param>
        /// <param name="serviceMember">The service member.</param>
        public MembersController(ServiceMember serviceMember)
        {
            this.serviceMember = serviceMember;
        }

        // GET: api/Members
        [HttpGet]
        [Route("api/sites/{siteId}/members")]
        public List<MemberDTO> GetAllBySite(int siteId)
        {
            return this.serviceMember.GetAllWhere(x => x.Site.Id == siteId);
        }

        // GET: api/Members/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (id > 0)
            {
                MemberDTO obj = this.serviceMember.Find(id);

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

        // POST: api/Members
        [HttpPost]
        public IHttpActionResult Post([FromBody]MemberDTO value)
        {
            if (value != null)
            {
                MemberDTO obj = this.serviceMember.Insert(value);

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

        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]MemberDTO value)
        {
            if (id > 0 && value != null)
            {
                MemberDTO obj = this.serviceMember.UpdateValues(value, new List<string>() { nameof(MemberDTO.MemberRole) });

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

        // DELETE: api/Members/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id > 0)
            {
                int ret = this.serviceMember.DeleteById(id);

                if (ret == -1)
                {
                    return this.NotFound();
                }
                else
                {
                    return this.Ok();
                }
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}
