using System.Collections.Generic;

namespace BIA.Net.Business.JQueryDataTable
{
    public class JQueryDataTableParameterModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// First Sort column
        /// </summary>
        public int iSortCol_0 { get; set; }

        /// <summary>
        /// Direction of First Sort column
        /// </summary>
        public string sSortDir_0 { get; set; }

        /// <summary>
        /// String of sNames separated by semi-colon
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// List of Columns fill by controller.
        /// </summary>
        public List<JQueryDataTableParameterColumn> Columns { get; set; }
    }

    /// <summary>
    /// Jquery DataTable Parameter Column
    /// </summary>
    public class JQueryDataTableParameterColumn
    {
        public int Index { get; set; }

        public string SName { get; set; }

        public bool Orderable { get; set; }

        public bool Searchable { get; set; }
    }
}
