namespace BIA.Net.DataTable.DTO
{
    using Newtonsoft.Json;

    /// <summary>
    /// Generic class to manage model send by DataTable in the AJAX action to take advanced filter
    /// </summary>
    /// <typeparam name="FilterAdvanced">Advanced filter</typeparam>
    public class DataTableAjaxPost<FilterAdvanced> : DataTableAjaxPostDTO
    {
        /// <summary>
        /// Gets or sets generic advanced filter
        /// </summary>
        [JsonProperty(PropertyName = "advancedFilter")]
        public FilterAdvanced AdvancedFilter { get; set; }
    }
}