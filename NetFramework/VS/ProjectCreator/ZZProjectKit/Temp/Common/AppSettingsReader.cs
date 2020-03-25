// <copyright file="AppSettingsReader.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// AppSettings Reader
    /// </summary>
    public static class AppSettingsReader
    {
        /// <summary>
        /// Store the value to display in log to avoid using real one for security reason.
        /// </summary>
        private const string AnonymizedValue = "********";

        /// <summary>
        /// Gets AD Domains
        /// </summary>
        public static List<string> ProjectTitle
        {
            get
            {
                string value = ConfigurationManager.AppSettings["ProjectTitle"];
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }

                return value.Split(',').ToList();
            }
        }

        /// <summary>
        /// Gets the WindowsService Name
        /// </summary>
        public static string WindowsServiceName
        {
            get
            {
                return ConfigurationManager.AppSettings["WindowsServiceName"];
            }
        }

        /// <summary>
        /// Gets the intervalSync
        /// </summary>
        public static int IntervalSync
        {
            get
            {
                int ret = default(int);
                int.TryParse(ConfigurationManager.AppSettings["intervalSync"], out ret);
                return ret;
            }
        }

        /// <summary>
        /// Gets the intervalSync
        /// </summary>
        public static int PathApplicationShareFolder
        {
            get
            {
                int ret = default(int);
                int.TryParse(ConfigurationManager.AppSettings["PathApplicationShareFolder"], out ret);
                return ret;
            }
        }
    }
}