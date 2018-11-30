

namespace BIA.Net.Authentication.Web
{
    using BIA.Net.Common;
    using Business;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;
    using System.Net.Http;
    using BIA.Net.Authentication.Business.Helpers;
    using System.Collections.Specialized;
    using System.Security.Cryptography.X509Certificates;
    using static BIA.Net.Common.Configuration.AuthenticationElement;
    using static BIA.Net.Common.Configuration.CommonElement;
    using System.Reflection;

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


    public class BaseAuthorizationFilter<TUserInfo, TUserProperties>
        where TUserInfo : AUserInfo<TUserProperties>, new()
        where TUserProperties : IUserProperties, new()
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
        /// Initializes a new instance of the <see cref="BaseAuthorizationFilter{TServiceSynchronizeUser, TUserInfo}"/> class.
        /// </summary>
        public BaseAuthorizationFilter(string roles = null, List<RolesRedirectURL> rolesRedirect = null, string rolesAllowAnonymous = null)
        {
            this.roles = roles?.Split(',').ToArray().ToList();
            this.rolesRedirect = rolesRedirect;
            this.rolesAllowAnonymous = rolesAllowAnonymous?.Split(',').ToArray().ToList();
        }

        public static TUserInfo PrepareUserInfo()
        {
            /*
            var tests= BIASettingsReader.BIANetSection?.Authentication?.Tests;
            foreach(HeterogeneousConfigurationElementBase test in tests)
            {
                if (test is Test1Element)
                {
                    string toto = ((Test1Element)test).KeyTest1;
                }
                if (test is Test2Element)
                {
                    string toto = ((Test2Element)test).KeyTest2;
                }
            }*/


            TUserInfo user = new TUserInfo();
            string cachingParameter = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.Caching;
            bool manageSession = false;

            if (cachingParameter == "Session")
            {
                HttpSessionState Session = HttpContext.Current.Session;
                if (Session != null)
                {
                    manageSession = true;
                    if (Session[AuthenticationConstants.SessionUserInfo] != null)
                    {
                        AUserInfo<TUserProperties>.UserInfoContainer container = (AUserInfo<TUserProperties>.UserInfoContainer)Session[AuthenticationConstants.SessionUserInfo];
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
            user.Identity = (WindowsIdentity)principal.Identity;

            if (!user.Identity.IsAuthenticated)
            {
                ClientCertificateInHeaderCollection clientCertificateInHeader = BIASettingsReader.BIANetSection?.Authentication?.Identities?.OfType<ClientCertificateInHeaderCollection>().FirstOrDefault();
                if (clientCertificateInHeader != null && !user.Identity.IsAuthenticated)
                {
                    NameValueCollection headers = HttpContext.Current.Request.Headers;
                    string certHeader = headers[clientCertificateInHeader.Key];
                    if (!String.IsNullOrEmpty(certHeader))
                    {
                        try
                        {
                            byte[] clientCertBytes = Convert.FromBase64String(certHeader);
                            X509Certificate2 Certificate = new X509Certificate2(clientCertBytes);
                            if (Certificate != null)
                            {
                                if (IsCertificatValid(Certificate, clientCertificateInHeader))
                                {
                                    user.Identity = new WindowsIdentity(GetCertificatValue(Certificate, clientCertificateInHeader.WindowsIdentity));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TraceManager.Error("Error when analyse certificat.", ex);
                        }
                    }
                    else
                    {
                        certHeader = "";
                    }
                }
            }

            /*
            if (((user.Identity == null || user.Identity.IsAuthenticated==false) && (windowsIdentity != null && windowsIdentity.IsAuthenticated)) || (user.Certificate == null && certificat != null))
            {
                if (retrivedFromSession)
                {
                    user = new TUserInfo();
                    HttpContext.Current.Session[AuthenticationConstants.SessionUserInfo] = user.userInfoContainer;
                }
                user.Identity = windowsIdentity;
                user.Certificate = certificat;
            }
            if (user.Identity == null) user.Identity = windowsIdentity;*/

            /*if (retrivedFromSession && user.Identity.IsAuthenticated && user.Login == null)
            {
                user.userInfoContainer.identitiesShouldBeRefreshed = true;
            }*/

            user.FinalizePreparation();

            if (manageSession && (user.Login == null))
            {
                HttpContext.Current.Session[AuthenticationConstants.SessionUserInfo] = null;
            }

            return user;
        }


        private static bool IsCertificatValid(X509Certificate2 Certificate, ClientCertificateInHeaderCollection clientCertificationCollection)
        {
            if (Certificate == null) return false;

            // 1. Check time validity of certificate
            if (DateTime.Compare(DateTime.Now, Certificate.NotBefore) < 0 || DateTime.Compare(DateTime.Now, Certificate.NotAfter) > 0) return false;

            bool isValid = true;
            foreach (ValidationCollection validationCollection in clientCertificationCollection.OfType<ValidationCollection>())
            {
                isValid = false; //reset to false if bad member.
                bool validation_rejected = false;
                foreach (KeyValueElement keyValue in validationCollection)
                {
                    string value = GetCertificatValue(Certificate, keyValue.Key);
                    if (value != keyValue.Value)
                    {
                        validation_rejected = true;
                        break;
                    }
                }
                if (!validation_rejected)
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }

        private static string GetCertificatValue(X509Certificate2 Certificate, string key)
        {
            string value = null;
            string[] keys = key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            PropertyInfo propertyInfo = Certificate.GetType().GetProperty(keys[0]);
            if (propertyInfo != null)
            {
                value = (string)propertyInfo.GetValue(Certificate);
            }
            if (keys.Length > 1)
            {
                string[] certSubjectData = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                bool foundKey = false;
                foreach (string s in certSubjectData)
                {
                    string[] subValues = s.Trim().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (subValues.Length == 2)
                    {
                        if (subValues[0] == keys[1])
                        {
                            value = subValues[1];
                            foundKey = true;
                            break;
                        }

                    }
                }
                if (!foundKey) value = null;
            }

            return value;
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
