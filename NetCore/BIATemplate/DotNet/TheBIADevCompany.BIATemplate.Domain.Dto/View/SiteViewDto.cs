// <copyright file="SiteViewDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIATemplate.Domain.Dto.SiteView
{
    using TheBIADevCompany.BIATemplate.Domain.Dto.View;

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