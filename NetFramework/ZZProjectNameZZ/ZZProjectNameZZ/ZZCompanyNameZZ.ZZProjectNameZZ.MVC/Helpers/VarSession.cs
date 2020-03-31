namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Helpers
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// list of session variables
    /// </summary>
    public static class VarSession
    {
        /// <summary>
        /// Gets or sets my Menu
        /// </summary>
        public static MvcHtmlString MyMenu
        {
            get
            {
                if (HttpContext.Current.Session != null)
                {
                    return HttpContext.Current.Session["MyMenu"] as MvcHtmlString;
                }

                return null;
            }

            set
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["MyMenu"] = value;
                }
            }
        }
    }
}