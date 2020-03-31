// <copyright file="ADRolesMode.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Crosscutting.Common.Enum
{
    /// <summary>
    /// The enumeration defining the different modes to search user roles.
    /// </summary>
    public enum ADRolesMode
    {
        /// <summary>
        /// Search roles only in IIS groups.
        /// </summary>
        IisGroup = 1,

        /// <summary>
        /// Search user in AD to find roles.
        /// </summary>
        ADUserFirst = 2,

        /// <summary>
        /// Check on each group if user is in it.
        /// </summary>
        ADGroupFirst = 3,
    }
}