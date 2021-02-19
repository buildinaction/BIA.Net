// <copyright file="DefaultSiteViewDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.View
{
    /// <summary>
    /// DefaultSiteView Dto.
    /// </summary>
    /// <seealso cref="Safran.BIATemplate.Domain.Dto.View.DefaultViewDto" />
    public class DefaultSiteViewDto : DefaultViewDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public int SiteId { get; set; }
    }
}