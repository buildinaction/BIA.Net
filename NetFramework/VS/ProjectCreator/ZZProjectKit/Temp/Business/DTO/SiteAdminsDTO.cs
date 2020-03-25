namespace $safeprojectname$.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Sites Admins DTO
    /// </summary>
    public class SiteAdminsDTO
    {
        /// <summary>
        /// Gets or sets the site
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets list of admin's login
        /// </summary>
        public List<AdminDTO> Admins { get; set; }
    }
}