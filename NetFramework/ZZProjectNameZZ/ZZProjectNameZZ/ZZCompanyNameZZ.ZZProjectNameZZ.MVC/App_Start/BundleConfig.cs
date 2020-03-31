// <copyright file="BundleConfig.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle configuration
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            ScriptBundle bundleProjectJs = new ScriptBundle("~/bundles/ProjectJs");
            bundles.Add(bundleProjectJs);

            StyleBundle bundleProjectCss = new StyleBundle("~/Content/ProjectCss");
            bundleProjectCss.Include("~/Content/Project.css");
            bundles.Add(bundleProjectCss);
        }
    }
}