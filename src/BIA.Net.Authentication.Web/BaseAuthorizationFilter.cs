

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
    using static BIA.Net.Common.Configuration.AuthenticationElement.IdentityElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.IdentityElement.ADIdentityValueCollection;
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

        public static TUserInfo PrepareUserInfo(IPrincipal principal, HttpSessionState Session)
        {
            TUserInfo user = default(TUserInfo);

            //bool shouldRefreshUserProperties = true;
            //user.userInfoContainer.userProfileShouldBeRefreshed = true;
            //bool shouldRefreshUserLangage = true;
            //bool shouldRefreshUserRoles = true;
            //bool shouldRefreshLogins = true;
            if (Session != null)
            {
                if (Session[AuthenticationConstants.SessionUserInfo] != null)
                {
                    user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];
                    //shouldRefreshLogins = false;

                    if (ShouldRefresh(AuthenticationConstants.SessionRefreshUserPropertiesDate, user?.Login, user.userInfoContainer.propertiesRefreshDate))
                    {
                        user.userInfoContainer.propertiesShouldBeRefreshed = true;
                        user.userInfoContainer.languageShouldBeRefreshed = true;
                    }

                    if (ShouldRefresh(AuthenticationConstants.SessionRefreshUserRolesDate, user?.Login, user.userInfoContainer.rolesRefreshDate))
                    {
                        user.userInfoContainer.rolesShouldBeRefreshed = true;
                    }

                    if (ShouldRefresh(AuthenticationConstants.SessionRefreshUserProfileDate, user.userInfoContainer.userProfileKey, user.userInfoContainer.userProfileRefreshDate))
                    {
                        user.userInfoContainer.userProfileShouldBeRefreshed = true;
                    }
                }
                else
                {
                    user = new TUserInfo();
                }
            }
            else
            {
                user = new TUserInfo();
                user.userInfoContainer.userProfileShouldBeRefreshed = false;
                user.userInfoContainer.languageShouldBeRefreshed = false;
            }
            WindowsIdentity windowsIdentity = (WindowsIdentity)principal.Identity;
            if (windowsIdentity != null && windowsIdentity.IsAuthenticated)
            {
                user.Identity = windowsIdentity;
            }
            if (Session != null) Session[AuthenticationConstants.SessionUserInfo] = user;

            RefreshUser(Session, user,
                shouldRefreshLogins : user.userInfoContainer.loginShouldBeRefreshed,
                shouldRefreshUserProperties: user.userInfoContainer.propertiesShouldBeRefreshed, 
                shouldRefreshUserProfile : user.userInfoContainer.userProfileShouldBeRefreshed, 
                shouldRefreshUserLangage: user.userInfoContainer.languageShouldBeRefreshed, 
                shouldRefreshUserRoles: user.userInfoContainer.rolesShouldBeRefreshed);

            return user;
        }

        static private void RefreshUser(HttpSessionState Session, TUserInfo user, bool shouldRefreshLogins = true, bool shouldRefreshUserProperties =true, bool shouldRefreshUserProfile = true, bool shouldRefreshUserLangage = true, bool shouldRefreshUserRoles = true)
        {


            if (shouldRefreshLogins)
            {

            }

            if (user.Login != null)
            {
                

                if (Session != null)
                {
                    if (shouldRefreshUserProperties) Session[AuthenticationConstants.SessionRefreshUserPropertiesDate] = DateTime.Now;
                    if (shouldRefreshUserRoles) Session[AuthenticationConstants.SessionRefreshUserRolesDate] = DateTime.Now;
                }


                if (shouldRefreshUserRoles)
                {

                }

                if (shouldRefreshUserProfile)
                {
                    user.UserProfile = null;
                }

                if (shouldRefreshUserLangage)
                {
                    InitLanguage(user);
                }

                //if (Session != null) Session[AuthenticationConstants.SessionUserInfo] = user;
            }
        }

        static protected TUserInfo ConnectUser(HttpSessionState Session, TUserDB aspNetUser, string localUserLogin)
        {
            TUserInfo user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];
            user.Properties = aspNetUser;
            user.LocalUserLogin = localUserLogin;
            user.AdditionnalRoles = new List<string> { "User" };
            RefreshUser(Session, user, shouldRefreshUserProperties: false );
            return user;
        }

        static protected TUserInfo DisconnectUser(HttpSessionState Session)
        {
            TUserInfo user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];
            user.Properties = default(TUserDB);
            user.LocalUserLogin = null;
            user.AdditionnalRoles = null;
            RefreshUser(Session, user);
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

        /// <summary>
        /// Initializes the language.
        /// </summary>
        /// <param name="user">The user.</param>
        static private void InitLanguage(TUserInfo user)
        {
            string languageCode = null;
            ResolversCollection resolvers = BIASettingsReader.BIANetSection?.Authentication?.Language?.Resolvers;
            List<string> ApplicationLanguages = BIASettingsReader.BIANetSection?.Language?.GetApplicationLanguages();

            if (resolvers != null && resolvers.Count > 0)
            {
                foreach (ResolversCollection.ResolverElement resolver in resolvers)
                {
                    if (resolver.Key == "mapping")
                    {
                        PropertyInfo propertyInfo = null;

                        if (resolver?.Mapping?.Property != null)
                        {
                            propertyInfo = user.Properties.GetType().GetProperty(resolver.Mapping.Property);
                        }
                        
                        if (propertyInfo != null)
                        {
                            string propertyToMap = propertyInfo.GetValue(user.Properties).ToString();
                            foreach (KeyValueElement mapping in resolver.Mapping)
                            {
                                if (mapping.Key == propertyToMap)
                                {
                                    languageCode = mapping.Value;
                                    break;
                                }
                            }
                        }
                        if (languageCode != null) break;
                    }
                    else if (resolver.Key == "browser")
                    {
                        if (!string.IsNullOrEmpty(HttpContext.Current?.Request?.UserLanguages?[0]))
                        {
                            string currentLanguageCode = HttpContext.Current.Request.UserLanguages[0];

                            if (ApplicationLanguages.Contains(currentLanguageCode))
                            {
                                languageCode = currentLanguageCode;
                                break;
                            }
                            else if (ApplicationLanguages.Select(d => d.Substring(0, 2)).Contains(currentLanguageCode.Substring(0, 2)))
                            {
                                languageCode = ApplicationLanguages.Where(c => c.Substring(0, 2) == currentLanguageCode.Substring(0, 2)).First();
                                break;
                            }
                        }
                    }
                }
            }
            if (languageCode == null)
            {
                languageCode = BIASettingsReader.BIANetSection?.Authentication?.Language?.Default;
            }
            if (languageCode != null)
            {
                CultureHelper.SetCurrentLangageCode(languageCode);
            }
            else
            {
                CultureHelper.SetCurrentLangageCode("en-US");
            }
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
