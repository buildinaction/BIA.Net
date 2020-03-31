namespace BIA.Net.DataTable.DTO
{
    using System.Runtime.Serialization;

    /// <summary>
    /// DataTable Search
    /// </summary>
    [DataContract(Name = "Search")]
    public class DataTableSearchDTO
    {
        /// <summary>
        /// Gets or sets the text to find
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable/disable escaping of regular expression characters in the search term
        /// </summary>
        [DataMember(Name = "regex")]
        public bool Regex { get; set; }
    }
}