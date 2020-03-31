// <copyright file="Constants.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Crosscutting.Common
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
            /// The current version of the application.
            /// </summary>
            public const string Version = "1.0.0";

            /// <summary>
            /// The framework version.
            /// </summary>
            public const string FrameworkVersion = "3.0.0";
        }

        /// <summary>
        /// The class containing HTTP headers constants.
        /// </summary>
        public static class HttpHeaders
        {
            /// <summary>
            /// The total count returned on getAll methods.
            /// </summary>
            public const string TotalCount = "X-Total-Count";
        }

        /// <summary>
        /// The class containing JWT constants.
        /// </summary>
        public static class Jwt
        {
            /// <summary>
            /// The user roles.
            /// </summary>
            public const string Roles = "roles";

            /// <summary>
            /// The user identifier.
            /// </summary>
            public const string Id = "id";
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
        /// CSV parameters.
        /// </summary>
        public static class Csv
        {
            /// <summary>
            /// the separator for a csv file.
            /// </summary>
            public const string Separator = ",";

            /// <summary>
            /// the extension of a csv file.
            /// </summary>
            public const string Extension = ".csv";

            /// <summary>
            /// the extension of a csv file.
            /// </summary>
            public const string ContentType = "text/csv";
        }
    }
}