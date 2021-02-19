// <copyright file="MemberFilterDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.User
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