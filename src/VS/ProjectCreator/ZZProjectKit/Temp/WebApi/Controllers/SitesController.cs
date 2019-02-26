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
        public SiteDTO Get(int id)
        {
            return this.serviceSite.Find(id);
        }

        // POST: api/Sites
        [HttpPost]
        public void Post([FromBody]SiteDTO value)
        {
            this.serviceSite.Insert(value);
        }

        // PUT: api/Sites/5
        [HttpPut]
        public void Put(int id, [FromBody]SiteDTO value)
        {
            value.Id = id;
            this.serviceSite.UpdateValues(value, new List<string>() { nameof(SiteDTO.Title) });
        }

        // DELETE: api/Sites/5
        [HttpDelete]
        public void Delete(int id)
        {
            this.serviceSite.DeleteById(id);
        }
    }
}
