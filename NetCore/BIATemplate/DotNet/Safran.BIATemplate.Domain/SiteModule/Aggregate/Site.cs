// <copyright file="Site.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.SiteModule.Aggregate
{
    using System.Collections.Generic;

    using BIA.Net.Core.Domain;
    using Safran.BIATemplate.Domain.UserModule.Aggregate;
    using Safran.BIATemplate.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The site entity.
    /// </summary>
    public class Site : VersionedTable, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the members.
        /// </summary>
        public virtual ICollection<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the collection of view site.
        /// </summary>
        public ICollection<ViewSite> ViewSites { get; set; }
    }
}