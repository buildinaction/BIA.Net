namespace BIA.Net.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Generation of Breadcrumb
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Get beradcrumb content
        /// </summary>
        /// <returns>html breadcrumb content</returns>
        public static string GetBreadcrumbContent()
        {
            SiteMapProvider siteMapProvider = System.Web.SiteMap.Provider;
            List<string> breadcrumbpath = new List<string>();

            if (siteMapProvider != null)
            {
                var node = siteMapProvider.CurrentNode;

                /* Treatment currentNode and foreach for parents */

                do
                {
                    string title = string.Empty;
                    if (node != null)
                    {
                        title = TranslateTitle(node.Title);

                        if (node == siteMapProvider.CurrentNode)
                        {
                            breadcrumbpath.Add("<div class='title-actual-page'>" + title + "</div>");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(node.Url))
                            {
                                breadcrumbpath.Add(title);
                            }
                            else
                            {
                                breadcrumbpath.Add("<a href='" + node.Url + "' BIADialogLinkToContent='DivContent:#BiaNetMainPageContent' >" + title + "</a>");
                            }
                        }

                        node = node.ParentNode;
                    }
                } while (node != null);
            }

            breadcrumbpath.Reverse();
            string breadcrumbContent = string.Empty;
            foreach (var bc in breadcrumbpath)
            {
                if (bc != breadcrumbpath.First())
                {
                    breadcrumbContent += " > ";
                }

                breadcrumbContent += bc;
            }

            return breadcrumbContent;
        }

        private static string TranslateTitle(string  nodeTitle)
        {
            var title = "";
            if (nodeTitle.IndexOf("|") > 0)
            {
                var splited = nodeTitle.Split('|');
                foreach (var partTitle in splited)
                {
                    if (string.IsNullOrEmpty(partTitle))
                    {
                        title += '|';
                    }
                    else
                    {
                        title += HtmlHelpersTranslate.TranslateString(partTitle);
                    }
                }
            }

            if (string.IsNullOrEmpty(title))
            {
                title = HtmlHelpersTranslate.TranslateString(nodeTitle);
            }

            return title;
        }
    }
}