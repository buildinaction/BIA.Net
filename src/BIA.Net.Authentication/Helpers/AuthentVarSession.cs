namespace BIA.Net.Common
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// list of session variables
    /// </summary>
    public static class AuthentVarSession
    {
        /// <summary>
        /// Gets or sets my Menu
        /// </summary>
        public static MvcHtmlString MyMenu
        {
            get { return HttpContext.Current.Session["MyMenu"] as MvcHtmlString; }
            set { HttpContext.Current.Session["MyMenu"] = value; }
        }
    }
}