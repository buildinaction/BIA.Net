namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.CTO
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Class to manage the advanced filter from the filter panel Site
    /// </summary>
    public class SiteAdvancedFilterCTO
    {
        /// <summary>
        /// Gets or sets list of the elements selected for the title from the site
        /// </summary>
        [JsonProperty(PropertyName = "filterTitle")]
        public List<string> FilterTitle { get; set; }

        /// <summary>
        /// Gets or sets list of the members selected
        /// </summary>
        [JsonProperty(PropertyName = "filterMember")]
        public List<string> FilterMember { get; set; }
    }
}