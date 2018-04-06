namespace BIA.Net.DataTable.DTO
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// DataTable Parameter
    /// </summary>
    [DataContract]
    public class DataTableAjaxPostDTO
    {
        /// <summary>
        /// Gets or sets value of draw parameter sent by client
        /// </summary>
        [DataMember(Name = "draw")]
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets object <see cref="DataTableColumnDTO"/>
        /// </summary>
        [DataMember(Name = "columns")]
        public List<DataTableColumnDTO> Columns { get; set; }

        /// <summary>
        /// Gets or sets object <see cref="DataTableOrderDTO"/>
        /// </summary>
        [DataMember(Name = "order")]
        public List<DataTableOrderDTO> Order { get; set; }

        /// <summary>
        /// Gets or sets start index (for pagination)
        /// </summary>
        [DataMember(Name = "start")]
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets length index (for pagination)
        /// </summary>
        [DataMember(Name = "length")]
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets object <see cref="DataTableSearchDTO"/>
        /// </summary>
        [DataMember(Name = "search")]
        public DataTableSearchDTO Search { get; set; }
    }
}