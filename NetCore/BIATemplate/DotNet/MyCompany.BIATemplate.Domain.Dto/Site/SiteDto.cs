// <copyright file="SiteDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Dto.Site
{
    using MyCompany.BIATemplate.Domain.Dto.Bia;

    /// <summary>
    /// The DTO used to manage site.
    /// </summary>
    public class SiteDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }
    }
}