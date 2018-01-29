using System.Collections.Generic;

namespace BIA.Net.Business.JQueryDataTable
{
    public class JQueryDataTableAnswerModel
    {
        public string sEcho { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int recordsTotal { get; set; }

        /// <summary>
        /// Gets or sets the total filtered records.
        /// </summary>
        /// <value>
        /// The total filtered records.
        /// </value>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable<object> aaData { get; set; }

    }
}
