namespace $safeprojectname$.ControllersApi
{
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Services;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// Example controller to delete [TODO]...
    /// </summary>
    public class SitesController : BaseAPIController
    {
        /// <summary>
        /// the service site
        /// </summary>
        private ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitesController"/> class.
        /// </summary>
        public SitesController()
        {
            this.serviceSite = BIAUnity.Resolve<ServiceSite>();
        }

        /// <summary>
        /// GET api/ExampleValues
        /// </summary>
        /// <returns>the list of ExampleValues title</returns>
        [HttpGet]
        public List<SiteDTO> Get()
        {
            return this.serviceSite.GetAll();
        }

        /// <summary>
        /// GET api/ExampleValues/5
        /// </summary>
        /// <param name="id">the id of a site</param>
        /// <returns>return the ExampleValue title</returns>
        [HttpGet]
        public SiteDTO Get(int id)
        {
            return this.serviceSite.Find(id);
        }

        /// <summary>
        /// POST api/ExampleValues
        /// </summary>
        /// <param name="value">the value to post</param>
        [HttpPost]
        public void Post([FromBody]SiteDTO value)
        {
            this.serviceSite.Insert(value);
        }

        /// <summary>
        /// PUT api/ExampleValues/5
        /// </summary>
        /// <param name="id">the id of a ExampleValue</param>
        /// <param name="value">the value to post</param>
        [HttpPost]
        public void Put(int id, [FromBody]SiteDTO value)
        {
            value.Id = id;
            this.serviceSite.UpdateValues(value, new List<string>() { nameof(SiteDTO.Title) });
        }

        /// <summary>
        /// DELETE api/ExampleValues/5
        /// </summary>
        /// <param name="id">the id of a site</param>
        [HttpDelete]
        public void Delete(int id)
        {
            this.serviceSite.DeleteById(id);
        }
    }
}