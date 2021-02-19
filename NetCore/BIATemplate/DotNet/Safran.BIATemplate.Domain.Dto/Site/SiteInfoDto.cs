// <copyright file="SiteInfoDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.Site
{
    using System.Collections.Generic;
    using BIA.Net.Core.Domain.Dto.Base;

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