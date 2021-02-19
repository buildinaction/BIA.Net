// <copyright file="QueryCustomMode.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.RepoContract
{
    /// <summary>
    /// Custom mode for the query.
    /// </summary>
    public static class QueryCustomMode
    {
        /// <summary>
        /// Mode Update the view of type user.
        /// </summary>
        public const string ModeUpdateViewUsers = "UpdateViewUsers";

        /// <summary>
        /// Mode Update the view of type site.
        /// </summary>
        public const string ModeUpdateViewSites = "UpdateViewSites";

        /// <summary>
        /// Mode Update the view of type site.
        /// </summary>
        public const string ModeUpdateViewSitesAndUsers = "UpdateViewSitesAndUsers";
    }
}
