// <copyright file="SiteInfoDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Dto.Site
{
    using System.Collections.Generic;
    using MyCompany.BIADemo.Domain.Dto.Bia;

    /// <summary>
    /// The DTO used to manage site information.
    /// </summary>
    public class SiteInfoDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of site admin.
        /// </summary>
        public IEnumerable<SiteMemberDto> SiteAdmins { get; set; }
    }
}