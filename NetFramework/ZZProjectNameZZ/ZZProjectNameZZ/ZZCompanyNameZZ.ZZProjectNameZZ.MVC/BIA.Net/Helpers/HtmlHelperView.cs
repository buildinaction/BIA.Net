namespace BIA.Net.Helpers
{
    using BIA.Net.Business.DTO;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Helper for views
    /// </summary>
    public static class HtmlHelperView
    {
        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="tableId">The table Id.</param>
        /// <returns>return the script string to </returns>
        public static MvcHtmlString ScriptViewToApply(this HtmlHelper htmlHelper, string tableId)
        {
            ViewDTO viewToApplied = null;
            if (htmlHelper.ViewBag.ViewApplied != null)
            {
                Dictionary<string, ViewDTO> allViewApplied = htmlHelper.ViewBag.ViewApplied;
                allViewApplied.TryGetValue(tableId, out viewToApplied);
            }

            if (viewToApplied != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type=\"text/javascript\">")
                    .Append("BIA.Net.View.ViewApplied(\"").Append(tableId).Append("\", {")
                    .Append("viewId:").Append(viewToApplied.Id).Append(",")
                    .Append("preference:").Append(!string.IsNullOrEmpty(viewToApplied.Preference) ? viewToApplied.Preference : "{}").Append(",")
                    .Append(" });")
                    .Append("</script>");
                return new MvcHtmlString(sb.ToString());
            }

            return new MvcHtmlString(string.Empty);
        }
    }
}