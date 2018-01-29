namespace BIA.Net.MVC.ViewModel
{
    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class JQueryDataTableParamModel
    {
        /// <summary>
        /// Gets or sets Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>
        public string sEcho { get; set; }

        /// <summary>
        /// Gets or sets text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Gets or sets number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// Gets or sets first record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Gets or sets number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Gets or sets number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Gets or sets comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// Gets or sets sort column
        /// </summary>
        public int iSortCol_0 { get; set; }

        /// <summary>
        /// Gets or sets asc or desc
        /// </summary>
        public string sSortDir_0 { get; set; }
    }
}
