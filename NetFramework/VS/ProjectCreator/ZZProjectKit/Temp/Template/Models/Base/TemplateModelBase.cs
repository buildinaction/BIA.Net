// <copyright file="TemplateModelBase.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$.Models.Base
{
    using BIA.Net.Common;
    using System.IO;
    using System.Linq;
    using static Common.Constants;

    /// <summary>
    /// Base template for ViewModel mail template
    /// </summary>
    public abstract class TemplateModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateModelBase"/> class.
        /// </summary>
        /// <param name="language">the language of the template</param>
        public TemplateModelBase(EnumLanguage language)
        {
            this.Language = language;

            DirectoryInfo dirInfo = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).GetDirectories("Templates", SearchOption.AllDirectories).FirstOrDefault();

            if (dirInfo != null)
            {
                TraceManager.Debug(ClassName, "TemplateModelBase", "dirInfo.FullName: " + dirInfo.FullName);

                FileInfo fileInfo = dirInfo.GetFiles(this.TemplateName + ".cshtml", SearchOption.AllDirectories).FirstOrDefault();

                if (fileInfo != null)
                {
                    this.TemplatePath = fileInfo.FullName;
                }
                else
                {
                    TraceManager.Error(ClassName, "TemplateModelBase", "file '" + this.TemplateName + ".cshtml' not found");
                }
            }
            else
            {
                TraceManager.Error(ClassName, "TemplateModelBase", "directory 'Templates' not found");
            }
        }

        /// <summary>
        /// Gets enum Language
        /// </summary>
        internal EnumLanguage Language { get; private set; }

        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        internal string TemplatePath { get; private set; }

        /// <summary>
        /// Gets the child class name.
        /// </summary>
        internal abstract string Key { get; }

        /// <summary>
        /// Gets the template name.
        /// </summary>
        internal virtual string TemplateName
        {
            get
            {
                string templateName = this.Key.Substring(0, this.Key.Length - "Model".Length);

                if (this.Language == EnumLanguage.English)
                {
                    templateName += "_EN";
                }
                else if (this.Language == EnumLanguage.Spanish)
                {
                    templateName += "_ES";
                }
                else if (this.Language == EnumLanguage.French)
                {
                    templateName += "_FR";
                }

                return templateName;
            }
        }

        /// <summary>
        /// Gets class Name
        /// </summary>
        private static string ClassName
        {
            get { return typeof(TemplateModelBase).Name; }
        }
    }
}