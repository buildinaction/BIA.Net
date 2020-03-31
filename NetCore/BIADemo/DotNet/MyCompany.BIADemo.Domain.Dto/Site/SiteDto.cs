// <copyright file="SiteDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Dto.Site
{
    using MyCompany.BIADemo.Domain.Dto.Bia;

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