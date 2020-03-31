namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.CTO
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Class to manage the advanced filter from the filter panel ExampleFullAjax
    /// </summary>
    public class ExampleAjaxAdvancedFilterCTO
    {
        /// <summary>
        /// Gets or sets Title
        /// </summary>
        [JsonProperty(PropertyName = "filterTitle")]
        public List<string> FilterTitle { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [JsonProperty(PropertyName = "filterDescription")]
        public List<string> FilterDescription { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [JsonProperty(PropertyName = "filterValue")]
        public List<string> FilterValue { get; set; }

        /// <summary>
        /// Gets or sets MyDecimal
        /// </summary>
        [JsonProperty(PropertyName = "filterMyDecimal")]
        public List<string> FilterMyDecimal { get; set; }

        /// <summary>
        /// Gets or sets MyDouble
        /// </summary>
        [JsonProperty(PropertyName = "filterMyDouble")]
        public List<string> FilterMyDouble { get; set; }

        /// <summary>
        /// Gets or sets DateOnly
        /// </summary>
        [JsonProperty(PropertyName = "filterDateOnly")]
        public List<string> FilterDateOnly { get; set; }

        /// <summary>
        /// Gets or sets DateAndTime
        /// </summary>
        [JsonProperty(PropertyName = "filterDateAndTime")]
        public List<string> FilterDateAndTime { get; set; }

        /// <summary>
        /// Gets or sets TimeOnly
        /// </summary>
        [JsonProperty(PropertyName = "filterTimeOnly")]
        public List<string> FilterTimeOnly { get; set; }

        /// <summary>
        /// Gets or sets MyTimeSpan
        /// </summary>
        [JsonProperty(PropertyName = "filterMyTimeSpan")]
        public List<string> FilterMyTimeSpan { get; set; }

        /// <summary>
        /// Gets or sets TimeSpanOver24H
        /// </summary>
        [JsonProperty(PropertyName = "filterTimeSpanOver24H")]
        public List<string> FilterTimeSpanOver24H { get; set; }

        /// <summary>
        /// Gets or sets Site
        /// </summary>
        [JsonProperty(PropertyName = "filterSite")]
        public List<string> FilterSite { get; set; }
    }
}