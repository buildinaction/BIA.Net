// <copyright file="MemberRole.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.UserModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity member role.
    /// </summary>
    public class MemberRole : VersionedTable
    {
        /// <summary>
        /// Gets or sets the member.
        /// </summary>
        public virtual Member Member { get; set; }

        /// <summary>
        /// Gets or sets the member id.
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}