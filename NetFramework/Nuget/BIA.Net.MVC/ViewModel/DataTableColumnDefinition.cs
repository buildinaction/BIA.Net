using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.MVC.ViewModel
{
    /// <summary>
    /// Class to describe the definition of a column for the Jquery DataTable
    /// </summary>
    public class DataTableColumnDefinition
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableColumnDefinition"/> class.
        /// </summary>
        public DataTableColumnDefinition()
        {
            // Define default values
            this.MData = null;
            this.MRenderMethod = null;
            this.SName = null;
            this.IsOrderable = true;
            this.IsSearchable = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the mData value.
        /// Default value: null
        /// </summary>
        public string MData { get; set; }

        /// <summary>
        /// Gets or sets the method identifier to use to get the appropriate javascript method for mRender.
        /// Default value: null
        /// </summary>
        public string MRenderMethod { get; set; }

        /// <summary>
        /// Gets or sets the sName value.
        /// Default value: null
        /// </summary>
        public string SName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is orderable.
        /// Default value: true
        /// </summary>
        public bool IsOrderable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is searchable.
        /// Default value: true
        /// </summary>
        public bool IsSearchable { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates the SName content to use for multiple fields. Null or white spaces entries will be ignored.
        /// </summary>
        /// <param name="fieldValues">The field values to use.</param>
        /// <returns>The SName content to use for multiple fields.</returns>
        public static string GenerateSNameFieldsContent(params string[] fieldValues)
        {
            #region Check parameters

            if (fieldValues == null)
            {
                throw new ArgumentNullException("fieldValues");
            }

            #endregion Check parameters

            return string.Join(" || ", fieldValues.Where(fv => !string.IsNullOrWhiteSpace(fv)));
        }

        #endregion Methods
    }
}
