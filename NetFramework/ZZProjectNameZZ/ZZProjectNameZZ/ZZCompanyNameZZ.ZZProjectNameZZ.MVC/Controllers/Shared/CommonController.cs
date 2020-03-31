// <copyright file="CommonController.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.Controllers.Shared
{
    using BIA.Net.Authentication.MVC.Controllers;
    using Business.DTO;
    using Business.Helpers;
    using Business.Services.Authentication;
    using Newtonsoft.Json;
    using System.Web.Mvc;

    /// <summary>
    /// Common controller for the whole application
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CommonController : CommonAuthentController<ServiceSynchronizeUser, UserInfo, UserDTO>
    {
        /// <summary>
        /// Changes the session language according to the language selected by user
        /// </summary>
        /// <param name="code">the theme to set</param>
        /// <returns>Empty Action Result</returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SetTheme(string code)
        {
            string sTheme = code;
            if (!string.IsNullOrEmpty(sTheme))
            {
                ((UserInfo)User).UserProfile["Theme"] = sTheme;
            }

            return new EmptyResult();
        }
    }
}