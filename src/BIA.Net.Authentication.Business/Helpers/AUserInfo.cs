namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.DirectoryServices.AccountManagement;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Text.RegularExpressions;
    using static BIA.Net.Common.Configuration.AuthenticationElement.LanguageElement;
    using static BIA.Net.Common.Configuration.CommonElement;

    /// <summary>
    /// Class to define identity.
    /// </summary>
    public abstract class AUserInfo<TUserProperties> : IPrincipal, IUserInfo
        where TUserProperties : IUserProperties, new()
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        public AUserInfo()
        {
            userInfoContainer = new UserInfoContainer();
        }
        #endregion Constructors

        #region Properties

        #region TemporaryWorkingValues
        /// <summary>
        /// Gets the current identity.
        /// </summary>
        public IIdentity Identity { get; set; }

        private bool isUserPrincipalInit = false;
        private UserPrincipal userPrincipal { get; set; }

        protected UserPrincipal UserPrincipal
        {
            get
            {
                if (!isUserPrincipalInit)
                {
                    userPrincipal = ADHelper.GetUserFromADs(Login);
                    isUserPrincipalInit = true;
                }
                return userPrincipal;
            }
        }

        private bool isUserGroupsInit = false;
        private List<string> userGroups = null;
        protected List<string> UserGroups
        {
            get
            {
                if (!isUserGroupsInit)
                {
                    userGroups = ADHelper.GetGroups(Login);
                    isUserGroupsInit = true;
                }
                return userGroups;
            }
        }

        private string login = null;
        public virtual string Login
        {
            get
            {
                if (login == null)
                {
                    login = Identities["Login"];
                }
                return login;
            }
        }


        private string localUserID = null;
        public virtual string LocalUserID
        {
            get
            {
                if (localUserID == null)
                {
                    localUserID = Identities["LocalUserID"];
                }
                return localUserID;
            }
        }


        Dictionary<string, string> identitiesInBuilding = null;
        TUserProperties propertiesInBuilding = default(TUserProperties);
        string languageInBuilding = null;
        Dictionary<string, string> userProfileInBuilding = null;
        List<string> rolesInBuilding = null;
        #endregion

        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        /// <value>
        /// The roles of the user.
        /// </value>
        public virtual List<string> Roles
        {
            get
            {
                if (userInfoContainer.rolesShouldBeRefreshed)
                {
                    if (rolesInBuilding != null)
                    {
                        TraceManager.Debug("Return the rolesInBuilding");
                        return rolesInBuilding;
                    }
                    RefreshRoles();
                }
                return userInfoContainer.roles;
            }
            set
            {
                userInfoContainer.roles = value;
                userInfoContainer.rolesShouldBeRefreshed = false;
                userInfoContainer.rolesRefreshDate = DateTime.Now;
            }
        }
        public virtual TUserProperties Properties
        {
            get
            {
                if (userInfoContainer.propertiesShouldBeRefreshed)
                {
                    if (propertiesInBuilding != null)
                    {
                        TraceManager.Debug("Return the propertiesInBuilding");
                        return propertiesInBuilding;
                    }
                    RefreshProperties();
                }
                return userInfoContainer.properties;
            }
            set
            {
                userInfoContainer.properties = value;
                userInfoContainer.propertiesShouldBeRefreshed = false;
                userInfoContainer.propertiesRefreshDate = DateTime.Now;
            }
        }

        public virtual Dictionary<string, string> Identities
        {
            get
            {
                if (userInfoContainer.identitiesShouldBeRefreshed)
                {
                    if (identitiesInBuilding != null)
                    {
                        TraceManager.Debug("Return the identitiesInBuilding");
                        return identitiesInBuilding;
                    }
                    RefreshIdentities();
                }
                return userInfoContainer.identities;
            }
            set
            {
                userInfoContainer.identities = value;
                userInfoContainer.identitiesShouldBeRefreshed = false;
                userInfoContainer.identitiesRefreshDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets or sets the user profil
        /// </summary>
        public virtual Dictionary<string, string> UserProfile
        {
            get
            {
                if (userInfoContainer.userProfileShouldBeRefreshed)
                {
                    if (userProfileInBuilding != null)
                    {
                        TraceManager.Debug("Return the userProfileInBuilding");
                        return userProfileInBuilding;
                    }
                    RefreshUserProfile();
                }
                return userInfoContainer.userProfile;
            }
            set
            {
                userInfoContainer.userProfile = value;
                userInfoContainer.userProfileShouldBeRefreshed = false;
                userInfoContainer.userProfileRefreshDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Gets or sets the language
        /// </summary>
        public virtual string Language
        {
            get
            {
                if (userInfoContainer.languageShouldBeRefreshed)
                {
                    if (languageInBuilding != null)
                    {
                        TraceManager.Debug("Return the languageInBuilding");
                        return languageInBuilding;
                    }
                    RefreshLanguage();
                }
                return userInfoContainer.language;
            }
            set
            {
                userInfoContainer.language = value;
                userInfoContainer.languageShouldBeRefreshed = false;
                userInfoContainer.languageRefreshDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Additionnal Role added by custom connect
        /// </summary>
        public List<string> AdditionnalRoles { get; set; }


        public UserInfoContainer userInfoContainer;


        /// <summary>
        /// Gets or sets the user name (used to retrieve profile)
        /// </summary>

        public class UserInfoContainer
        {
            #region identities
            public DateTime identitiesRefreshDate = DateTime.MinValue;
            public bool identitiesShouldBeRefreshed = true;
            public string identitiesKey;

            public Dictionary<string, string> identities = null;
            #endregion

            #region language
            public DateTime languageRefreshDate = DateTime.MinValue;
            public bool languageShouldBeRefreshed = true;
            public string languageKey;

            public string language = null;
            #endregion

            #region roles
            public DateTime rolesRefreshDate = DateTime.MinValue;
            public bool rolesShouldBeRefreshed = true;
            public string rolesKey;

            public List<string> roles = null;
            #endregion

            #region properties
            public DateTime propertiesRefreshDate = DateTime.MinValue;
            public bool propertiesShouldBeRefreshed = true;
            public string propertiesKey;

            public TUserProperties properties = default(TUserProperties);
            #endregion

            #region userProfile
            public DateTime userProfileRefreshDate = DateTime.MinValue;
            public bool userProfileShouldBeRefreshed = true;
            public string userProfileKey;

            public Dictionary<string, string> userProfile = null;
            #endregion
        }


        #endregion Properties

        #region Methods

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        static public IUserInfo GetCurrentUserInfo()
        {
            return BIAUnity.ResolveContent<IUserInfo>();
        }

        #endregion Methods

        protected virtual void RefreshIdentities()
        {
            BaseRefreshIdentities();
        }


        public void BaseRefreshIdentities()
        {
            identitiesInBuilding = new Dictionary<string, string>();

            HeterogeneousCollection identities = BIASettingsReader.BIANetSection?.Authentication?.Identities;
            if (identities != null && identities.Count > 0)
            {
                foreach (IHeterogeneousConfigurationElement heterogeneousElem in identities)
                {
                    if (heterogeneousElem is ValueElement)
                    {
                        SetFromValueElement(identitiesInBuilding, heterogeneousElem);
                    }
                    else if (heterogeneousElem is WindowsIdentityElement)
                    {
                        WindowsIdentityElement value = (WindowsIdentityElement)heterogeneousElem;
                        if (value.IdentityField != null)
                        {
                            identitiesInBuilding.Add(value.Key, PreparePrincipalUserName(Identity, value.IdentityField, value.RemoveDomain));
                        }
                    }
                    else if (heterogeneousElem is ObjectFieldElement)
                    {
                        ObjectFieldElement value = (ObjectFieldElement)heterogeneousElem;
                        object result = ExtractObjectFieldValue(value);
                        if (result != null)
                        {
                            identitiesInBuilding.Add(value.Key, result.ToString());
                        }
                    }
                    else if (heterogeneousElem.TagName == "CustomCode")
                    {
                        if (SetFromCustomCodeElement(identitiesInBuilding, heterogeneousElem))
                        {
                            CustomCodeIdentities(identitiesInBuilding);
                        }
                    }
                    else
                    {
                        throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > Identities");
                    }
                }
            }

            Identities = identitiesInBuilding;
            identitiesInBuilding = null;
        }

        public virtual void CustomCodeIdentities(Dictionary<string, string> identities)
        {

        }


        private static string PreparePrincipalUserName(IIdentity Identity, string fromFieldName, bool removeDomain)
        {
            string userName = null;
            PropertyInfo propertyInfo = Identity.GetType().GetProperty(fromFieldName);
            if (propertyInfo != null)
            {
                userName = (string)propertyInfo.GetValue(Identity);
            }
            /*
            if (string.IsNullOrEmpty(userName))
            {
                //SAML2
                userName = ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                TraceManager.Debug("SafranAuthorizationFilterApi", "OnAuthorization", "NameID SAML2 : " + userName);
            }
            */
            if (removeDomain)
            {
                userName = ADHelper.RemoveDomain(userName);
            }

            return userName;
        }



        protected virtual void RefreshUserProfile()
        {
            BaseRefreshUserProfile();
        }

        public void BaseRefreshUserProfile()
        {
            if (userProfileInBuilding == null)
            {
                //Initialize User profile
                userProfileInBuilding = new Dictionary<string, string>();
                string userProfileKey = "";

                HeterogeneousCollection userProfileValues = BIASettingsReader.BIANetSection?.Authentication?.UserProfile;
                if (userProfileValues != null && userProfileValues.Count > 0)
                {
                    foreach (IHeterogeneousConfigurationElement heterogeneousElem in userProfileValues)
                    {
                        if (heterogeneousElem is ValueElement)
                        {
                            SetFromValueElement(userProfileInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem is WebServiceElement)
                        {
                            if (!string.IsNullOrEmpty(userProfileKey)) userProfileKey += ",";
                            userProfileKey += SetFromWebService(userProfileInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem.TagName == "CustomCode")
                        {
                            if (SetFromCustomCodeElement(userProfileInBuilding, heterogeneousElem))
                            {
                                CustomCodeUserProfile(userProfileInBuilding);
                            }
                        }
                        else
                        {
                            throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > UserProfile");
                        }
                    }
                }

                UserProfile = userProfileInBuilding;
                userInfoContainer.userProfileKey = userProfileKey;
            }
        }

        public virtual void CustomCodeUserProfile(Dictionary<string, string> userProfile)
        {

        }


        protected virtual void RefreshRoles()
        {
            BaseRefreshRoles();
        }

        public void BaseRefreshRoles()
        {
            if (rolesInBuilding == null)
            {
                rolesInBuilding = new List<string>();
                if (AdditionnalRoles != null)
                {
                    foreach (string group in AdditionnalRoles)
                    {
                        rolesInBuilding.Add(group);
                    }
                }
                HeterogeneousCollection rolesValues = BIASettingsReader.BIANetSection?.Authentication?.Roles;
                if (rolesValues != null && rolesValues.Count > 0)
                {
                    foreach (IHeterogeneousConfigurationElement heterogeneousElem in rolesValues)
                    {
                        if (heterogeneousElem.TagName == "Value")
                        {
                            SetFromKeyElement(rolesInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem.TagName == "ADRole")
                        {
                            ValueElement ADRole = (ValueElement)heterogeneousElem;
                            if (ADHelper.HasRole(UserGroups, Login, ADRole.Key))
                            {
                                rolesInBuilding.Add(ADRole.Key);
                            }
                        }
                        else if (heterogeneousElem is CustomCodeElement)
                        {
                            if (SetFromCustomCodeElement(rolesInBuilding, heterogeneousElem))
                            {
                                CustomCodeRoles(rolesInBuilding);
                            }
                        }
                        else
                        {
                            throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > Role");
                        }
                    }
                }

                Roles = rolesInBuilding;
                rolesInBuilding = null;
                userInfoContainer.rolesKey = Login;
            }
        }

        public virtual void CustomCodeRoles(List<string> basicRoles)
        {

        }


        /// <summary>
        /// Refresh the user properties
        /// </summary>
        protected virtual void RefreshProperties()
        {
            BaseRefreshProperties();
        }

        /// <summary>
        /// Refresh the user properties (not overidable)
        /// </summary>
        public void BaseRefreshProperties()
        {
            if (propertiesInBuilding == null)
            {
                propertiesInBuilding = new TUserProperties();
                HeterogeneousCollection propertiesValues = BIASettingsReader.BIANetSection?.Authentication?.Properties;
                if (propertiesValues != null && propertiesValues.Count > 0)
                {
                    foreach (IHeterogeneousConfigurationElement heterogeneousElem in propertiesValues)
                    {
                        if (heterogeneousElem is ValueElement)
                        {
                            SetFromValueElement(propertiesInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem is ADFieldElement)
                        {
                            SetFromADFieldElement(propertiesInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem is ObjectFieldElement)
                        {
                            SetFromObjectFieldElement(propertiesInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem is FunctionElement)
                        {
                            SetFromFunctionElement(propertiesInBuilding, heterogeneousElem);
                        }
                        else if (heterogeneousElem.TagName ==  "CustomCode")
                        {
                            if (SetFromCustomCodeElement(propertiesInBuilding, heterogeneousElem))
                            {
                                CustomCodeProperties(propertiesInBuilding);
                            }
                        }
                        else
                        {
                            throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > Properties");
                        }
                    }
                }

                Properties = propertiesInBuilding;
                propertiesInBuilding = default(TUserProperties);
                userInfoContainer.propertiesKey = Login;
            }
        }

        public virtual void CustomCodeProperties(TUserProperties userProperties)
        {
        }


        protected virtual void RefreshLanguage()
        {
            BaseRefreshLanguage();
        }

        public void BaseRefreshLanguage()
        {
            if (languageInBuilding == null)
            {
                HeterogeneousCollection languageValues = BIASettingsReader.BIANetSection?.Authentication?.Language;
                if (languageValues != null && languageValues.Count > 0)
                {
                    foreach (IHeterogeneousConfigurationElement heterogeneousElem in languageValues)
                    {
                        if (heterogeneousElem.TagName == "Value")
                        {
                            ValueElement value = (ValueElement)heterogeneousElem;
                            languageInBuilding = value.Value;
                        }
                        else if (heterogeneousElem.TagName == "Mapping")
                        {
                            MappingCollection mappingCollection = (MappingCollection)heterogeneousElem;
                            PropertyInfo propertyInfo = null;

                            if (mappingCollection?.Key != null)
                            {
                                propertyInfo = Properties.GetType().GetProperty(mappingCollection.Key);
                            }

                            if (propertyInfo != null)
                            {
                                string propertyToMap = propertyInfo.GetValue(Properties).ToString();
                                foreach (KeyValueElement mapping in mappingCollection)
                                {
                                    if (mapping.Key == propertyToMap)
                                    {
                                        languageInBuilding = mapping.Value;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (heterogeneousElem.TagName == "CustomCode")
                        {
                            CustomCodeElement customCode = (CustomCodeElement) heterogeneousElem;
                            if (customCode != null)
                            {
                                if (string.IsNullOrEmpty(customCode.Function))
                                {
                                    languageInBuilding = CustomCodeLanguage();
                                }
                                else
                                {
                                    languageInBuilding = (string)this.GetType().GetMethod(customCode.Function).Invoke(this, null);
                                    if (languageInBuilding != null) break;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > Language");
                        }
                        if (languageInBuilding != null) break;
                    }
                }
                if (languageInBuilding == null)
                {
                    languageInBuilding = "en-US";
                }
                Language = languageInBuilding;
                languageInBuilding = null;
                userInfoContainer.languageKey = Login;
            }
        }

        public virtual string CustomCodeLanguage()
        {
            return null;
        }

        public virtual void FinalizePreparation()
        {

        }

        /// <summary>
        /// Return the profile value or default string
        /// </summary>
        /// <param name="key">key of the dictionnay</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>return the profile value or default string</returns>
        public string GetProfileValueOrDefault(string key, string defaultValue)
        {
            string value;
            if (UserProfile == null)
            {
                return defaultValue;
            }

            return UserProfile.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        /// Check if provided role is in identity's roles.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the role is in identity's roles, false otherwise.</returns>
        public bool IsInRole(string role)
        {
            if ((this.Roles != null) && (this.Roles.Any(r => r == role)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the list of identity's roles.
        /// </summary>
        /// <returns>A formatted string representation of the identity's roles.</returns>
        public string ListOfRole()
        {
            string ret = string.Empty;
            foreach (string role in this.Roles)
            {
                if (ret != string.Empty)
                {
                    ret += ", ";
                }

                ret += role;
            }

            if (ret != string.Empty)
            {
                ret = "(" + ret + ")";
            }

            return ret;
        }

        private object ExtractObjectFieldValue(ObjectFieldElement value)
        {
            object result = null;


            if (value.Object == "UserInfo" || value.Object == "Properties")
            {
                if (!string.IsNullOrEmpty(value.Field))
                {
                    object srcObject = null;
                    if (value.Object == "UserInfo")
                    {
                        srcObject = this;
                    }
                    else if (value.Object == "Properties")
                    {
                        PropertyInfo propertySrcObject = srcObject.GetType().GetProperty(value.Object);
                        srcObject = propertySrcObject.GetValue(srcObject);
                    }

                    if (srcObject != null)
                    {
                        PropertyInfo propertyInfoSrc = srcObject.GetType().GetProperty(value.Field);
                        result = propertyInfoSrc.GetValue(srcObject);
                    }
                }
            }
            else if (value.Object == "Identities")
            {
                string identity = null;
                if (identitiesInBuilding != null) identitiesInBuilding.TryGetValue(value.Field, out identity);
                else Identities.TryGetValue(value.Field, out identity);
                result = identity;
            }

            return result;
        }
        #region SetToObject

        private void SetFromFunctionElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            FunctionElement value = (FunctionElement)heterogeneousElem;
            PropertyInfo propertyInfo = target.GetType().GetProperty(value.Key);
            if (propertyInfo != null)
            {
                if (value.Type != null)
                {
                    if (!string.IsNullOrEmpty(value.Method))
                    {
                        object result = value.Type.GetMethod(value.Method).Invoke(null, null);
                        propertyInfo.SetValue(target, Convert.ChangeType(result, propertyInfo.PropertyType));
                    }
                    else
                    {
                        object result = value.Type.GetProperty(value.Property).GetValue(null, null);
                        propertyInfo.SetValue(target, Convert.ChangeType(result, propertyInfo.PropertyType));
                    }
                }
            }
        }

        private bool SetFromCustomCodeElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            CustomCodeElement CustomCode = (CustomCodeElement)heterogeneousElem;
            if (CustomCode != null)
            {
                if (string.IsNullOrEmpty(CustomCode.Function))
                {
                    return true;
                }
                else
                {
                    this.GetType().GetMethod(CustomCode.Function).Invoke(this, new object[] { target });
                }
            }
            return false;
        }

        private void SetFromADFieldElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ADFieldElement value = (ADFieldElement)heterogeneousElem;
            if (UserPrincipal != null)
            {
                PropertyInfo propertyInfo = target.GetType().GetProperty(value.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(target, Convert.ChangeType(ADHelper.GetProperty(UserPrincipal, value.Adfield, value.MaxLenght, value.Default), propertyInfo.PropertyType));
                }
            }
        }

        private void SetFromValueElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ValueElement value = (ValueElement)heterogeneousElem;
            PropertyInfo propertyInfo = target.GetType().GetProperty(value.Key);
            propertyInfo.SetValue(target, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
        }

        private void SetFromObjectFieldElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ObjectFieldElement value = (ObjectFieldElement)heterogeneousElem;
            PropertyInfo propertyInfo = target.GetType().GetProperty(value.Key);
            if (propertyInfo != null)
            {
                object result = ExtractObjectFieldValue(value);
                propertyInfo.SetValue(target, Convert.ChangeType(result, propertyInfo.PropertyType));
            }
        }
        #endregion
        #region SetToList
        private void SetFromKeyElement(List<string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ValueElement value = (ValueElement)heterogeneousElem;
            target.Add(value.Value);
        }
        #endregion
        #region SetToDictionary
        private void SetFromValueElement(Dictionary<string, string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ValueElement value = (ValueElement)heterogeneousElem;
            target.Add(value.Key, value.Value);
        }

        private string SetFromWebService(Dictionary<string, string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            WebServiceElement webService = (WebServiceElement)heterogeneousElem;
            string userProfileKey = "";
            string parameters = "";
            foreach (ObjectFieldElement parameter in webService)
            {
                object value = ExtractObjectFieldValue(parameter);
                if (!string.IsNullOrEmpty(userProfileKey)) userProfileKey += ",";
                userProfileKey += value;
                if (!string.IsNullOrEmpty(parameters)) parameters += "&";
                else parameters += "?";
                parameters += parameter.Key + "=" + value;
            }

            TraceManager.Info("AUserInfo", "RefreshUserProfile", "Refresh profile from web service " + webService.URL + parameters);
            string url = webService.URL;

            if (url.Contains("$("))
            {
                string pat = @"\$\(([\w\d]+)\)";

                // Instantiate the regular expression object.
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);

                // Match the regular expression pattern against a text string.
                Match m = r.Match(url);
                while (m.Success)
                {
                    string appSetting = ConfigurationManager.AppSettings[m.Groups[1].Value];

                    url = url.Replace(m.Value, appSetting);
                    m = m.NextMatch();
                }
            }

            String profilURL = url + parameters;

            Uri address = new Uri(profilURL);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Timeout = 200000;
            request.Credentials = CredentialCache.DefaultCredentials;

            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseText = reader.ReadToEnd();
                /*var encoding = ASCIIEncoding.ASCII;
                string responseText = "";
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }*/
                response.Close();
                Dictionary<string, string> userProfileReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                foreach (var item in userProfileReturn)
                    target.Add(item.Key, item.Value);
            }
            catch (WebException e)
            {
                TraceManager.Error("AUserInfo", "RefreshUserProfile", "Service user profile do not work : " + address.AbsoluteUri + " Error : " + e.Message);
            }

            return userProfileKey;
        }
        #endregion
    }
}
