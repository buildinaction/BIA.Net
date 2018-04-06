namespace BIA.Net.DataTable.DTO
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Datatable Order
    /// </summary>
    [DataContract(Name = "Order")]
    public class DataTableOrderDTO
    {
        /// <summary>
        /// Gets or sets the column index
        /// </summary>
        [DataMember(Name = "column")]
        public int Column { get; set; }

        /// <summary>
        /// Gets or sets the sort order (asc or desc)
        /// </summary>
        [DataMember(Name = "dir")]
        public string Dir { get; set; }
    }
}