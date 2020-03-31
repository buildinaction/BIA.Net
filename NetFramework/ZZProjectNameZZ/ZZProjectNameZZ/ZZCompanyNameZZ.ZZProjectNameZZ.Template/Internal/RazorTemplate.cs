// <copyright file="RazorTemplate.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Template.Internal
{
    using BIA.Net.Common;
    using Models.Base;
    using RazorEngine;
    using RazorEngine.Templating;
    using System;

    /// <summary>
    /// Create a template based on a Razor file
    /// </summary>
    internal class RazorTemplate
    {
        /// <summary>
        /// Fill the template with the model
        /// </summary>
        /// <param name="template">Template path</param>
        /// <param name="model">the entity</param>
        /// <returns>the template filled with model</returns>
        internal string FillTemplate(string template, TemplateModelBase model)
        {
            string result = null;
            string templateAsString = null;
            try
            {
                templateAsString = System.IO.File.ReadAllText(template);

                result = Engine.Razor.RunCompile(
                    new LoadedTemplateSource(templateAsString, template),
                    model.Key + "-" + model.Language,
                    null,
                    model);
            }
            catch (Exception e)
            {
                TraceManager.Error("Error durring template acces", e);
            }

            return result;
        }
    }
}