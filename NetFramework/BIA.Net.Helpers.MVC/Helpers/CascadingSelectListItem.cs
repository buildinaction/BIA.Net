namespace BIA.Net.Helpers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Cascading SelectListItem
    /// </summary>
    public class CascadingSelectListItem : SelectListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSelectListItem"/> class.
        /// </summary>
        public CascadingSelectListItem()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingSelectListItem"/> class.
        /// </summary>
        /// <param name="value">the value of the selected item</param>
        /// <param name="text">the text of the selected item</param>
        /// <param name="parentValues">Key: id of the parent list. Value: List of values of the parent list that allows the display of this data.</param>
        public CascadingSelectListItem(string value, string text, Dictionary<string, List<string>> parentValues)
            : base()
        {
            this.Text = text;
            this.Value = value;
            this.ParentValues = parentValues;
        }

        /// <summary>
        /// Gets or sets mapping parentId > parent values
        /// </summary>
        public Dictionary<string, List<string>> ParentValues { get; set; }
    }
}