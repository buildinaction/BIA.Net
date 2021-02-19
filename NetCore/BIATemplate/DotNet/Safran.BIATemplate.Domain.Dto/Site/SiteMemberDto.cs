// <copyright file="SiteMemberDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.Site
{
    /// <summary>
    /// The DTO used to manage site member.
    /// </summary>
    public class SiteMemberDto
    {
        /// <summary>
        /// Gets or sets the user first name.
        /// </summary>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Gets or sets the user last name.
        /// </summary>
        public string UserLastName { get; set; }

        /// <summary>
        /// Gets or sets the user login.
        /// </summary>
        public string UserLogin { get; set; }
    }
}