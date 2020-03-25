namespace $safeprojectname$.Helpers
{
    using BIA.Net.Authentication.Web;
    using BIA.Net.Common;
    using BIA.Net.Web.Utility;
    using Business.Helpers;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;

    /// <summary>
    /// UserInfo adapted to MVC.
    /// </summary>
    public class UserInfoMVC : UserInfo
    {
        /// <inheritdoc/>
        public override string Language
        {
            get
            {
                return base.Language;
            }

            set
            {
                VarSession.MyMenu = null;
                base.Language = value;
            }
        }

        /// <summary>
        /// Get language from browser.
        /// </summary>
        /// <returns>Browser language if match with application languages</returns>
        public override string CustomCodeLanguage()
        {
            List<string> applicationLanguages = BIASettingsReader.BIANetSection?.Language?.GetApplicationLanguages().Select(s => s.Code).ToList();
            if (!string.IsNullOrEmpty(HttpContext.Current?.Request?.UserLanguages?[0]))
            {
                string currentLanguageCode = HttpContext.Current.Request.UserLanguages[0];

                if (applicationLanguages.Contains(currentLanguageCode))
                {
                    return currentLanguageCode;
                }
                else if (applicationLanguages.Select(d => d.Substring(0, 2)).Contains(currentLanguageCode.Substring(0, 2)))
                {
                    return applicationLanguages.Where(c => c.Substring(0, 2) == currentLanguageCode.Substring(0, 2)).First();
                }
            }

            return null;
        }

        /// <inheritdoc/>
        protected override void RefreshRolesInBuilding()
        {
            base.RefreshRolesInBuilding();
            VarSession.MyMenu = null;
        }
    }
}