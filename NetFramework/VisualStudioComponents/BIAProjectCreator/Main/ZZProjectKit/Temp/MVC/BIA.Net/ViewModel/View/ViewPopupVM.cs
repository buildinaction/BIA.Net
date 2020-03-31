namespace BIA.Net.MVC.ViewModel.View
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Class to mange the ViewModel of the Popup of the view
    /// </summary>
    public class ViewPopupVM
    {
        /// <summary>
        /// Gets or sets the table id
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets List of the views associated to the table
        /// </summary>
        public List<ViewVM> ViewsICanSee { get; set; }

        /// <summary>
        /// Gets or sets views of the user to override existing
        /// </summary>
        public SelectList UserViewsUpdatable { get; set; }

        /// <summary>
        ///  Gets or sets views associed to the site yet
        /// </summary>
        public SelectList SitesViewsUpdatable { get; set; }

        /// <summary>
        /// Gets or sets List of the views associated to the table for the site selected
        /// </summary>
        public List<ViewVM> SiteViewsIManage { get; set; }

        /// <summary>
        /// Gets or sets list of the site to manage
        /// </summary>
        public SelectList SitesPossible { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether the user is SiteAdmin
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}