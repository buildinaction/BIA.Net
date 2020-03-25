namespace BIA.Net.DataTable.DTO
{
    using System.Runtime.Serialization;

    /// <summary>
    /// DataTable Column
    /// </summary>
    [DataContract(Name = "Column")]
    public class DataTableColumnDTO
    {
        /// <summary>
        /// Gets or sets Data
        /// </summary>
        [DataMember(Name = "data")]
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets name of column
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is searchable.
        /// </summary>
        [DataMember(Name = "searchable")]
        public bool Searchable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is sortable
        /// </summary>
        [DataMember(Name = "orderable")]
        public bool Orderable { get; set; }

        /// <summary>
        /// Gets or sets object <see cref="DataTableSearchDTO"/>
        /// </summary>
        [DataMember(Name = "search")]
        public DataTableSearchDTO Search { get; set; }
    }
}