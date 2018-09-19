// <copyright file="BIAConstants.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Common
{
    public class BIAConstants
    {
        /// <summary>
        /// The role Internal user (other digital manufacturing app pool) of the application
        /// </summary>
        public const string RoleInternal = "Internal";

        /// <summary>
        /// The role User of the application
        /// </summary>
        public const string RoleUser = "User";

        /// <summary>
        /// The theme by default
        /// </summary>
        public const string ThemeDefault = "Light";

        /// <summary>
        /// Themes for design
        /// </summary>
        public enum Theme
        {
            /// <summary>
            /// Theme Light
            /// </summary>
            Light,

            /// <summary>
            /// Theme Dark
            /// </summary>
            Dark
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public virtual string Version { get { return "1.0.0"; } }
    }
}
