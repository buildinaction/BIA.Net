// <copyright file="Constants.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$
{
    /// <summary>
    /// Application contants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The version number
        /// </summary>
        public const string Version = "1.0.0";

        /// <summary>
        /// The FrameworkVersion number
        /// </summary>
        public const string FrameworkVersion = "2.1.1";

        /// <summary>
        /// The ad group of application administrators
        /// </summary>
        public const string RoleAdmin = "Admin";

        /// <summary>
        /// The ad group of application administrators
        /// </summary>
        public const string RoleService = "Service";

        /// <summary>
        /// The ad group of application administrators
        /// </summary>
        public const string RoleInternal = "Internal";

        /// <summary>
        /// The custom group of site administrators
        /// </summary>
        public const string RoleSiteAdmin = "SiteAdmin";

        /// <summary>
        /// The custom group of site members
        /// </summary>
        public const string RoleSiteMember = "SiteMember";

        /// <summary>
        /// The member role site admin Id
        /// </summary>
        public const int RoleSiteAdminId = 1;

        /// <summary>
        /// Right for screen
        /// </summary>
        public enum Right
        {
            /// <summary>
            /// The forbidden : cannot access
            /// </summary>
            Forbidden,

            /// <summary>
            /// The read : read only
            /// </summary>
            Read,

            /// <summary>
            /// The edit : full control
            /// </summary>
            Edit
        }
    }
}