namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.CTO
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class ExampleTable2FACompColAdvancedFilterCTO
    {
        /// <summary>
        /// Gets or sets list of the elements selected for the Title
        /// </summary>
        [JsonProperty(PropertyName = "filterTitle")]
        public List<string> FilterTitle { get; set; }

        /// <summary>
        /// Gets or sets list of the elements selected for the Description
        /// </summary>
        [JsonProperty(PropertyName = "filterDescription")]
        public List<string> FilterDescription { get; set; }

        /// <summary>
        /// Gets or sets list of the elements selected for the Site
        /// </summary>
        [JsonProperty(PropertyName = "filterSite")]
        public List<string> FilterSite { get; set; }
    }
}