namespace $safeprojectname$.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Business.DTO;
    using Business.Services;

    /// <summary>
    /// Sites Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
    public class SitesController : ApiController
    {
        /// <summary>
        /// The service site
        /// </summary>
        private ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitesController"/> class.
        /// </summary>
        /// <param name="serviceSite">The service site.</param>
        public SitesController(ServiceSite serviceSite)
        {
            this.serviceSite = serviceSite;
        }

        // GET: api/Sites
        [HttpGet]
        public List<SiteDTO> Get()
        {
            return this.serviceSite.GetAll();
        }

        // GET: api/Sites/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            if (id > 0)
            {
                SiteDTO obj = this.serviceSite.Find(id);

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

        // POST: api/Sites
        [HttpPost]
        public IHttpActionResult Post([FromBody]SiteDTO value)
        {
            if (value != null)
            {
                SiteDTO obj = this.serviceSite.Insert(value);

                if (obj == null)
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

        // PUT: api/Sites/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]SiteDTO value)
        {
            if (id > 0 && value != null)
            {
                value.Id = id;

                SiteDTO obj = this.serviceSite.UpdateValues(value, new List<string>() { nameof(SiteDTO.Title) });

                if (obj == null)
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

        // DELETE: api/Sites/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id > 0)
            {
                int ret = this.serviceSite.DeleteById(id);

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
