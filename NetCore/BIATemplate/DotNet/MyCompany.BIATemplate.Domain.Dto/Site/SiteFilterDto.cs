// <copyright file="SiteFilterDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Dto.Site
{
    using MyCompany.BIATemplate.Domain.Dto.Bia;

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