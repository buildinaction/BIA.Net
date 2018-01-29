using System.Globalization;
using System.Threading;
using System.Web;

namespace BIA.Net.Web.Utility
{
    /// <summary>
    /// Helper to get set the culture of the current thread
    /// </summary>
    public static class CultureHelper
    {
        #region Methods

        #region Public methods

        /// <summary>
        /// Get the name of user current culture
        /// </summary>
        /// <returns>The current langage code.</returns>
        public static string GetCurrentLangageCode()
        {
            string currentLangageCode = default(string);
            if (HttpContext.Current.Session != null)
            {
                currentLangageCode = HttpContext.Current.Session["langageCode"] as string;
            }

            if (string.IsNullOrWhiteSpace(currentLangageCode))
            {
                currentLangageCode = GetDefaultLangageCode();
            }

            return currentLangageCode;
        }

        /// <summary>
        /// Set the default culture.
        /// </summary>
        public static void SetCurrentLangageCode()
        {
            SetCurrentLangageCode(null);
        }

        /// <summary>
        /// Set the culture with the its code
        /// </summary>
        /// <param name="langageCode">Langage code of the culture to use. If null, the session one will be used if available or the default one otherwise.</param>
        public static void SetCurrentLangageCode(string langageCode)
        {
            string sessionLangageCode = GetCurrentLangageCode();

            if (string.IsNullOrWhiteSpace(langageCode))
            {
                langageCode = sessionLangageCode;
            }

            if (string.IsNullOrWhiteSpace(langageCode))
            {
                langageCode = GetDefaultLangageCode();
            }

            if (langageCode != sessionLangageCode && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["langageCode"] = langageCode;
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(langageCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langageCode);
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Get the default Culture (en-us)
        /// </summary>
        /// <returns>Name of culture</returns>
        private static string GetDefaultLangageCode()
        {
            return "en-US";
        }

        #endregion Private methods

        #endregion Methods
    }
}