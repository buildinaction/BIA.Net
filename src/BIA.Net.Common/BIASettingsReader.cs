// <copyright file="SafranSettingsReader.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// AppSettings Reader
    /// </summary>
    public static class BIASettingsReader
    {
        private static Dictionary<string, string> dialogLayouts = null;

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<string, string> DialogLayouts
        {
            get
            {
                if (dialogLayouts == null)
                {
                    dialogLayouts = new Dictionary<string, string>();
                    BIANetSection section = (BIANetSection)ConfigurationManager.GetSection("BiaNet");
                    LayoutsCollection layouts = section?.Dialog?.Layouts;
                    if (layouts != null)
                    {
                        foreach (LayoutElement layout in layouts)
                        {
                            dialogLayouts.Add(layout.Name, layout.Path);
                        }
                    }
                }
                return dialogLayouts;
            }
        }

        /// <summary>
        /// Gets AD Groups As Application Users
        /// </summary>
        public static string GetDialogLayout(string name)
        {
            string value = null;
            if (DialogLayouts.TryGetValue(name, out value))
            {
                return value;
            }

            return null;
        }
    }
}