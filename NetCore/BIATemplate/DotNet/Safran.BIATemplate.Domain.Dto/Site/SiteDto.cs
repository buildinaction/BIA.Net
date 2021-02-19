// <copyright file="SiteDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.Site
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to manage site.
    /// </summary>
    public class SiteDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is the default one.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}