// <copyright file="Roles.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Crosscutting.Common.Configuration.BiaNet
{
    /// <summary>
    /// The roles configuration.
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Gets or sets the type of the role (Fake or AD).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the role.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the label of the role.
        /// </summary>
        public string Label { get; set; }
    }
}