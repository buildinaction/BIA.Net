

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
    using static BIA.Net.Common.Configuration.AuthenticationElement.SourcesElement.IdentityElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.SourcesElement.IdentityElement.ADIdentityValueCollection;

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

            bool shouldRefreshUserProperties = true;
            bool shouldRefreshUserProfile = true;
            bool shouldRefreshUserLangage = true;
            bool shouldRefreshUserRoles = true;
            bool shouldRefreshLogins = true;
            if (Session != null)
            {
                if (Session[AuthenticationConstants.SessionUserInfo] != null)
                {
                    user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];
                    shouldRefreshLogins = false;

                    if (!ShouldRefresh(AuthenticationConstants.SessionRefreshUserPropertiesDate, Session, user?.Login))
                    {
                        shouldRefreshUserLangage = false;
                        shouldRefreshUserProperties = false;
                    }

                    if (!ShouldRefresh(AuthenticationConstants.SessionRefreshUserRolesDate, Session, user?.Login))
                    {
                        shouldRefreshUserRoles = false;
                    }

                    if (!ShouldRefresh(AuthenticationConstants.SessionRefreshUserProfileDate, Session, user.UserProfileName))
                    {
                        shouldRefreshUserProfile = false;
                    }
                }
                else
                {
                    user = new TUserInfo();
                }
            }
            else
            {
                shouldRefreshUserProfile = false;
                shouldRefreshUserLangage = false;
                user = new TUserInfo();
            }
            WindowsIdentity windowsIdentity = (WindowsIdentity)principal.Identity;
            if (windowsIdentity != null && windowsIdentity.IsAuthenticated)
            {
                user.Identity = windowsIdentity;
            }

            RefreshUser(Session, user,
                shouldRefreshLogins : shouldRefreshLogins,
                shouldRefreshUserProperties: shouldRefreshUserProperties, 
                shouldRefreshUserProfile : shouldRefreshUserProfile, 
                shouldRefreshUserLangage: shouldRefreshUserLangage, 
                shouldRefreshUserRoles: shouldRefreshUserRoles);

            return user;
            /* TODO for AD
            WindowsIdentity windowsIdentity = (WindowsIdentity)principal.Identity;
            if (windowsIdentity != null && windowsIdentity.IsAuthenticated)
            {

                if (Session == null)
                {
                    //Section used by webApi without session

                    List<string> userGroups = ADHelper.GetGroups(windowsIdentity);
                    if (userGroups.Count == 0 && !BIASettingsReader.DisableUserGroupCheck)
                    {
                        TraceManager.Info("AuthService", "GetUserInfo", "User " + PreparePrincipalUserName(windowsIdentity) + " is not in AD '...User' group.");
                    }
                    else
                    {
                        user = new TUserInfo() { };
                        user.Identity = windowsIdentity;
                        TUserDB userProperties = ADHelper.UserDBFromWindowsIdentity<TUserDB>(windowsIdentity);
                        user.SetProperties(userProperties);
                        user.SetRoles(userGroups, userProperties);
                    }
                }
                else
                {
                    bool shouldRefreshUserProperties = true;
                    bool shouldRefreshUserProfile = true;
                    bool shouldRefreshUserLangage = true;
                    bool shouldRefreshUserRoles = true;

                    if (Session[AuthenticationConstants.SessionUserInfo] != null)
                    {
                        user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];

                        if (!ShouldRefresh(AuthenticationConstants.SessionRefreshUserPropertiesDate, Session, user?.Properties?.Login))
                        {
                            shouldRefreshUserLangage = false;
                            shouldRefreshUserProperties = false;
                        }

                        if (!ShouldRefresh(AuthenticationConstants.SessionRefreshUserRolesDate, Session, user?.Properties?.Login))
                        {
                            shouldRefreshUserRoles = false;
                        }

                        if (!ShouldRefresh(AuthenticationConstants.SessionRefreshUserProfileDate, Session, user.UserProfileName))
                        {
                            shouldRefreshUserProfile = false;
                        }
                    }
                    else
                    {
                        user = new TUserInfo();
                    }
                    user.Identity = windowsIdentity;

                    RefreshUser(Session, user, shouldRefreshUserProperties, shouldRefreshUserProfile, shouldRefreshUserLangage, shouldRefreshUserRoles);
                }
                if (user == null)
                {
                    TraceManager.Warn("user null for : " + PreparePrincipalUserName(windowsIdentity));
                }
            }
            return user;*/
        }

        static private void RefreshUser(HttpSessionState Session, TUserInfo user, bool shouldRefreshLogins = true, bool shouldRefreshUserProperties =true, bool shouldRefreshUserProfile = true, bool shouldRefreshUserLangage = true, bool shouldRefreshUserRoles = true)
        {
            List<string> adUserGroups = null;

            if (shouldRefreshLogins)
            {
                user.Login = null;
                user.SecondaryLogin = null;
                KeyValueCollection ValuesIdentities = BIASettingsReader.BIANetSection?.Authentication?.Sources?.Identity?.Values;
                if (ValuesIdentities != null)
                {
                    foreach (KeyValueElement value in ValuesIdentities)
                    {
                        PropertyInfo propertyInfo = user.GetType().GetProperty(value.Key);
                        if (propertyInfo!=null)
                        {
                            propertyInfo.SetValue(user, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
                        }
                    }
                }
                ADIdentityValueCollection AdIdentities = BIASettingsReader.BIANetSection?.Authentication?.Sources?.Identity?.AD;
                if (AdIdentities != null)
                {
                    foreach (ADIdentityValueElement value in AdIdentities)
                    {
                        if (value.IdentityField != null)
                        {
                            //The user.Identity is the login send by Windows identity
                            PropertyInfo propertyInfo = user.GetType().GetProperty(value.Key);
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(user, Convert.ChangeType(PreparePrincipalUserName(user.Identity, value.IdentityField, value.RemoveDomain), propertyInfo.PropertyType));
                            }
                        }
                    }
                }
            }

            if (user.Login != null)
            {
                TUserDB newUserProperties = default(TUserDB);
                if (shouldRefreshUserProperties || shouldRefreshUserRoles || shouldRefreshUserProfile)
                {
                    if (shouldRefreshUserProperties || shouldRefreshUserRoles)
                    {
                        //Initialize Roles
                        adUserGroups = new List<string>();

                        //TODO for ADadUserGroups = ADHelper.GetGroups(userLogin1stLevel);
                        ValueCollection ValuesRoles = BIASettingsReader.BIANetSection?.Authentication?.Sources?.Roles?.Values;
                        if (ValuesRoles != null)
                        {
                            foreach (ValueElement value in ValuesRoles)
                            {
                                adUserGroups.Add(value.Value);
                            }
                        }

                        KeyValueCollection ADRoles = BIASettingsReader.BIANetSection?.Authentication?.Sources?.Roles?.AD;
                        if (ADRoles != null)
                        {
                            adUserGroups.AddRange(ADHelper.GetGroups(user.Login, ADRoles, BIASettingsReader.BIANetSection?.Authentication?.Parameters?.AD?.Domains));
                        }

                        if (user?.AdditionnalRoles != null)
                        {
                            foreach (string group in user.AdditionnalRoles)
                            {
                                if (!adUserGroups.Contains(group)) adUserGroups.Add(group);
                            }
                        }


                        //TO test
                        ///>>string appliLogin = user.Login;
                        ///Get login name if user connected with generic account
                        ///>if (!shouldRefreshUserProperties && !string.IsNullOrEmpty(user?.SecondaryLogin)) appliLogin = user.SecondaryLogin;
                        //get properties from database or from AD (used to compute roles)
                        ///>>newUserProperties = GetUserProperties(user.Login, appliLogin, adUserGroups);
                        newUserProperties = GetUserProperties(user, adUserGroups);
                    }
                }

                if (Session != null)
                {
                    if (shouldRefreshUserProperties) Session[AuthenticationConstants.SessionRefreshUserPropertiesDate] = DateTime.Now;
                    if (shouldRefreshUserRoles) Session[AuthenticationConstants.SessionRefreshUserRolesDate] = DateTime.Now;
                    if (shouldRefreshUserProfile) Session[AuthenticationConstants.SessionRefreshUserProfileDate] = DateTime.Now;
                }

                if (shouldRefreshUserProperties)
                {
                    user.SetProperties(newUserProperties);
                }

                if (shouldRefreshUserRoles)
                {
                    if (Session != null) Session["MyMenu"] = null;
                    user.Roles = null;
                    if (user.Properties != null)
                    {
                        user.SetRoles(adUserGroups, newUserProperties);
                    }
                }

                if (shouldRefreshUserProfile)
                {
                    user.UserProfile = null;
                    if (user.Roles != null)
                    {
                        //UserProfil refreshed when it is called (TODO: to Validate)
                        user.UserProfileName = user.Roles.Contains("Generic") ? null : user.Login;
                    }
                    else
                    {
                        user.UserProfileName = null;
                    }
                }

                if (shouldRefreshUserLangage)
                {
                    InitLanguage(user);
                }

                if (Session != null) Session[AuthenticationConstants.SessionUserInfo] = user;
            }
        }

        private static string PreparePrincipalUserName(IIdentity Identity, string fromFieldName, bool removeDomain)
        {
            string userName = null;
            PropertyInfo propertyInfo = Identity.GetType().GetProperty(fromFieldName);
            if (propertyInfo != null)
            {
                userName = (string)propertyInfo.GetValue(Identity);
            }

            if (string.IsNullOrEmpty(userName))
            {
                //SAML2
                userName = ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                TraceManager.Debug("SafranAuthorizationFilterApi", "OnAuthorization", "NameID SAML2 : " + userName);
            }

            if (!string.IsNullOrEmpty(BIASettingsReader.ADSimuUser))
            {
                userName = BIASettingsReader.ADSimuUser;
            }

            if (removeDomain)
            {
                userName = ADHelper.RemoveDomain(userName);
            }

            return userName;
        }

        static protected TUserInfo ConnectUser(HttpSessionState Session, TUserDB aspNetUser, string secondaryLogin)
        {
            TUserInfo user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];
            user.Properties = aspNetUser;
            user.SecondaryLogin = secondaryLogin;
            user.AdditionnalRoles = new List<string> { "User" };
            RefreshUser(Session, user, shouldRefreshUserProperties: false );
            return user;
        }

        static protected TUserInfo DisconnectUser(HttpSessionState Session)
        {
            TUserInfo user = (TUserInfo)Session[AuthenticationConstants.SessionUserInfo];
            user.Properties = default(TUserDB);
            user.SecondaryLogin = null;
            user.AdditionnalRoles = null;
            RefreshUser(Session, user);
            return user;
        }

        private static bool ShouldRefresh(string RefreshKey, HttpSessionState Session, string userName)
        {
            if (Session[RefreshKey] == null) return true;
            return HttpContext.Current.Application[RefreshKey + "_" + userName] != null && DateTime.Compare((DateTime)HttpContext.Current.Application[RefreshKey + "_" + userName], (DateTime)Session[RefreshKey]) > 0;
        }

        /// <summary>
        /// Initializes the language.
        /// </summary>
        /// <param name="user">The user.</param>
        static private void InitLanguage(TUserInfo user)
        {
            //TODO
            CultureHelper.SetCurrentLangageCode("en-US");
            /*
            string languageCode = ADLanguageHelper.GetADLanguage(user.GetLanguage());

            if (languageCode == null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current?.Request?.UserLanguages?[0]))
                {
                    string currentLanguageCode = HttpContext.Current.Request.UserLanguages[0];

                    if (LanguageInfo.LanguageInfoDictionary.Select(d => d.Value.Code).Contains(currentLanguageCode))
                    {
                        languageCode = currentLanguageCode;
                    }
                    else if (LanguageInfo.LanguageInfoDictionary.Select(d => d.Value.Code.Substring(0, 2)).Contains(currentLanguageCode.Substring(0, 2)))
                    {
                        languageCode = LanguageInfo.LanguageInfoDictionary.Select(d => d.Value.Code).Where(c => c.Substring(0, 2) == currentLanguageCode.Substring(0, 2)).First();
                    }
                    else
                    {
                        languageCode = "en-US";
                    }
                }
                else
                {
                    languageCode = "en-US";
                }
            }

            CultureHelper.SetCurrentLangageCode(languageCode);*/
        }


        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The user information</returns>
        /// <exception cref="System.Exception">You're not authorize to connect to this application: Your aren't in the AD user group.</exception>
        static private TUserDB GetUserProperties(TUserInfo userInfo, List<string> adUserGroups)
        {
            TUserDB userProperties = default(TUserDB);
            //Initialize Properties

            MethodFunctionElement Service = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.Service;
            if (Service!= null && Service.Type != null)
            {
                 userProperties = (TUserDB)Service.Type.GetMethod(Service.Method).Invoke(null, new object[] { userInfo.Login, userInfo.SecondaryLogin, adUserGroups });
            }
            else
            {
                userProperties = PropertiesHelper.PrepareProperties<TUserInfo,TUserDB>(userInfo);
            }

            KeyValueCollection UserPropertiesValues = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.Values;
            if (UserPropertiesValues != null && UserPropertiesValues.Count > 0)
            {
                foreach (KeyValueElement value in UserPropertiesValues)
                {
                    PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                    propertyInfo.SetValue(userProperties, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
                }
            }

            return userProperties;
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
