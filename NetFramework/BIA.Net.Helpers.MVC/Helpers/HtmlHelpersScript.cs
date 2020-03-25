namespace BIA.Net.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    /// <summary>
    /// Helpers to place script at bottom of the page
    /// </summary>
    public static class HtmlHelpersScript
    {
        /// <summary>
        /// Prority of script insertion.
        /// </summary>
        public enum Priority
        {
            /// <summary>
            /// higher priority to use only for script that impact render and run fast
            /// </summary>
            FastRender = 1,

            /// <summary>
            /// The hight priority script
            /// </summary>
            HightScript = 2,

            /// <summary>
            /// The medium priority script to run after
            /// </summary>
            MediumScript = 3,

            /// <summary>
            /// The low priority script to run at the end
            /// </summary>
            LowPriority = 4,
        }

        /// <summary>
        /// Memorize a script to render at the bottom of the page.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="template">The template.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>Empty</returns>
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template, Priority priority = Priority.MediumScript)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + priority + "_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Memorize a script to render at the bottom of the page.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="template">The template.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>Empty</returns>
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, string template, Priority priority = Priority.MediumScript)
        {
            htmlHelper.ViewContext.HttpContext.Items["_scriptString_" + priority + "_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Renders the scripts (to be called on time only from layout).
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>Empty</returns>
        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
            {
                foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
                {
                    if (key.ToString().StartsWith("_script_" + priority + "_"))
                    {
                        var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                        if (template != null)
                        {
                            htmlHelper.ViewContext.Writer.Write(template(null));
                        }
                    }

                    if (key.ToString().StartsWith("_scriptString_" + priority + "_"))
                    {
                        var template = htmlHelper.ViewContext.HttpContext.Items[key] as string;
                        if (template != null)
                        {
                            htmlHelper.ViewContext.Writer.Write(template);
                        }
                    }
                }
            }

            return MvcHtmlString.Empty;
        }
    }
}