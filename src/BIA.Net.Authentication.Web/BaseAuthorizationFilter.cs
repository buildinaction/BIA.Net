﻿

namespace BIA.Net.Authentication.Web
{
    using BIA.Net.Common;
    using BIA.Net.Web.Utility;
    using Business;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;
    using System.Net.Http;
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common.Helpers;
    using System.Reflection;
    using System.DirectoryServices.AccountManagement;
    using static BIA.Net.Common.Configuration.CommonElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.IdentitiesElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.LanguageElement;

    public enum RolesRedirectAction
    {
        Authorized,
        Unauthorized,
        Redirect
    }

    public class RolesRedirectURL
    {
        public List<string> Roles { get; set; }
        public string Url { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }

    public class BaseAuthorizationFilter<TUserInfo, TUserDB>
        where TUserInfo : AUserInfo<TUserDB>, new()
        where TUserDB : IUserDB, new()
    {
        /// <summary>
        /// Gets or sets role at format :  Role1,Role1bis:~/Controller1/Action1;Role2:~/Controller1/Action1
        /// </summary>
        private List<RolesRedirectURL> rolesRedirect { get; set; }

        /// <summary>
        /// Gets or sets the user roles that are authorized to access the controller or action method.
        /// </summary>
        private List<string> roles;

        /// <summary>
        /// Gets or sets the user roles that are authorized to access the controller or action method.
        /// </summary>
        private List<string> rolesAllowAnonymous { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SafranAuthorizationFilter{TServiceSynchronizeUser, TUserInfo}"/> class.
        /// </summary>
        public BaseAuthorizationFilter(string roles = null, List<RolesRedirectURL> rolesRedirect = null, string rolesAllowAnonymous = null)
        {
            this.roles = roles?.Split(',').ToArray().ToList();
            this.rolesRedirect = rolesRedirect;
            this.rolesAllowAnonymous = rolesAllowAnonymous?.Split(',').ToArray().ToList();
        }

        public static TUserInfo PrepareUserInfo()
        {

            TUserInfo user = new TUserInfo();
            string cachingParameter = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.Caching;

            if (cachingParameter == "Session")
            {
                HttpSessionState Session = HttpContext.Current.Session;
                if (Session != null)
                {
                    AUserInfo<TUserDB>.UserInfoContainer container = (AUserInfo<TUserDB>.UserInfoContainer)Session[AuthenticationConstants.SessionUserInfo];
                    if (Session[AuthenticationConstants.SessionUserInfo] != null)
                    {
                        user.userInfoContainer = container;
                        CheckInfoToRefresh(user);
                    }
                    else
                    {
                        Session[AuthenticationConstants.SessionUserInfo] = user.userInfoContainer;
                    }
                }
            }


            IPrincipal principal = HttpContext.Current.User;
            WindowsIdentity windowsIdentity = (WindowsIdentity)principal.Identity;
            if (windowsIdentity != null && windowsIdentity.IsAuthenticated)
            {
                user.Identity = windowsIdentity;
            }

            user.FinalizePreparation();

            return user;
        }

        public static void CheckInfoToRefresh(TUserInfo user)
        {
            if (ShouldRefresh(AuthenticationConstants.SessionRefreshUserPropertiesDate, user.userInfoContainer.propertiesKey, user.userInfoContainer.propertiesRefreshDate))
            {
                user.userInfoContainer.propertiesShouldBeRefreshed = true;
            }

            if (ShouldRefresh(AuthenticationConstants.SessionRefreshUserRolesDate, user.userInfoContainer.rolesKey, user.userInfoContainer.rolesRefreshDate))
            {
                user.userInfoContainer.rolesShouldBeRefreshed = true;
            }

            if (ShouldRefresh(AuthenticationConstants.SessionRefreshUserProfileDate, user.userInfoContainer.userProfileKey, user.userInfoContainer.userProfileRefreshDate))
            {
                user.userInfoContainer.userProfileShouldBeRefreshed = true;
            }

            if (ShouldRefresh(AuthenticationConstants.SessionRefreshLanguageDate, user.userInfoContainer.languageKey, user.userInfoContainer.languageRefreshDate))
            {
                user.userInfoContainer.languageShouldBeRefreshed = true;
            }
        }

        static private void RefreshUser(TUserInfo user, bool shouldRefreshIdentities = true, bool shouldRefreshUserProperties =true, bool shouldRefreshUserProfile = true, bool shouldRefreshUserLangage = true, bool shouldRefreshUserRoles = true)
        {
            user.userInfoContainer.identitiesShouldBeRefreshed = shouldRefreshIdentities;
            user.userInfoContainer.propertiesShouldBeRefreshed = shouldRefreshUserProperties;
            user.userInfoContainer.userProfileShouldBeRefreshed = shouldRefreshUserProfile;
            user.userInfoContainer.languageShouldBeRefreshed = shouldRefreshUserLangage;
            user.userInfoContainer.rolesShouldBeRefreshed = shouldRefreshUserRoles;
        }

        static protected TUserInfo ConnectUser(HttpSessionState Session, TUserDB aspNetUser, string localUserId)
        {
            TUserInfo user = new TUserInfo();
            user.userInfoContainer = (AUserInfo<TUserDB>.UserInfoContainer) Session[AuthenticationConstants.SessionUserInfo];
            user.Properties = aspNetUser;
            if (user.Identities.Keys.Contains("LocalUserID")) user.Identities.Remove("LocalUserID");
            user.Identities.Add("LocalUserID", localUserId);
            user.AdditionnalRoles = new List<string> { "User" };
            RefreshUser( user, shouldRefreshUserProperties: false );
            return user;
        }

        static protected TUserInfo DisconnectUser(HttpSessionState Session)
        {
            TUserInfo user = new TUserInfo();
            user.userInfoContainer = (AUserInfo<TUserDB>.UserInfoContainer)Session[AuthenticationConstants.SessionUserInfo];
            user.Properties = default(TUserDB);
            if (user.Identities.Keys.Contains("LocalUserID")) user.Identities.Remove("LocalUserID");
            user.AdditionnalRoles = null;
            RefreshUser(user);
            return user;
        }

        private static bool ShouldRefresh(string RefreshKey, HttpSessionState Session, string userName)
        {
            if (Session[RefreshKey] == null) return true;
            return HttpContext.Current.Application[RefreshKey + "_" + userName] != null && DateTime.Compare((DateTime)HttpContext.Current.Application[RefreshKey + "_" + userName], (DateTime)Session[RefreshKey]) > 0;
        }


        private static bool ShouldRefresh(string RefreshKey, string userName, DateTime dateTimeLastRefresh)
        {
            if (dateTimeLastRefresh == DateTime.MinValue) return true;
            return HttpContext.Current.Application[RefreshKey + "_" + userName] != null && DateTime.Compare((DateTime)HttpContext.Current.Application[RefreshKey + "_" + userName], dateTimeLastRefresh) > 0;
        }

        protected RolesRedirectAction CheckAuthorize<TContext>(TUserInfo user, out RolesRedirectURL redirect, Func<TContext, bool> IsAllowAnonymousCallback, Func<TContext, List<string>> DisableRedirectRoles, TContext context)
        {
            redirect = null;
            if (this.roles == null)
            {
                return RolesRedirectAction.Authorized;
            }
            else
            {
                if ((user.Roles == null) || (user.Roles.Count == 0))
                {
                    return RolesRedirectAction.Unauthorized;
                }
                else
                {
                    if (TestRole(user, this.roles))
                    {
                        return RolesRedirectAction.Authorized;
                    }
                    else
                    {
                        if (this.rolesRedirect == null)
                        {
                            return RolesRedirectAction.Unauthorized;
                        }
                        else
                        {
                            bool isAllowAnonymous = IsAllowAnonymousCallback(context);

                            if (isAllowAnonymous)
                            {
                                if (rolesAllowAnonymous != null)
                                {
                                    if (!TestRole(user, this.rolesAllowAnonymous))
                                    {
                                        isAllowAnonymous = false;
                                    }
                                }
                            }

                            if (isAllowAnonymous)
                            {
                                return RolesRedirectAction.Authorized;
                            }

                            List<string> disableRedirectRoles = DisableRedirectRoles(context);

                            if (disableRedirectRoles!= null)
                            {
                                if (disableRedirectRoles.Count == 0) return RolesRedirectAction.Authorized;
                                if (TestRole(user, disableRedirectRoles))
                                {
                                    return RolesRedirectAction.Authorized;
                                }
                            }

                            foreach (RolesRedirectURL roleRedirect in this.rolesRedirect)
                            {
                                if (TestRole(user, roleRedirect.Roles))
                                {
                                    redirect = roleRedirect;
                                    return RolesRedirectAction.Redirect;
                                }
                            }

                            return RolesRedirectAction.Unauthorized;
                            
                        }
                    }
                }
            }
        }

        private bool TestRole(TUserInfo user, List<string> rolesToTest)
        {
            return rolesToTest.Any(r => user.Roles.Contains(r));
        }
    }
}
