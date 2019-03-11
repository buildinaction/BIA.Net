namespace $safeprojectname$.ControllersApi
{
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Services;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Web.Http;

    /// <summary>
    /// SiteAdmins Controller
    /// </summary>
    public class SiteAdminsController : BaseAPIController
    {
        /// <summary>
        /// the service site
        /// </summary>
        private ServiceSite serviceSite;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAdminsController"/> class.
        /// </summary>
        public SiteAdminsController()
        {
            this.serviceSite = BIAUnity.Resolve<ServiceSite>();
        }

        /// <summary>
        /// Get all the sites and the admins of this site
        /// </summary>
        /// <returns>list of sites and the admins of this site</returns>
        [HttpGet]
        public string Get()
        {
            List<SiteAdminsDTO> sitesAdmins = serviceSite.GetAllSitesAndAdmins();
            string requestsJson = JsonConvert.SerializeObject(sitesAdmins, JSonFormat());

            return requestsJson;
        }
    }
}