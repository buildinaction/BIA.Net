// <copyright file="Site.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.SiteModule.Aggregate
{
    using System.Collections.Generic;

    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.UserModule.Aggregate;
    using MyCompany.BIATemplate.Domain.ViewModule.Aggregate;

    /// <summary>
    /// The site entity.
    /// </summary>
    public class Site : IEntity
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