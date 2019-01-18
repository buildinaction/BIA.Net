// <copyright file="LanguageInfo.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class used for the multi-language change
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>
        /// Dictionary containing the languages the application is translatable into
        /// </summary>
        public static readonly Dictionary<string, LanguageInfo> LanguageInfoDictionary = new Dictionary<string, LanguageInfo>
        {
            { "DeutschDE", new LanguageInfo { Name = "Deutsch DE", ShortName = "DE", Code = "de-DE" } },
            { "EnglishUS", new LanguageInfo { Name = "English US", ShortName = "EN US", Code = "en-US" } },
            { "EnglishGB", new LanguageInfo { Name = "English GB", ShortName = "EN GB", Code = "en-GB" } },
            { "Français", new LanguageInfo { Name = "Français", ShortName = "FR", Code = "fr-FR" } },
            { "Español", new LanguageInfo { Name = "Español", ShortName = "ES", Code = "es-ES" } }
        };

        /// <summary>
        /// Gets or sets name of the language
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets shortname of the language that is displayed in the menu
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets cultureInfo code of the language
        /// </summary>
        public string Code { get; set; }
    }
}