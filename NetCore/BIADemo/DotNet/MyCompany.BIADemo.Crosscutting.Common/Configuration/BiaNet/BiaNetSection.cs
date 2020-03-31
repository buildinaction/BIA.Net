// <copyright file="BiaNetSection.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Crosscutting.Common.Configuration.BiaNet
{
    using System.Collections.Generic;

    /// <summary>
    /// The BIA Net section.
    /// </summary>
    public class BiaNetSection
    {
        /// <summary>
        /// Gets or sets the authentication configuration.
        /// </summary>
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets the Roles configuration.
        /// </summary>
        public IEnumerable<Roles> Roles { get; set; }

        /// <summary>
        /// Gets or sets the permissions configuration.
        /// </summary>
        public IEnumerable<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the user profile configuration.
        /// </summary>
        public UserProfile UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the languages configuration.
        /// </summary>
        public IEnumerable<Language> Languages { get; set; }
    }
}