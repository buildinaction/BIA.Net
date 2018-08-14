namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business;
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using static BIA.Net.Common.Configuration.AuthenticationElement.LanguageElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.UserPropertiesElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.UserPropertiesElement.ADFieldsCollection;
    using static BIA.Net.Common.Configuration.CommonElement;

    /// <summary>
    /// Class to define identity.
    /// </summary>
    public abstract class AUserInfo<TUserDB> : IPrincipal, IUserInfo
        where TUserDB : IUserDB, new()
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

        protected UserPrincipal userPrincipal { get; set; }

        private List<string> coreRoles = null;
        protected List<string> CoreRoles
        {
            get
            {
                if (coreRoles == null) RefreshCoreRoles();
                return coreRoles;
            }
        }

        private string login = null;
        public virtual string Login
        {
            get {
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
        TUserDB propertiesInBuilding = default(TUserDB) ;

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
                if (userInfoContainer.rolesShouldBeRefreshed) RefreshRoles();
                return userInfoContainer.roles;
            }
            set {
                userInfoContainer.roles = value;
                userInfoContainer.rolesShouldBeRefreshed = false;
                userInfoContainer.rolesRefreshDate = DateTime.Now;
            }
        }
        public virtual TUserDB Properties
        {
            get
            {
                if (userInfoContainer.propertiesShouldBeRefreshed) RefreshProperties();
                return userInfoContainer.properties;
            }
            set {
                userInfoContainer.properties = value;
                userInfoContainer.propertiesShouldBeRefreshed = false;
                userInfoContainer.propertiesRefreshDate = DateTime.Now;
            }
        }

        public virtual Dictionary<string,string> Identities
        {
            get
            {
                if (userInfoContainer.identitiesShouldBeRefreshed) RefreshIdentities();
                return userInfoContainer.identities;
            }
            set {
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
                if (userInfoContainer.userProfileShouldBeRefreshed) RefreshUserProfile();
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
                if (userInfoContainer.languageShouldBeRefreshed) RefreshLanguage();
                return userInfoContainer.language;
            }
            set {
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

            public TUserDB properties = default(TUserDB);
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
            identitiesInBuilding = new Dictionary<string,string>();

            KeyValueCollection ValuesIdentities = BIASettingsReader.BIANetSection?.Authentication?.Identities?.Values;
            if (ValuesIdentities != null && ValuesIdentities.Count >0)
            {
                foreach (KeyValueElement value in ValuesIdentities)
                {
                    identitiesInBuilding.Add(value.Key, value.Value);
                }
            }
            WindowsIdentityCollection WindowsIdentities = BIASettingsReader.BIANetSection?.Authentication?.Identities?.WindowsIdentities;
            if (WindowsIdentities != null && WindowsIdentities.Count>0)
            {
                foreach (WindowsIdentityElement value in WindowsIdentities)
                {
                    if (value.IdentityField != null)
                    {
                        identitiesInBuilding.Add(value.Key, PreparePrincipalUserName(Identity, value.IdentityField, value.RemoveDomain));
                    }
                }
            }
            ObjectFieldsCollection ObjectFields = BIASettingsReader.BIANetSection?.Authentication?.Identities?.Objects;
            if (ObjectFields != null && ObjectFields.Count >0)
            {
                foreach (ObjectFieldElement value in ObjectFields)
                {
                    object result = ExtractObjectFieldValue(value);
                    if (result != null)
                    {
                        identitiesInBuilding.Add(value.Key, result.ToString());
                    }
                }
            }

            Identities = identitiesInBuilding;
            identitiesInBuilding = null;
        }

        /// <summary>
        /// Return true if find in cache
        /// </summary>
        /// <returns>True if find in cache</returns>
        public virtual bool ResetUserInfoContainnerFromCache()
        {
            return false;
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

            if (removeDomain)
            {
                userName = ADHelper.RemoveDomain(userName);
            }

            return userName;
        }

        protected virtual void RefreshUserProfile()
        {
            //Initialize User profile
            Dictionary<string, string> userProfile = new Dictionary<string, string>();
            string userProfileKey = "";

            KeyValueCollection UserProfileValues = BIASettingsReader.BIANetSection?.Authentication?.UserProfile?.Values;
            if (UserProfileValues != null)
            {
                foreach (KeyValueElement value in UserProfileValues)
                {
                    userProfile.Add(value.Key, value.Value);
                }
            }

            if (Roles != null)
            {
                //UserProfil refreshed when it is called (TODO: to Validate)
                userProfileKey = Roles.Contains("Generic") ? null : Login;
            }
            else
            {
                userProfileKey = null;
            }

            if ((userProfileKey != null) && !(string.IsNullOrEmpty(BIASettingsReader.UrlRefreshProfile)))
            {
                TraceManager.Info("AUserInfo", "RefreshUserProfile", "Refresh profile for :" + userProfileKey);
                string refreshProfilUrl = BIASettingsReader.UrlRefreshProfile.ToLower();
                String profilURL = refreshProfilUrl + "?login=" + userProfileKey;

                Uri address = new Uri(profilURL);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                request.Timeout = 200000;
                request.Credentials = CredentialCache.DefaultCredentials;

                HttpWebResponse response;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    var encoding = ASCIIEncoding.ASCII;
                    string responseText = "";
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        responseText = reader.ReadToEnd();
                    }
                    userProfile = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                }
                catch (WebException e)
                {
                    TraceManager.Error("AUserInfo", "RefreshUserProfile", "Service user profile do not work : " + address.AbsoluteUri + " Error : " + e.Message);
                }
            }
            UserProfile = userProfile;
            userInfoContainer.userProfileKey = userProfileKey;
        }

        protected virtual void RefreshCoreRoles()
        {
            //Initialize Roles
            List<string> adUserGroups = new List<string>();

            //TODO for ADadUserGroups = ADHelper.GetGroups(userLogin1stLevel);
            KeyCollection ValuesRoles = BIASettingsReader.BIANetSection?.Authentication?.Roles?.Values;
            if (ValuesRoles != null && ValuesRoles.Count >0)
            {
                foreach (KeyElement value in ValuesRoles)
                {
                    adUserGroups.Add(value.Key);
                }
            }

            KeyValueCollection ADRoles = BIASettingsReader.BIANetSection?.Authentication?.Roles?.AD;
            if (BIASettingsReader.ADRoles.Count>0)
            {
                adUserGroups.AddRange(ADHelper.GetGroups(Login, BIASettingsReader.BIANetSection?.Authentication?.Parameters?.ADDomains));
            }

            if (AdditionnalRoles != null)
            {
                foreach (string group in AdditionnalRoles)
                {
                    if (!adUserGroups.Contains(group)) adUserGroups.Add(group);
                }
            }
            coreRoles = adUserGroups;
        }

        protected virtual void RefreshRoles()
        {
            List<string> userRoles = new List<string>();
            userRoles.AddRange(CoreRoles);

            CustomCodeElement CustomCode = BIASettingsReader.BIANetSection?.Authentication?.Roles?.CustomCode;
            if (!string.IsNullOrEmpty(CustomCode?.Function))
            {
               this.GetType().GetMethod(CustomCode.Function).Invoke(this, new object[] { userRoles });
            }

            Roles = userRoles;
            userInfoContainer.rolesKey = Login;
        }


        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The user information</returns>
        /// <exception cref="System.Exception">You're not authorize to connect to this application: Your aren't in the AD user group.</exception>
        protected virtual void RefreshProperties()
        {
            BuildProperties();
            CustomCodeElement CustomCode = BIASettingsReader.BIANetSection?.Authentication?.Properties?.CustomCode;
            if (!string.IsNullOrEmpty(CustomCode?.Function))
            {
                this.GetType().GetMethod(CustomCode.Function).Invoke(this, new object[] { propertiesInBuilding });
            }

            Properties = propertiesInBuilding;
            propertiesInBuilding = default(TUserDB);
            userInfoContainer.propertiesKey = Login;
        }

        public virtual TUserDB BuildProperties()
        {
            if (propertiesInBuilding == null) propertiesInBuilding = new TUserDB();
            KeyValueCollection UserPropertiesValues = BIASettingsReader.BIANetSection?.Authentication?.Properties?.Values;
            if (UserPropertiesValues != null && UserPropertiesValues.Count > 0)
            {
                foreach (KeyValueElement value in UserPropertiesValues)
                {
                    PropertyInfo propertyInfo = propertiesInBuilding.GetType().GetProperty(value.Key);
                    propertyInfo.SetValue(propertiesInBuilding, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
                }
            }

            SetPropertiesFromAD(Login, propertiesInBuilding.UserAdInDB);
            ObjectFieldsCollection objects = BIASettingsReader.BIANetSection?.Authentication?.Properties?.Objects;
            if (objects != null && objects.Count > 0)
            {
                foreach (ObjectFieldElement value in objects)
                {
                    PropertyInfo propertyInfo = propertiesInBuilding.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        object result = ExtractObjectFieldValue(value);
                        propertyInfo.SetValue(propertiesInBuilding, Convert.ChangeType(result, propertyInfo.PropertyType));
                    }
                }
            }

            FunctionsCollection functions = BIASettingsReader.BIANetSection?.Authentication?.Properties?.Functions;
            if (functions != null && functions.Count > 0)
            {
                foreach (FunctionsCollection.FunctionElement value in functions)
                {
                    PropertyInfo propertyInfo = propertiesInBuilding.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        if (value.Type != null)
                        {
                            if (!string.IsNullOrEmpty(value.Method))
                            {
                                object result = value.Type.GetMethod(value.Method).Invoke(null, null);
                                propertyInfo.SetValue(propertiesInBuilding, Convert.ChangeType(result, propertyInfo.PropertyType));
                            }
                            else
                            {
                                object result = value.Type.GetProperty(value.Property).GetValue(null, null);
                                propertyInfo.SetValue(propertiesInBuilding, Convert.ChangeType(result, propertyInfo.PropertyType));
                            }
                        }
                    }
                }

            }
            return propertiesInBuilding;
        }

        protected virtual void RefreshLanguage()
        {
            string languageCode = null;
            ResolversCollection resolvers = BIASettingsReader.BIANetSection?.Authentication?.Language?.Resolvers;

            if (resolvers != null && resolvers.Count > 0)
            {
                foreach (ResolversCollection.ResolverElement resolver in resolvers)
                {
                    if (resolver.Key == "mapping")
                    {
                        PropertyInfo propertyInfo = null;

                        if (resolver?.Mapping?.Property != null)
                        {
                            propertyInfo = Properties.GetType().GetProperty(resolver.Mapping.Property);
                        }

                        if (propertyInfo != null)
                        {
                            string propertyToMap = propertyInfo.GetValue(Properties).ToString();
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
                    if (resolver.Key == "customCode")
                    {
                        CustomCodeElement customCode = resolver?.CustomCode;
                        if (resolver?.Mapping?.Property != null)
                        {
                            if (customCode != null && customCode.Function != null)
                            {
                                languageCode = (string) this.GetType().GetMethod(customCode.Function).Invoke(this, null);
                                if (languageCode != null) break;
                            }
                        }
                    }
                }
            }
            if (languageCode == null)
            {
                languageCode = BIASettingsReader.BIANetSection?.Authentication?.Language?.Default;
                if (languageCode == null)
                {
                    languageCode = "en-US";
                }
            }
            Language = languageCode;
            userInfoContainer.languageKey = Login;

        }

        public virtual void FinalizePreparation()
        {

        }


        /// <summary>
        /// Set Properties from AD conformly to the parameter in web.config
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="userProperties"></param>
        /// <param name="adFieldsCollection"></param>
        protected virtual void SetPropertiesFromAD<TUserADinDB>(string userLogin, TUserADinDB userProperties)
        {
            ADFieldsCollection adFieldsCollection = BIASettingsReader.BIANetSection?.Authentication?.Properties?.AD;
            CustomCodeElement customCodeAD = BIASettingsReader.BIANetSection?.Authentication?.Properties?.AD?.CustomCode;

            if ((adFieldsCollection != null && adFieldsCollection.Count > 0) || (customCodeAD != null))
            {
                userPrincipal = ADHelper.GetUserFromADs(userLogin);

                if (adFieldsCollection != null && adFieldsCollection.Count > 0)
                {

                    if (userPrincipal != null)
                    {
                        foreach (ADFieldElement value in adFieldsCollection)
                        {
                            PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(ADHelper.GetProperty(userPrincipal, value.Adfield, value.MaxLenght, value.Default), propertyInfo.PropertyType));
                            }
                        }
                    }
                }
            }
            if (customCodeAD != null && customCodeAD.Function != null)
            {
                this.GetType().GetMethod(customCodeAD.Function).Invoke(this, new object[] { userProperties });
            }
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
                        if (propertiesInBuilding != null) srcObject = propertiesInBuilding;
                        else srcObject = Properties;
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
                if (identitiesInBuilding!=null) identitiesInBuilding.TryGetValue(value.Field, out identity);
                else Identities.TryGetValue(value.Field, out identity);
                result = identity;
            }

            return result;
        }
    }
}
