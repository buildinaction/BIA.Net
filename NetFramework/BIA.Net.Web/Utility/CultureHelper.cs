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

        public static void SetLangageCode(string langageCode)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(langageCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langageCode);
        }

        #endregion Methods
    }
}