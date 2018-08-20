// <copyright file="CommonController.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication.Controllers
{
    using Authentication.Business;
    using BIA.Net.Web.Utility;
    using Common;
    using BIA.Net.Authentication;
    using Newtonsoft.Json;
    using BIA.Net.Authentication.Business.Helpers;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Common controller for the whole application
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CommonAuthentController<TServiceSynchronizeUser, TUserInfo, TUserProperties, TUserPropertiesInDB> : Controller
        where TServiceSynchronizeUser : AServiceSynchronizeUser, new()
        where TUserInfo : AUserInfo<TUserProperties>, new()
        where TUserProperties : IUserProperties, new()
        where TUserPropertiesInDB : IUserPropertiesInDB, new()
    {
        /// <summary>
        /// Changes the session language according to the language selected by user
        /// </summary>
        /// <param name="code">code of the language</param>
        /// <returns>Empty Action Result</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetLanguageInfo(string code)
        {
            string languageCode = JsonConvert.DeserializeObject<string>(code);
            if (!string.IsNullOrEmpty(languageCode))
            {
                //AuthentVarSession.MyMenu = null;
                //CultureHelper.SetCurrentLangageCode(languageCode);
                ((TUserInfo)User).Language = languageCode;
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Refresh the current user properties and right
        /// </summary>
        [HttpPost]
        public virtual void RefreshUserInfo()
        {
            SafranAuthorizationFilter<TUserInfo, TUserProperties>.RefreshAllUserInfo(((TUserInfo)User).Login);
        }

        /// <summary>
        /// Refresh the current user profile
        /// </summary>
        /// <param name="login">login of the user to refresh</param>
        /// <returns>Empty</returns>
        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = BIAConstantes.RoleInternal)]
        public virtual ActionResult RefreshUserProfile(string login)
        {
            SafranAuthorizationFilter<TUserInfo, TUserProperties>.RefreshUserProfile(login);
            return Content("User " + login + " is refreshed.");
        }

        /// <summary>
        /// Refresh all users properties and right
        /// </summary>
        /// <returns>Empty</returns>
        [HttpPost]
        public virtual ActionResult RefreshUsersFromAD()
        {
            List<string> userDeleted = new List<string>();
            if (ADHelper.GetADGroupsForRole("User") != null)
            {
                using (TServiceSynchronizeUser serviceUserDB = new TServiceSynchronizeUser())
                {
                    userDeleted = serviceUserDB.SynchronizeUsers<TUserInfo, TUserProperties, TUserPropertiesInDB>(ADHelper.GetADGroupsForRole("User"));
                }
            }

            foreach (string userName in userDeleted)
            {
                SafranAuthorizationFilter<TUserInfo, TUserProperties>.RefreshUserRoles(userName);
            }

            return this.Json(string.Empty);
        }
    }
}