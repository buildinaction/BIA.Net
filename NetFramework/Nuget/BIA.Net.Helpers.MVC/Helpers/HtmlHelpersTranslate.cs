// <copyright file="HtmlHelpersTranslate.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// HtmlHelpers for Translation
    /// </summary>
    public static class HtmlHelpersTranslate
    {
        /// <summary>
        /// Translate and plurializes the specified singular expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="singularExpression">The singular expression.</param>
        /// <returns>Translated and plurilized expression</returns>
        public static MvcHtmlString TranslateAndPlurialize<TModel>(this HtmlHelper<TModel> html, string singularExpression)
        {
            string plurializedExpression = TranslateString(singularExpression + "s");
            if (string.IsNullOrEmpty(plurializedExpression))
            {
                var translatedExpression = TranslateString(singularExpression);
                if (!string.IsNullOrEmpty(translatedExpression))
                {
                    plurializedExpression = translatedExpression + "s";
                }
            }

            if (string.IsNullOrEmpty(plurializedExpression))
            {
                plurializedExpression = singularExpression + "s";
            }

            return new MvcHtmlString(plurializedExpression);
        }

        /// <summary>
        /// Translates the specified singular expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="singularExpression">The singular expression.</param>
        /// <returns>Translated expression</returns>
        public static MvcHtmlString Translate<TModel>(this HtmlHelper<TModel> html, string singularExpression)
        {
            var translatedExpression = TranslateString(singularExpression);
            if (string.IsNullOrEmpty(translatedExpression))
            {
                translatedExpression = singularExpression;
            }

            return new MvcHtmlString(translatedExpression);
        }

        /// <summary>
        /// LabelFor translated.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>teh name translated</returns>
        public static MvcHtmlString T_LabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return html.Label(html.T_DisplayNameFor(expression).ToString(), htmlAttributes);
        }

        /// <summary>
        /// LabelFor translated.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>teh name translated</returns>
        public static MvcHtmlString T_Label<TModel>(this HtmlHelper<TModel> html,  string expression, object htmlAttributes)
        {
            return html.Label(TranslateStringOrOriginal(expression), htmlAttributes);
        }
        /// <summary>
        /// Display name for translated.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The Display name for translated</returns>
        public static MvcHtmlString T_DisplayNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {
            return TranslateMvcHtmlString(html.DisplayNameFor(expression));
        }

        /// <summary>
        /// Display name for translated.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>The Display name for translated</returns>
        public static MvcHtmlString T_DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return TranslateMvcHtmlString(html.DisplayNameFor(expression));
        }

        /// <summary>
        /// Translates a string.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <returns>string translated</returns>
        private static MvcHtmlString TranslateMvcHtmlString(MvcHtmlString origin)
        {
            string originString = origin.ToString();
            string translated = TranslateString(originString);

            if (string.IsNullOrEmpty(translated))
            {
                return origin;
            }
            else
            {
                return new MvcHtmlString(translated);
            }
        }

        static List<Type> resourceTypes = null;

        public static void InitResources(List<Type> lResourceTypes)
        {
            resourceTypes = lResourceTypes;
        }

        /// <summary>
        /// Translates the string.
        /// </summary>
        /// <param name="originString">The origin string.</param>
        /// <returns>the translated string</returns>
        public static string TranslateStringOrOriginal(string originString)
        {
            string translated = TranslateString(originString);

            if (string.IsNullOrEmpty(translated))
            {
                return originString;
            }
            else
            {
                return translated;
            }
        }

        /// <summary>
        /// Translates the string.
        /// </summary>
        /// <param name="originString">The origin string.</param>
        /// <returns>the translated string</returns>
        public static string TranslateString(string originString)
        {
            string translated = null;
            foreach (Type ressource in resourceTypes)
            {
                translated = TranslateWithResx(originString, ressource);
                if (!string.IsNullOrEmpty(translated)) break;
            }
            /*
            string translated = TranslateWithResx(originString, typeof(TextResources));
            if (string.IsNullOrEmpty(translated))
            {
                translated = TranslateWithResx(originString, typeof(AppliResources));
            }*/

            return translated;
        }

        /// <summary>
        /// Translates a string with RESX.
        /// </summary>
        /// <param name="originString">The origin string.</param>
        /// <param name="resxType">Type of the RESX.</param>
        /// <returns>the string translated with RESX</returns>
        private static string TranslateWithResx(string originString, Type resxType)
        {
            string translated = new System.Resources.ResourceManager(resxType).GetString(originString);
            if (string.IsNullOrEmpty(translated))
            {
                if (originString.Substring(originString.Length - 1, 1) == "s")
                {
                    translated = new System.Resources.ResourceManager(resxType).GetString(originString.Substring(0, originString.Length - 1));
                    if (!string.IsNullOrEmpty(translated))
                    {
                        translated = translated + "s";
                    }
                }
            }

            return translated;
        }
    }
}