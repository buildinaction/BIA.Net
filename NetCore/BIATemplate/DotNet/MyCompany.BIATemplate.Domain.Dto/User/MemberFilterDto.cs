// <copyright file="MemberFilterDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Dto.User
{
    using MyCompany.BIATemplate.Domain.Dto.Bia;

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