// <copyright file="Constants.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Crosscutting.Common
{
    /// <summary>
    /// The class containing all constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The application constants.
        /// </summary>
        public static class Application
        {
            /// <summary>
            /// The back end version.
            /// </summary>
            public const string BackEndVersion = "1.3.0";

            /// <summary>
            /// The front end version.
            /// </summary>
            public const string FrontEndVersion = "1.3.0";

            /// <summary>
            /// The framework version.
            /// </summary>
            public const string FrameworkVersion = "3.3.0";
        }

        /// <summary>
        /// The default values.
        /// </summary>
        public static class DefaultValues
        {
            /// <summary>
            /// The default theme.
            /// </summary>
            public const string Theme = "Light";

            /// <summary>
            /// The default language.
            /// </summary>
            public const string Language = "en-US";
        }

        /// <summary>
        /// Role.
        /// </summary>
        public static class Role
        {
            /// <summary>
            /// The name of the user role.
            /// </summary>
            public const string User = "User";
        }
    }
}