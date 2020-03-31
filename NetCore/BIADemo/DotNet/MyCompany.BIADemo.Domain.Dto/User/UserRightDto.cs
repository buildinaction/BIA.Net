// <copyright file="UserRightDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Dto.User
{
    using System.Collections.Generic;

    /// <summary>
    /// The DTO used for rights on user.
    /// </summary>
    public class UserRightDto
    {
        /// <summary>
        /// The user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The user login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// The role list.
        /// </summary>
        public IEnumerable<string> Rights { get; set; }
    }
}