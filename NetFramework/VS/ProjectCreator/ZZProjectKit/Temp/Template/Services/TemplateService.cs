// <copyright file="TemplateService.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace $safeprojectname$.Services
{
    using BIA.Net.Common;
    using Internal;
    using Models.Base;
    using Rotativa;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// TemplateService provide fonction to generate Html and PDF
    /// </summary>
    public static class TemplateService
    {
        /// <summary>
        /// Gets class Name
        /// </summary>
        private static string ClassName
        {
            get { return typeof(TemplateService).Name; }
        }

        /// <summary>
        /// Generate an html file
        /// </summary>
        /// <param name="model">parametre of generation</param>
        /// <returns>A html created with a templet</returns>
        public static string GetHtml(TemplateModelBase model)
        {
            return new RazorTemplate().FillTemplate(model.TemplatePath, model);
        }

        /// <summary>
        /// Generate a PDF file
        /// </summary>
        /// <param name="model">parametre of generation</param>
        /// <returns>a PDF file</returns>
        public static byte[] GetPdf(TemplateModelBase model)
        {
            string html = new RazorTemplate().FillTemplate(model.TemplatePath, model);
            return ConvertHtmlToPdf(html);
        }

        /// <summary>
        /// Convert html to pdf.
        /// </summary>
        /// <param name="html">html code</param>
        /// <returns>Pdf in binary format</returns>
        public static byte[] ConvertHtmlToPdf(string html)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            byte[] pdf = null;

            if (!string.IsNullOrWhiteSpace(html))
            {
                DirectoryInfo dir = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).Parent.GetDirectories("Rotativa", SearchOption.AllDirectories).FirstOrDefault();
                if (dir != null)
                {
                    pdf = WkhtmltopdfDriver.ConvertHtml(dir.FullName, null, html);
                    TraceManager.Debug(ClassName, methodName, "ConvertHtmlToPdf OK");
                }
                else
                {
                    TraceManager.Warn(ClassName, methodName, "directory 'Rotativa' not found");
                }
            }
            else
            {
                TraceManager.Warn(ClassName, methodName, "html parameter is null or empty");
            }

            return pdf;
        }
    }
}