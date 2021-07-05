// <copyright file="MemberFilterDto.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIATemplate.Domain.Dto.User
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The member filter DTO.
    /// </summary>
    public class MemberFilterDto : LazyLoadDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }
    }
}