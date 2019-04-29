// <copyright file="HtmlHelpersInput.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

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
    using System.Web.Routing;
    using System.Xml.Linq;

    /// <summary>
    /// HtmlHelpersInput
    /// </summary>
    public static class HtmlHelpersInput
    {
        /// <summary>
        /// Merge 2 dictionnary, can be use to merge htmlAttribut
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static IDictionary<string, object> MergeDict<TModel>(this HtmlHelper<TModel> htmlHelper, object obj1, object obj2)
        {
            var dict1 = new RouteValueDictionary(obj1);
            var dict2 = new RouteValueDictionary(obj2);
            IDictionary<string, object> result = new Dictionary<string, object>();

            foreach (var pair in dict1.Concat(dict2))
            {
                if (result.ContainsKey(pair.Key))
                {
                    result[pair.Key] = result[pair.Key] + " " + pair.Value;
                }
                else
                {
                    result.Add(pair);
                }
            }

            return result;
        }

        /// <summary>
        /// Replace unauthorized char by underscore and first char by Z if not authorize
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="type">The type of the field</param>
        /// <param name="precision">The precision of the field</param>
        /// <returns>escaped string</returns>
        public static string PatternNumber<TModel>(this HtmlHelper<TModel> htmlHelper, string type, int? precision = null)
        {
            string patternNumber = "^\\d*";
            if (type.ToLower().IndexOf("double") == 0 || type.ToLower().IndexOf("decimal") == 0 || type.ToLower().IndexOf("float") == 0)
            {
                patternNumber += "(";
                string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (separator == ".")
                {
                    patternNumber += "\\.";
                }
                else
                {
                    patternNumber += separator;
                }

                patternNumber += "\\d";
                if (precision == null)
                {
                    patternNumber += "*";
                }
                else
                {
                    patternNumber += "{0," + precision + "}";
                }

                patternNumber += ")?";
            }

            patternNumber += "$";
            return patternNumber;
        }

        /// <summary>
        /// Replace unauthorized char by underscore and first char by Z if not authorize
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">The id to secure</param>
        /// <returns>escaped string</returns>
        public static string SafeStringForId<TModel>(this HtmlHelper<TModel> htmlHelper, string id)
        {
            string idWithProtectedFirstChar = Regex.Replace(id, "^[^A-Za-z]", "Z");
            return Regex.Replace(idWithProtectedFirstChar, "[^-A-Za-z0-9_:.]", "_");
        }

        /// <summary>
        /// Set a icon before the field.
        /// This icon is define from the list of glyphicons.
        /// By default the icon is blog
        /// </summary>
        /// <param name="mvcControl">string of the control html</param>
        /// <param name="iconDisplay">Icon name from glyphicons</param>
        /// <returns>Return the control html with an encapsulation with an icon</returns>
        public static MvcHtmlString ToIconified(this MvcHtmlString mvcControl, string iconDisplay = "", string position = "left")
        {
            MvcHtmlString returnvalue = new MvcHtmlString(string.Empty);

            // Define the fontawesome icon
            TagBuilder builderiIcon = new TagBuilder("i");

            // No Icon specify => Set an icon and hidden this one
            if (string.IsNullOrEmpty(iconDisplay))
            {
                builderiIcon.AddCssClass("fas fa-check");
                builderiIcon.Attributes.Add("style", "visibility:hidden");
            }
            else
            {
                builderiIcon.AddCssClass(iconDisplay);
            }

            // Define the parent span for the fontawesome icon
            TagBuilder builderSpan = new TagBuilder("span");
            builderSpan.AddCssClass("input-group-text");

            builderSpan.InnerHtml = builderiIcon.ToString(TagRenderMode.Normal);

            // specify the icon position
            TagBuilder builderPosition = new TagBuilder("div");
            if (position == "left")
            {
                builderPosition.AddCssClass("input-group-prepend");
            }
            else
            {
                builderPosition.AddCssClass("input-group-append");
            }

            builderPosition.InnerHtml = builderSpan.ToString(TagRenderMode.Normal);

            // Define the form-group including the floating label and the principal field
            TagBuilder builderFormGroup = new TagBuilder("div");
            builderFormGroup.AddCssClass("form-group");

            // Define the global div to set the icon and the html control
            TagBuilder builder = new TagBuilder("div");
            builder.AddCssClass("col-md-10");
            builder.AddCssClass("input-group");

            // Retrieve the html control to set the class form-control if not present
            string currentControl = mvcControl.ToString();

            // Create a div for html control which has a simple label or empty
            if (string.IsNullOrEmpty(currentControl) || currentControl.Substring(0, 1) != "<")
            {
                TagBuilder builderDiv = new TagBuilder("div");
                builderDiv.AddCssClass("form-control");
                builderDiv.InnerHtml = currentControl;
                currentControl = builderDiv.ToString(TagRenderMode.Normal);
            }

            // Add a div at the checkbox/radiobutton to have a prettier display of the html control
            else if (currentControl.ToLower().Contains("type=\"checkbox\"") || currentControl.ToLower().Contains("type=\"radio\""))
            {
                TagBuilder builderDiv = new TagBuilder("div");
                builderDiv.AddCssClass("form-control");
                builderDiv.InnerHtml = currentControl;
                currentControl = builderDiv.ToString(TagRenderMode.Normal);
            }

            // Add the class form-control if not present in the current html control
            if (!currentControl.ToLower().Contains("form-control") && currentControl.ToLower().Contains("class=\""))
            {
                currentControl = currentControl.Insert(currentControl.IndexOf("class=\"") + 7, "form-control ");
            }
            else if (!currentControl.ToLower().Contains("class="))
            {
                currentControl = currentControl.Insert(currentControl.IndexOf(" "), " class=\"form-control\" ");
            }

            //Add the floating label
            TagBuilder builderLabel = new TagBuilder("label");
            builderLabel.AddCssClass("bmd-label-floating");
            builderFormGroup.InnerHtml = string.Format("{0}{1}", builderLabel.ToString(TagRenderMode.Normal), currentControl);



            // Add the html control + the icon in the parent div and return the new object
            builder.InnerHtml = string.Format("{0}{1}", builderPosition.ToString(TagRenderMode.Normal), builderFormGroup.ToString(TagRenderMode.Normal));
            //MvcHtmlString.Create(currentControl)
            returnvalue = MvcHtmlString.Create(builder.ToString());
            return returnvalue;
        }

        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Html code with a list of checkboxes</returns>
        /// <exception cref="System.ArgumentNullException">selectList</exception>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var propName = metadata.PropertyName;
            List<int> values = metadata.Model as List<int>;

            if (selectList == null)
            {
                throw new ArgumentNullException("selectList");
            }

            StringBuilder sb = new StringBuilder();

            foreach (SelectListItem info in selectList)
            {
                TagBuilder builder = new TagBuilder("input");
                if (values != null && values.Contains(int.Parse(info.Value)))
                {
                    builder.MergeAttribute("checked", "checked");
                }

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
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="expression">The expression</param>
        /// <param name="selectList">The select list</param>
        /// <param name="htmlAttributes">The HTML attributes</param>
        /// <param name="searchAppearOver">Number of items in the list before the search functionnality is used</param>
        /// <param name="multiselectParameter">parameter for multiselect</param>
        /// <returns>Html code with a list of checkable elements in a dropdownlist</returns>
        public static MvcHtmlString MultiselectFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null, int searchAppearOver = 8, string multiselectParameter = "maxHeight: 200, buttonWidth: '100%'")
        {
            string idSelect = htmlHelper.IdFor(expression).ToString();

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">$('#").Append(idSelect)
                .Append("').multiselect({ buttonClass: 'btn btn-default form-control',")
                .Append("selectAllText : '" + HtmlHelpersTranslate.TranslateString("SelectAll") + "',")
                .Append("filterPlaceholder : '" + HtmlHelpersTranslate.TranslateString("Search") + "',")
                .Append("nonSelectedText : '" + /*HtmlHelpersTranslate.TranslateString("NonSelected") +*/ "',")
                .Append("nSelectedText : '" + HtmlHelpersTranslate.TranslateString("nSelected") + "',")
                .Append("allSelectedText : '" + HtmlHelpersTranslate.TranslateString("AllSelected") + "',")
                .Append(multiselectParameter);
            if (selectList.Count() > searchAppearOver)
            {
                script.Append(", enableCaseInsensitiveFiltering: true");
            }

            script.Append("});</script>");

            htmlHelper.Script(script.ToString(), HtmlHelpersScript.Priority.FastRender);

            return htmlHelper.ListBoxFor(expression, selectList, htmlAttributes);
        }

        /// <summary>
        /// Control that displays a dropdownlist of checkable items
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="expression">The expression</param>
        /// <param name="selectList">The select list</param>
        /// <param name="htmlAttributes">The HTML attributes</param>
        /// <returns>Html code with a list of checkable elements in a dropdownlist</returns>
        public static MvcHtmlString Multiselect2ListsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null)
        {
            string idSelect = htmlHelper.IdFor(expression).ToString();

            htmlHelper.Script(
                @"<script type = ""text/javascript"" > $('#" + idSelect + @"').multiSelect();</script>",
                HtmlHelpersScript.Priority.FastRender);

            return htmlHelper.ListBoxFor(expression, selectList, htmlAttributes);
        }


        public enum CascadingDropDownListType
        {
            None=1,
            ChildList=2
        }

        /// <summary>
        /// Create a dropdownlist whose display will depend on a parent dropdownlist
        /// </summary>
        /// <typeparam name="TModel">The type of the model</typeparam>
        /// <typeparam name="TProperty">The type of the property</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem objects that are used to populate the drop-down list</param>
        /// <param name="optionLabel">The text for a default empty item. This parameter can be null</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element</param>
        /// <param name="listType">automaticaly add the javascript for a type of list</param>
        /// <returns>An HTML select element for each property in the object that is represented by the expression</returns>
        public static MvcHtmlString CascadingDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CascadingSelectListItem> selectList, string optionLabel, object htmlAttributes = null, CascadingDropDownListType listType = CascadingDropDownListType.ChildList)
        {
            MvcHtmlString mvcHtmlString = htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
            string dropDownListId = htmlHelper.IdFor(expression).ToString();
            return CascadingDropDownList(htmlHelper, selectList, optionLabel, mvcHtmlString, dropDownListId, listType);
        }

        /// <summary>
        /// Create a dropdownlist whose display will depend on a parent dropdownlist
        /// </summary>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="name">The name of the form field to return</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem objects that are used to populate the drop-down list</param>
        /// <param name="optionLabel">The text for a default empty item. This parameter can be null</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element</param>
        /// <param name="listType">automaticaly add the javascript for a type of list</param>
        /// <returns>An HTML select element with an option subelement for each item in the list</returns>
        public static MvcHtmlString CascadingDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<CascadingSelectListItem> selectList, string optionLabel, object htmlAttributes = null, CascadingDropDownListType listType = CascadingDropDownListType.ChildList)
        {
            MvcHtmlString mvcHtmlString = htmlHelper.DropDownList(name, selectList, optionLabel, htmlAttributes);
            string dropDownListId = name;
            return CascadingDropDownList(htmlHelper, selectList, optionLabel, mvcHtmlString, dropDownListId, listType);
        }

        /// <summary>
        /// Create a dropdownlist whose display will depend on a parent dropdownlist
        /// </summary>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="selectList">A collection of System.Web.Mvc.SelectListItem objects that are used to populate the drop-down list</param>
        /// <param name="optionLabel">The text for a default empty item. This parameter can be null</param>
        /// <param name="mvcHtmlString">An HTML select element with an option subelement for each item</param>
        /// <param name="dropDownListId">The id of the form field to return</param>
        /// <param name="listType">automaticaly add the javascript for a type of list</param>
        /// <returns>An HTML select element with an option subelement for each item in the list</returns>
        private static MvcHtmlString CascadingDropDownList(HtmlHelper htmlHelper, IEnumerable<CascadingSelectListItem> selectList, string optionLabel, MvcHtmlString mvcHtmlString, string dropDownListId, CascadingDropDownListType listType)
        {
            XDocument selectDoc = XDocument.Parse(mvcHtmlString.ToString());
            string prefixDataAttr = "data-";
            string prefixIdAttr = "id-";
            string separator = "|";

            List<XElement> options = selectDoc.Element("select").Descendants().ToList();

            if (options != null && options.Any())
            {
                foreach (XElement option in options)
                {

                    CascadingSelectListItem cascadingItem = null;

                    if (option.Attributes("value").FirstOrDefault() != null)
                    {
                        cascadingItem = selectList.FirstOrDefault(x => x.Value == option.Attributes("value").First().Value);
                    }

                    if (cascadingItem != null)
                    {
                        foreach (var parentValue in cascadingItem.ParentValues)
                        {
                            option.SetAttributeValue(prefixIdAttr + parentValue.Key, parentValue.Key);
                            option.SetAttributeValue(prefixDataAttr + parentValue.Key, string.Join(separator, parentValue.Value));
                        }
                    }
                    else if (option.Value == optionLabel)
                    {
                        List<string> parentIds = selectList.SelectMany(x => x.ParentValues.Select(y => y.Key)).Distinct().ToList();
                        if (parentIds != null && parentIds.Any())
                        {
                            parentIds.Sort();
                            foreach (var parentId in parentIds)
                            {
                                List<string> parentValues = selectList.SelectMany(x => x.ParentValues.Where(z => z.Key == parentId).SelectMany(y => y.Value)).Distinct().ToList();
                                if (parentValues != null && parentValues.Any())
                                {
                                    parentValues.Sort();
                                    option.SetAttributeValue(prefixIdAttr + parentId, parentId);
                                    option.SetAttributeValue(prefixDataAttr + parentId, string.Join(separator, parentValues));
                                }
                            }
                        }
                    }
                }
            }

            if (listType == CascadingDropDownListType.ChildList)
            {
                string javascript = "<script type=\"text/javascript\">$(document).ready(function() { $(\"#" + dropDownListId + "\").cascadingDropDownList(); });</script>";
                htmlHelper.Script(javascript, HtmlHelpersScript.Priority.FastRender);
            }

            return MvcHtmlString.Create(selectDoc.ToString());
        }
    }
}