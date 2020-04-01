// <copyright file="Permission.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Crosscutting.Common.Configuration.BiaNet
{
    using System.Collections.Generic;

    /// <summary>
    /// The permission configuration.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets or sets the name of the permission.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the roles associated to the permission.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}