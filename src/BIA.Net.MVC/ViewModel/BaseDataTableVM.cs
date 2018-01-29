namespace BIA.Net.MVC.ViewModel
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Base view model for data table management.
    /// </summary>
    /// <typeparam name="TListItem">Type of item listed in the table.</typeparam>
    public abstract class BaseDataTableVM<TListItem>
    {
        #region Fields

        /// <summary>
        /// Store the lazy loaded <see cref="JavaScriptSerializer"/> to use.
        /// </summary>
        private JavaScriptSerializer javaScriptSerializer = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the JSON value to use for columns definition.
        /// </summary>
        public string ColumnsDefinitionJSON
        {
            get
            {
                if (this.ColumnsDefinition == null)
                {
                    return string.Empty;
                }

                return JsonConvert.SerializeObject(ToJQueryDataTableColumnDefinition(this.ColumnsDefinition).ToArray());
            }
        }

        /// <summary>
        /// Gets the javascript value to use for columns definition.
        /// </summary>
        public string ColumnsDefinitionJavascript
        {
            get
            {
                if (this.ColumnsDefinition == null)
                {
                    return string.Empty;
                }

                return this.JavaScriptSerializer.Serialize(ToJQueryDataTableColumnDefinition(this.ColumnsDefinition).ToArray());
            }
        }

        /// <summary>
        /// Gets or sets the items listed in the table.
        /// </summary>
        public IEnumerable<TListItem> Items { get; set; }

        /// <summary>
        /// Gets the html identifier of the targetted table element.
        /// </summary>
        public abstract string TableId { get; }

        /// <summary>
        /// Gets the columns definition.
        /// </summary>
        protected abstract IEnumerable<DataTableColumnDefinition> ColumnsDefinition { get; }

        /// <summary>
        /// Gets the <see cref="JavaScriptSerializer"/> to use.
        /// </summary>
        private JavaScriptSerializer JavaScriptSerializer
        {
            get
            {
                if (this.javaScriptSerializer == null)
                {
                    this.javaScriptSerializer = new JavaScriptSerializer();
                }

                return this.javaScriptSerializer;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generate the <see cref="IEnumerable{T}"/> to use for JQuery data table columns definition management.
        /// </summary>
        /// <param name="columnDefinitions">The <see cref="IEnumerable{DataTableColumnDefinition}"/> to use.</param>
        /// <returns>The <see cref="IEnumerable{T}"/> to use for JQuery data table columns definition management.</returns>
        private static IEnumerable<dynamic> ToJQueryDataTableColumnDefinition(IEnumerable<DataTableColumnDefinition> columnDefinitions)
        {
            if (columnDefinitions == null)
            {
                return null;
            }

            return columnDefinitions
                .Where(c => c != null)
                .Select(
                    c => new
                    {
                        mData = c.MData,
                        sName = c.SName,
                        mRenderMethod = c.MRenderMethod,
                        orderable = c.IsOrderable,
                        searchable = c.IsSearchable
                    });
        }

        #endregion Methods
    }
}