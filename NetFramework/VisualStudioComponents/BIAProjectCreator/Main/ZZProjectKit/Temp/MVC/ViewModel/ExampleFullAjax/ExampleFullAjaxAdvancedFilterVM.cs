namespace $safeprojectname$.ViewModel.ExampleFullAjax
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class ExampleFullAjaxAdvancedFilterVM
    {
        /// <summary>
        /// Gets or sets Title MultiSelectList
        /// </summary>
       public MultiSelectList MslTitle { get; set; }

        /// <summary>
        /// Gets or sets Description MultiSelectList
        /// </summary>
       public MultiSelectList MslDescription { get; set; }

        /// <summary>
        /// Gets or sets Value MultiSelectList
        /// </summary>
       public MultiSelectList MslValue { get; set; }

        /// <summary>
        /// Gets or sets MyDecimal MultiSelectList
        /// </summary>
       public MultiSelectList MslMyDecimal { get; set; }

        /// <summary>
        /// Gets or sets MyDouble MultiSelectList
        /// </summary>
       public MultiSelectList MslMyDouble { get; set; }

        /// <summary>
        /// Gets or sets DateOnly MultiSelectList
        /// </summary>
       public MultiSelectList MslDateOnly { get; set; }

        /// <summary>
        /// Gets or sets DateAndTime MultiSelectList
        /// </summary>
       public MultiSelectList MslDateAndTime { get; set; }

        /// <summary>
        /// Gets or sets TimeOnly MultiSelectList
        /// </summary>
       public MultiSelectList MslTimeOnly { get; set; }

        /// <summary>
        /// Gets or sets MyTimeSpan MultiSelectList
        /// </summary>
       public MultiSelectList MslMyTimeSpan { get; set; }

        /// <summary>
        /// Gets or sets TimeSpanOver24H MultiSelectList
        /// </summary>
       public MultiSelectList MslTimeSpanOver24H { get; set; }

        /// <summary>
        /// Gets or sets Site MultiSelectList
        /// </summary>
       public MultiSelectList MslSite { get; set; }
    }
}