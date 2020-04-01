// <copyright file="MemberRoleDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Dto.User
{
    using MyCompany.BIATemplate.Domain.Dto.Bia;

    /// <summary>
    /// The DTO used for member roles.
    /// </summary>
    public class MemberRoleDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}