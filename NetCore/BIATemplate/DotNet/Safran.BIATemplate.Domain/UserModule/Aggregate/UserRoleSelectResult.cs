// <copyright file="UserRoleSelectResult.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.UserModule.Aggregate
{
    using System.Collections.Generic;

    /// <summary>
    /// The select result used to get user role.
    /// </summary>
    public class UserRoleSelectResult
    {
        /// <summary>
        /// The user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The list of roles.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}