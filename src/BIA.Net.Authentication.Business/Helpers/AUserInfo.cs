namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using static BIA.Net.Common.Configuration.AuthenticationElement.IdentityElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.IdentityElement.ADIdentityValueCollection;
    using static BIA.Net.Common.Configuration.AuthenticationElement.UserPropertiesElement;
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

        /// <summary>
        /// Gets the current identity.
        /// </summary>
        public IIdentity Identity { get; set; }

        List<string> coreRoles = null;
        public List<string> CoreRoles
        {
            get
            {
                if (coreRoles == null) RefreshCoreRoles();
                return coreRoles;
            }
        }


        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        /// <value>
        /// The roles of the user.
        /// </value>
        public List<string> Roles 
        {
            get
            {
                if (userInfoContainer.rolesShouldBeRefreshed) RefreshRoles();
                return userInfoContainer.roles;
            }
            set { userInfoContainer.roles = value;  }
        }
        public TUserDB Properties
        {
            get
            {
                if (userInfoContainer.propertiesShouldBeRefreshed) RefreshProperties();
                return userInfoContainer.properties;
            }
            set { userInfoContainer.properties = value; }
        }

        public string Login
        {
            get
            {
                if (userInfoContainer.loginShouldBeRefreshed) RefreshLogin();
                return userInfoContainer.login;
            }
            set { userInfoContainer.login = value; }
        }

        public virtual string LocalUserLogin
        {
            get
            {
                if (userInfoContainer.loginShouldBeRefreshed) RefreshLogin();
                return userInfoContainer.localUserLogin;
            }
            set { userInfoContainer.localUserLogin = value; }
        }

        /// <summary>
        /// Additionnal Role added by custom connect
        /// </summary>
        public List<string> AdditionnalRoles { get; set; }


        public UserInfoContainer userInfoContainer;
        /// <summary>
        /// Gets or sets the user profil
        /// </summary>
        public Dictionary<string, string> UserProfile
        {
            get
            {
                if (userInfoContainer.userProfileShouldBeRefreshed) RefreshUserProfile();
                return userInfoContainer.userProfile;
            }
            set
            {
                userInfoContainer.userProfile = value;
            }
         }

        /// <summary>
        /// Gets or sets the user name (used to retrieve profile)
        /// </summary>

        public class UserInfoContainer
        {
            #region login
            public DateTime loginRefreshDate = DateTime.MinValue;
            public bool loginShouldBeRefreshed = true;
            public string loginKey;

            public string login = null;
            public string localUserLogin = null;
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

        public virtual void RefreshLogin()
        {
            this.Login = null;
            this.LocalUserLogin = null;
            KeyValueCollection ValuesIdentities = BIASettingsReader.BIANetSection?.Authentication?.Identity?.Values;
            if (ValuesIdentities != null)
            {
                foreach (KeyValueElement value in ValuesIdentities)
                {
                    PropertyInfo propertyInfo = this.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(this, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
                    }
                }
            }
            ADIdentityValueCollection AdIdentities = BIASettingsReader.BIANetSection?.Authentication?.Identity?.AD;
            if (AdIdentities != null)
            {
                foreach (ADIdentityValueElement value in AdIdentities)
                {
                    if (value.IdentityField != null)
                    {
                        //The user.Identity is the login send by Windows identity
                        PropertyInfo propertyInfo = this.GetType().GetProperty(value.Key);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(this, Convert.ChangeType(PreparePrincipalUserName(Identity, value.IdentityField, value.RemoveDomain), propertyInfo.PropertyType));
                        }
                    }
                }
            }
            userInfoContainer.loginShouldBeRefreshed = false;
            userInfoContainer.loginRefreshDate = DateTime.Now;
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

        public virtual void RefreshUserProfile()
        {
            //Initialize User profile
            userInfoContainer.userProfile = new Dictionary<string, string>();
            KeyValueCollection UserProfileValues = BIASettingsReader.BIANetSection?.Authentication?.UserProfile?.Values;
            if (UserProfileValues != null)
            {
                foreach (KeyValueElement value in UserProfileValues)
                {
                    userInfoContainer.userProfile.Add(value.Key, value.Value);
                }
            }

            if (Roles != null)
            {
                //UserProfil refreshed when it is called (TODO: to Validate)
                userInfoContainer.userProfileKey = Roles.Contains("Generic") ? null : Login;
            }
            else
            {
                userInfoContainer.userProfileKey = null;
            }

            if ((userInfoContainer.userProfileKey != null) && !(string.IsNullOrEmpty(BIASettingsReader.UrlRefreshProfile)))
            {
                TraceManager.Info("AUserInfo", "RefreshUserProfile", "Refresh profile for :" + userInfoContainer.userProfileKey);
                string refreshProfilUrl = BIASettingsReader.UrlRefreshProfile.ToLower();
                String profilURL = refreshProfilUrl + "?login=" + userInfoContainer.userProfileKey;

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
                    userInfoContainer.userProfile = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                }
                catch (WebException e)
                {
                    TraceManager.Error("AUserInfo", "RefreshUserProfile", "Service user profile do not work : " + address.AbsoluteUri + " Error : " + e.Message);
                }
            }
            userInfoContainer.userProfileShouldBeRefreshed = false;
            userInfoContainer.userProfileRefreshDate = DateTime.Now;
        }

        public virtual void RefreshCoreRoles()
        {
            //Initialize Roles
            List<string> adUserGroups = new List<string>();

            //TODO for ADadUserGroups = ADHelper.GetGroups(userLogin1stLevel);
            KeyCollection ValuesRoles = BIASettingsReader.BIANetSection?.Authentication?.Roles?.Values;
            if (ValuesRoles != null)
            {
                foreach (KeyElement value in ValuesRoles)
                {
                    adUserGroups.Add(value.Key);
                }
            }

            KeyValueCollection ADRoles = BIASettingsReader.BIANetSection?.Authentication?.Roles?.AD;
            if (ADRoles != null)
            {
                adUserGroups.AddRange(ADHelper.GetGroups(Login, ADRoles, BIASettingsReader.BIANetSection?.Authentication?.Parameters?.AD?.Domains));
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

        public virtual void RefreshRoles()
        {
            List<string> userRoles = CoreRoles;

            CustomCodeElement CustomCode = BIASettingsReader.BIANetSection?.Authentication?.Roles?.CustomCode;
            if (CustomCode != null && CustomCode.Function != null)
            {
               this.GetType().GetMethod(CustomCode.Function).Invoke(this, new object[] { userRoles });
            }

            userInfoContainer.roles = userRoles;
            userInfoContainer.rolesShouldBeRefreshed = false;
            userInfoContainer.rolesRefreshDate = DateTime.Now;
        }


        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The user information</returns>
        /// <exception cref="System.Exception">You're not authorize to connect to this application: Your aren't in the AD user group.</exception>
        public virtual void RefreshProperties()
        {
            TUserDB userProperties = default(TUserDB);
            //Initialize Properties

            userProperties = GetBasicProperties();


            CustomCodeElement CustomCode = BIASettingsReader.BIANetSection?.Authentication?.UserProperties?.CustomCode;
            if (CustomCode != null && CustomCode.Function != null)
            {
                this.GetType().GetMethod(CustomCode.Function).Invoke(this, new object[] { userProperties });
            }

            userInfoContainer.properties = userProperties;
            userInfoContainer.propertiesShouldBeRefreshed = false;
            userInfoContainer.propertiesRefreshDate = DateTime.Now;
        }

        public virtual TUserDB GetBasicProperties()
        {
            TUserDB userProperties = new TUserDB();

            KeyValueCollection UserPropertiesValues = BIASettingsReader.BIANetSection?.Authentication?.UserProperties?.Values;
            if (UserPropertiesValues != null && UserPropertiesValues.Count > 0)
            {
                foreach (KeyValueElement value in UserPropertiesValues)
                {
                    PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                    propertyInfo.SetValue(userProperties, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
                }
            }

            ADHelper.SetPropertiesFromAD(Login, userProperties.UserAdInDB);
            ObjectFieldsCollection objects = BIASettingsReader.BIANetSection?.Authentication?.UserProperties?.Objects;
            if (objects != null && objects.Count > 0)
            {
                foreach (ObjectFieldElement value in objects)
                {
                    PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        if (!string.IsNullOrEmpty(value.Object))
                        {
                            if (value.Object == "UserInfo")
                            {
                                PropertyInfo propertyInfoSrc = this.GetType().GetProperty(value.Field);
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(propertyInfoSrc.GetValue(this), propertyInfo.PropertyType));
                            }
                        }
                    }
                }
            }

            FunctionsCollection functions = BIASettingsReader.BIANetSection?.Authentication?.UserProperties?.Functions;
            if (functions != null && functions.Count > 0)
            {
                foreach (FunctionsCollection.FunctionElement value in functions)
                {
                    PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        if (value.Type != null)
                        {
                            if (!string.IsNullOrEmpty(value.Method))
                            {
                                object result = value.Type.GetMethod(value.Method).Invoke(null, null);
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(result, propertyInfo.PropertyType));
                            }
                            else
                            {
                                object result = value.Type.GetProperty(value.Property).GetValue(null, null);
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(result, propertyInfo.PropertyType));
                            }
                        }
                    }
                }

            }
            return userProperties;

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
    }
}
