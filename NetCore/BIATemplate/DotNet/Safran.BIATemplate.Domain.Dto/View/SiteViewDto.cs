// <copyright file="SiteViewDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.SiteView
{
    using Safran.BIATemplate.Domain.Dto.View;

    /// <summary>
    /// The DTO used to represent a siteView.
    /// </summary>
    public class SiteViewDto : ViewDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }
    }
}