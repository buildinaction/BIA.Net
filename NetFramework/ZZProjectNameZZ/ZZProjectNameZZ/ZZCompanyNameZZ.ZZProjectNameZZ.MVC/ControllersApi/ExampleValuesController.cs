namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.ControllersApi
{
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// Example controller to delete [TODO]...
    /// </summary>
    public class ExampleValuesController : BaseAPIController
    {
        /// <summary>
        /// the service site
        /// </summary>
        private ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleValuesController"/> class.
        /// </summary>
        public ExampleValuesController()
        {
            this.serviceSite = BIAUnity.Resolve<ServiceSite>();
        }

        /// <summary>
        /// GET api/ExampleValues
        /// </summary>
        /// <returns>the list of ExampleValues title</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<SiteDTO> list = this.serviceSite.GetAll();
            return list.Select(s => s.Title).ToArray();
        }

        /// <summary>
        /// GET api/ExampleValues/5
        /// </summary>
        /// <param name="id">the id of a site</param>
        /// <returns>return the ExampleValue title</returns>
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// POST api/ExampleValues
        /// </summary>
        /// <param name="value">the value to post</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// PUT api/ExampleValues/5
        /// </summary>
        /// <param name="id">the id of a ExampleValue</param>
        /// <param name="value">the value to post</param>
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// DELETE api/ExampleValues/5
        /// </summary>
        /// <param name="id">the id of a site</param>
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}