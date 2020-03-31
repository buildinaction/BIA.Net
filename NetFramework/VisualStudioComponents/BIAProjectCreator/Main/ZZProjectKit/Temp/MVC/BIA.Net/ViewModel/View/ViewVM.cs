namespace BIA.Net.MVC.ViewModel.View
{
#pragma warning disable SA1210 // Using directives must be ordered alphabetically by namespace
    using $companyName$.$saferootprojectname$.Common.Resources.BIA.Net;
#pragma warning restore SA1210 // Using directives must be ordered alphabetically by namespace

    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class to manage a view
    /// </summary>
    public class ViewVM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewVM"/> class.
        /// </summary>
        public ViewVM()
        {
            Id = 0;
        }

        /// <summary>
        ///  Gets or sets the id of the view
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Gets or sets name of the view
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets the description of the view
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        ///  Gets or sets the differents paramaters for the grid (advanced filter, grid, header)
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        ///  Gets or sets specific comment
        /// </summary>
        public int SitesAssignedCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is assigned to the selected site
        /// </summary>
        public bool IsAssignedToThisSite { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether define if the view is system or site
        /// </summary>
        public bool IsReference { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether define if the view is the default view
        /// </summary>
        public bool IsDefaultView { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether define if the view is the current view
        /// </summary>
        public bool IsCurrentView { get; set; }

        /// <summary>
        /// Gets return a formatted string indicate the usage of the view
        /// </summary>
        public string Usage
        {
            get
            {
                switch (SitesAssignedCount)
                {
                    case 0:
                    case 1:
                        return string.Format(TextResources.View_SiteUsed, SitesAssignedCount);
                    case int n when n > 1:
                        return string.Format(TextResources.View_SitesUsed, SitesAssignedCount);
                }

                return string.Empty;
            }
        }

        /// <summary>
        ///  Gets the name fo the view
        /// </summary>
        /// <returns>return the name of the view</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}