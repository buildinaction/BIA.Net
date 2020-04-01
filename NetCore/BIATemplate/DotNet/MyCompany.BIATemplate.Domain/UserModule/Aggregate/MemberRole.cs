// <copyright file="MemberRole.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.UserModule.Aggregate
{
    /// <summary>
    /// The entity member role.
    /// </summary>
    public class MemberRole
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