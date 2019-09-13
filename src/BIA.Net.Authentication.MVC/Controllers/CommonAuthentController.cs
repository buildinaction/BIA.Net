// <copyright file="CommonController.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication.MVC.Controllers
{
    using Authentication.Business;
    using BIA.Net.Web.Utility;
    using Common;
    using BIA.Net.Authentication.MVC;
    using Newtonsoft.Json;
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Authentication.Business.Synchronize;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using BIA.Net.Common.Helpers;

    /// <summary>
    /// Common controller for the whole application
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CommonAuthentController<TServiceSynchronizeUser, TUserInfo, TUserProperties> : Controller
        where TServiceSynchronizeUser : IServiceSynchronizeUser
        where TUserInfo : AUserInfo<TUserProperties>, new()
        where TUserProperties : IUserProperties, new()
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
            BIAAuthorizationFilterMVC<TUserInfo, TUserProperties>.RefreshAllUserInfo(((TUserInfo)User).Login);
        }

        /// <summary>
        /// Refresh a current application value
        /// </summary>
        /// <returns>Empty</returns>
        [HttpPost]
        public virtual void RefreshCurrentApplicationValues(string sKey)
        {
            BIAAuthorizationFilterMVC<TUserInfo, TUserProperties>.RefreshCurrentApplicationValues(sKey);
        }


        /// <summary>
        /// Refresh the current user profile
        /// </summary>
        /// <param name="login">login of the user to refresh</param>
        /// <returns>Empty</returns>
        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = BIAConstants.RoleInternal)]
        public virtual ActionResult RefreshUserProfile(string login)
        {
            BIAAuthorizationFilterMVC<TUserInfo, TUserProperties>.RefreshUserProfile(login);
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
                IServiceSynchronizeUser serviceUserDB = BIAUnity.Resolve<IServiceSynchronizeUser>();
                userDeleted = serviceUserDB.SynchronizeUsers(ADHelper.GetADGroupsForRole("User"));
            }

            foreach (string userName in userDeleted)
            {
                BIAAuthorizationFilterMVC<TUserInfo, TUserProperties>.RefreshUserRoles(userName);
            }

            return this.Json(string.Empty);
        }
    }
}