// <copyright file="LanguageInfo.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Common
{
    using System.Collections.Generic;

    /// <summary>
    /// The language information.
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>
        /// The dictionnary of all languages available.
        /// </summary>
        public static readonly IEnumerable<LanguageInfo> AllLanguageInfos = BIASettingsReader.BIANetSection.Language.GetApplicationLanguages();

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the language name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the language short name.
        /// </summary>
        public string ShortName { get; set; }
    }
}
