namespace BIA.Net.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Xml.Linq;
    public static class HtmlHelpersFilter
    {
        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propName">The propName.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Html code with a list of checkboxes</returns>
        /// <exception cref="System.ArgumentNullException">selectList</exception>
        public static MvcHtmlString CheckBoxListFilter(this HtmlHelper htmlHelper, string propName, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {

            if (selectList == null)
            {
                throw new ArgumentNullException("selectList");
            }

            StringBuilder sb = new StringBuilder();

            foreach (SelectListItem info in selectList)
            {
                TagBuilder builder = new TagBuilder("input");

                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", propName);
                builder.InnerHtml = info.Text;
                sb.Append(builder.ToString(TagRenderMode.Normal));
                sb.Append("<br />");
            }

            return new MvcHtmlString(sb.ToString());
        }
        /// <summary>
        /// Control that displays a dropdownlist of checkable items
        /// </summary>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="idSelect">The idSelect</param>
        /// <param name="selectList">The select list</param>
        /// <param name="htmlAttributes">The HTML attributes</param>
        /// <param name="searchAppearOver">Number of items in the list before the search functionnality is used</param>
        /// <param name="multiselectParameter">parameter for multiselect</param>
        /// <returns>Html code with a list of checkable elements in a dropdownlist</returns>
        public static MvcHtmlString MultiselectFilter(this HtmlHelper htmlHelper, string idSelect, IEnumerable<SelectListItem> selectList, object htmlAttributes = null, int searchAppearOver = 8, string multiselectParameter = "maxHeight: 200, buttonWidth: '100%'")
        {

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">$('#").Append(idSelect)
                .Append("').multiselect({ ")
                .Append("selectAllText : '" + HtmlHelpersTranslate.TranslateString("SelectAll") + "',")
                .Append("filterPlaceholder : '" + HtmlHelpersTranslate.TranslateString("Search") + "',")
                .Append("nonSelectedText : '" + HtmlHelpersTranslate.TranslateString("NonSelected") + "',")
                .Append("nSelectedText : '" + HtmlHelpersTranslate.TranslateString("nSelected") + "',")
                .Append("allSelectedText : '" + HtmlHelpersTranslate.TranslateString("AllSelected") + "',")
                .Append(multiselectParameter);
            if (selectList.Count() > searchAppearOver)
            {
                script.Append(", enableCaseInsensitiveFiltering: true");
            }

            script.Append("});</script>");

            htmlHelper.Script(script.ToString(), HtmlHelpersScript.Priority.FastRender);

            return htmlHelper.ListBox(idSelect, selectList, htmlAttributes);
        }
    }
}