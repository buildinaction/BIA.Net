// <copyright file="ZZTemplateModel.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Template.Models
{
    using Base;
    using Common;

    /// <summary>
    /// ZZTemplateModel sample class
    /// </summary>
    public class ZZTemplateModel : TemplateModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZZTemplateModel"/> class.
        /// </summary>
        public ZZTemplateModel()
            : base(Constants.EnumLanguage.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZZTemplateModel"/> class.
        /// </summary>
        /// <param name="language">the language of the template</param>
        public ZZTemplateModel(Constants.EnumLanguage language)
            : base(language)
        {
        }

        /// <summary>
        /// Gets or sets param1 (sample)
        /// </summary>
        public string Param1 { get; set; }

        /// <summary>
        /// Gets or sets param2 (sample)
        /// </summary>
        public string Param2 { get; set; }

        /// <summary>
        /// Gets or sets param3 (sample)
        /// </summary>
        public string Param3 { get; set; }

        /// <summary>
        /// Gets the class name.
        /// </summary>
        internal override string Key
        {
            get
            {
                return typeof(ZZTemplateModel).Name;
            }
        }
    }
}