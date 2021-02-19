// <copyright file="SiteFilterDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.Site
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The site filter DTO.
    /// </summary>
    public class SiteFilterDto : LazyLoadDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}