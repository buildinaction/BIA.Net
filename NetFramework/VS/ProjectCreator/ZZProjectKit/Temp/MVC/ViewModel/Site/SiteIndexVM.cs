namespace $safeprojectname$.ViewModel.Site
{
    using Business.DTO;
    using System.Collections.Generic;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class SiteIndexVM
    {
        /// <summary>
        /// Gets or sets filter
        /// </summary>
        public SiteFilterVM Filter { get; set; }

        /// <summary>
        /// Gets or sets the Model
        /// </summary>
        public List<SiteDTO> ListSiteDTO { get; set; }
    }
}