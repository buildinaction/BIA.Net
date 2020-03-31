namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.ViewModel.Site
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class SiteAdvancedFilterVM
    {
        /// <summary>
        /// Gets or sets title id dto list
        /// </summary>
        public MultiSelectList MslTitle { get; set; }

        /// <summary>
        /// Gets or sets Member id dto list
        /// </summary>
        public MultiSelectList MslMember { get; set; }
/*
        /// <summary>
        /// Gets or sets Site id list
        /// </summary>
        public ICollection<int> SiteIds { get; set; }

        /// <summary>
        /// Gets or sets Member id list
        /// </summary>
        public ICollection<int> MemberIds { get; set; }*/
    }
}