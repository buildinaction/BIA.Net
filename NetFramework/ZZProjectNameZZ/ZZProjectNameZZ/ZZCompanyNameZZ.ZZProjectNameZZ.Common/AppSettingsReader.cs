// <copyright file="AppSettingsReader.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Common
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
        /// Gets Project title
        /// </summary>
        public static string ProjectTitle
        {
            get
            {
                return ConfigurationManager.AppSettings["ProjectTitle"];
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
                int.TryParse(ConfigurationManager.AppSettings["intervalSync"], out int ret);
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
                int.TryParse(ConfigurationManager.AppSettings["PathApplicationShareFolder"], out int ret);
                return ret;
            }
        }
    }
}