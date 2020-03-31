// <copyright file="Authentication.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Crosscutting.Common.Configuration.BiaNet
{
    /// <summary>
    /// The authentication configuration.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// The AD domain.
        /// </summary>
        public string ADDomain { get; set; }

        /// <summary>
        /// The refresh mode of AD roles.
        /// </summary>
        public string ADRolesMode { get; set; }
    }
}