// <copyright file="MyAuthorizationFilter.cs" company="BIA.Net">
// Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication.MVC
{
    using Business;
    using Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using BIA.Net.Authentication.Business.Helpers;



    /// <summary>
    /// Authorization Filter
    /// </summary>
    /// <seealso cref="System.Web.Mvc.IAuthorizationFilter" />
    /// <typeparam name="TServiceSynchronizeUser">The type of the service to synchronize users in DB.</typeparam>
    /// <typeparam name="TUserInfo">The type of the format stocked in session variable.</typeparam>
    /// <typeparam name="TUserProperties">The type of the user stocked in db.</typeparam>
    public class BIAAuthorizationFilterMVC<TUserInfo, TUserProperties> : BaseAuthorizationFilter<TUserInfo, TUserProperties>, IAuthorizationFilter
    where TUserInfo : AUserInfo<TUserProperties>, new()
    where TUserProperties : IUserProperties, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BIAAuthorizationFilterMVC{TServiceSynchronizeUser, TUserInfo}"/> class.
        /// </summary>
        public BIAAuthorizationFilterMVC(string roles = null, List<RolesRedirectURL> rolesRedirect = null, string rolesAllowAnonymous = null) 
            : base (roles,rolesRedirect,rolesAllowAnonymous)
        { }


        /// <summary>
        /// Refreshes the user roles of a specific user.
        /// </summary>
        /// <param name="userLogin">Login of the user.</param>
        public static void RefreshAllUserInfo(string userLogin)
        {
            RefreshUserProperties(userLogin);
            RefreshUserRoles(userLogin);
            RefreshUserProfile(userLogin);
        }

        /// <summary>
        /// Refreshes the user roles of a specific user.
        /// </summary>
        /// <param name="userLogin">Login of the user.</param>
        public static void RefreshUserRoles(string userLogin)
        {
            HttpContext.Current.Application[AuthenticationConstants.SessionRefreshUserRolesDate + "_" + userLogin] = DateTime.Now;
        }

        /// <summary>
        /// Refreshes the user properties of a specific user.
        /// </summary>
        /// <param name="userLogin">Login of the user.</param>
        public static void RefreshUserProperties(string userLogin)
        {
            HttpContext.Current.Application[AuthenticationConstants.SessionRefreshUserPropertiesDate + "_" + userLogin] = DateTime.Now;
        }

        /// <summary>
        /// Refreshes the user profile of a specific user.
        /// </summary>
        /// <param name="userLogin">Login of the user.</param>
        public static void RefreshUserProfile(string userLogin)
        {
            HttpContext.Current.Application[AuthenticationConstants.SessionRefreshUserProfileDate + "_" + userLogin] = DateTime.Now;
        }


        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            /*
            var principal = filterContext.HttpContext.User;
            HttpSessionState Session = HttpContext.Current.Session;

            TUserInfo user = default(TUserInfo);
            user = PrepareUserInfo(principal, Session);
            filterContext.HttpContext.User = (IPrincipal)user;*/

            TUserInfo user = (TUserInfo) AUserInfo<TUserProperties>.GetCurrentUserInfo();

            RolesRedirectURL roleRedirect;
            RolesRedirectAction action = CheckAuthorize(user, out roleRedirect, IsAllowAnonymous, DisableRedirectRoles, filterContext);

            switch (action)
            {
                case RolesRedirectAction.Unauthorized:
                    HandleUnauthorizedRequest(filterContext);
                    break;
                case RolesRedirectAction.Redirect:
                    filterContext.Result = new RedirectResult(roleRedirect.Url + "?urlReturn=" + filterContext.HttpContext.Request.RawUrl);
                    break;
                default:
                    break;
            }
        }

        private static bool IsAllowAnonymous(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
        }

        private static List<string> DisableRedirectRoles(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(DisableRedirectAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(DisableRedirectAttribute), true))
            {
                List<string> roles = new List<string>();
                var actionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(DisableRedirectAttribute), true);
                var controllerAttributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(DisableRedirectAttribute), true);
                var attributes = actionAttributes.Concat(controllerAttributes).OfType<DisableRedirectAttribute>().ToList();
                foreach (var disableRedirectAttribute in attributes)
                {
                    List<string> curentRoles = disableRedirectAttribute.GetRoles();
                    if (curentRoles == null) return new List<string>();
                    else
                    {
                        foreach(string role in curentRoles)
                        {
                            if (!roles.Contains(role)) roles.Add(role);
                        }
                    }    
                }
                return roles;
            }
            return null;
        }

        static public void ConnectUser(TUserProperties user, string localUserId)
        {
            HttpContext.Current.User = (IPrincipal)ConnectUser(HttpContext.Current.Session, user, localUserId);
        }

        static public void DisconnectUser()
        {
            HttpContext.Current.User = (IPrincipal)DisconnectUser(HttpContext.Current.Session);
        }
        /// <summary>
        /// Handles the unauthorized request.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() || string.Compare("GET", filterContext.HttpContext.Request.HttpMethod, true) != 0)
            {
                // Returns 403.
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden);
            }
            else
            {
                // Returns 401.
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}