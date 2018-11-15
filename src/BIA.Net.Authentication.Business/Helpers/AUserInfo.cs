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
    using static BIA.Net.Common.Configuration.AuthenticationElement.ParametersElement;
    using static BIA.Net.Common.Configuration.CommonElement;


    /// <summary>
    /// Class to define identity.
    /// </summary>
    public abstract class AUserInfo<TUserProperties> : AUserInfoCommon, IPrincipal, IUserInfo
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


        private bool isUserGroupsFromIISInit = false;
        private List<string> userGroupsFromIIS = null;
        public  List<string> UserGroupsFromIIS
        {
            get
            {
                if (!isUserGroupsFromIISInit)
                {
                    userGroupsFromIIS = ADHelper.GetGroups((WindowsIdentity) Identity);
                    isUserGroupsFromIISInit = true;
                }
                return userGroupsFromIIS;
            }
        }
        private bool isUserGroupsFromADInit = false;
        private List<string> userGroupsFromAD = null;
        public List<string> UserGroupsFromAD
        {
            get
            {
                if (!isUserGroupsFromADInit)
                {
                    userGroupsFromAD = ADHelper.GetGroups(Login);
                    isUserGroupsFromADInit = true;
                }
                return userGroupsFromAD;
            }
        }

        public override string Login
        {
            get
            {
                if (login == null)
                {
                    login = Identities.Get("Login");
                    /*if (login == null)
                    {
                        login = "";
                    }*/
                }
                return login;
            }
        }

        public class IdentityManager
        {
            Dictionary<string, string> IdentitiesContainer = null;
            public IdentityManager()
            {
                IdentitiesContainer = new Dictionary<string, string>();
            }
            public void Set(string key, string value, UserInfoContainer userInfoContainer)
            {
                bool identityChange = false;
                if (!IdentitiesContainer.Keys.Contains(key))
                {
                    IdentitiesContainer.Add(key, value);
                    identityChange = true;
                }
                else
                {
                    if (IdentitiesContainer[key] != value)
                    {
                        IdentitiesContainer[key] = value;
                        identityChange = true;
                    }

                }
                if (identityChange)
                {
                    userInfoContainer.languageShouldBeRefreshed = true;
                    userInfoContainer.propertiesShouldBeRefreshed = true;
                    userInfoContainer.rolesShouldBeRefreshed = true;
                    userInfoContainer.userProfileShouldBeRefreshed = true;
                }

            }
            public string Get (string key)
            {
                string value = null;
                if (value == null)
                {
                        IdentitiesContainer.TryGetValue("Login", out value);
                }
                return value;
            }
        }


        protected IdentityManager identitiesInBuilding = null;
        protected TUserProperties propertiesInBuilding = default(TUserProperties);
        protected string languageInBuilding = null;
        protected Dictionary<string, string> userProfileInBuilding = null;
        protected List<string> rolesInBuilding = null;
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
                    rolesInBuilding = new List<string>();
                    RefreshRolesInBuilding();
                    Roles = rolesInBuilding;
                }
                return userInfoContainer.roles;
            }
            set
            {
                userInfoContainer.roles = value;
                rolesInBuilding = null;
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
                    propertiesInBuilding = new TUserProperties();
                    RefreshPropertiesInBuilding();
                    Properties = propertiesInBuilding;
                }
                return userInfoContainer.properties;
            }
            set
            {
                userInfoContainer.properties = value;
                propertiesInBuilding = default(TUserProperties);
                userInfoContainer.propertiesShouldBeRefreshed = false;
                userInfoContainer.propertiesRefreshDate = DateTime.Now;
            }
        }
        public virtual IdentityManager Identities
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
                    identitiesInBuilding = new IdentityManager();
                    RefreshIdentitiesInBuilding();
                    Identities = identitiesInBuilding;
                }
                return userInfoContainer.identities;
            }
            set
            {
                userInfoContainer.identities = value;
                identitiesInBuilding = null;
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
                    userProfileInBuilding = new Dictionary<string, string>();
                    RefreshUserProfileInBuilding();
                    UserProfile = userProfileInBuilding;
                }
                return userInfoContainer.userProfile;
            }
            set
            {
                userInfoContainer.userProfile = value;
                userProfileInBuilding = null;
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
                    languageInBuilding = "en-US";
                    RefreshLanguageInBuilding();
                    Language = languageInBuilding;
 
                }
                return userInfoContainer.language;
            }
            set
            {
                userInfoContainer.language = value;
                languageInBuilding = null;
                userInfoContainer.languageShouldBeRefreshed = false;
                userInfoContainer.languageRefreshDate = DateTime.Now;
            }
        }




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

            public IdentityManager identities = null;
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

        public void RefreshUser(bool shouldRefreshIdentities = true, bool shouldRefreshUserProperties = true, bool shouldRefreshUserProfile = true, bool shouldRefreshUserLangage = true, bool shouldRefreshUserRoles = true)
        {
            userInfoContainer.identitiesShouldBeRefreshed = shouldRefreshIdentities;
            userInfoContainer.propertiesShouldBeRefreshed = shouldRefreshUserProperties;
            userInfoContainer.userProfileShouldBeRefreshed = shouldRefreshUserProfile;
            userInfoContainer.languageShouldBeRefreshed = shouldRefreshUserLangage;
            userInfoContainer.rolesShouldBeRefreshed = shouldRefreshUserRoles;
        }


        protected virtual void RefreshIdentitiesInBuilding()
        {
            BaseRefreshIdentitiesInBuilding();
        }


        public void BaseRefreshIdentitiesInBuilding()
        {
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
                        if (Identity != null && Identity.IsAuthenticated && value.IdentityField != null)
                        {
                            identitiesInBuilding.Set(value.Key, PreparePrincipalUserName(Identity, value.IdentityField, value.RemoveDomain), userInfoContainer);
                        }
                    }
                    else if (heterogeneousElem is ObjectFieldElement)
                    {
                        ObjectFieldElement value = (ObjectFieldElement)heterogeneousElem;
                        object result = ExtractObjectFieldValue(value);
                        if (result != null)
                        {
                            identitiesInBuilding.Set(value.Key, result.ToString(), userInfoContainer);
                        }
                    }
                    else if (heterogeneousElem.TagName == "CustomCode")
                    {
                        if (SetFromCustomCodeElement(identitiesInBuilding, heterogeneousElem))
                        {
                            CustomCodeIdentities(identitiesInBuilding);
                        }
                    }
                    else if (heterogeneousElem is ClientCertificateInHeaderCollection)
                    {
                    }
                    else
                    {
                        throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > Identities");
                    }
                }
            }
        }

        public virtual void CustomCodeIdentities(IdentityManager identities)
        {

        }

        protected virtual void RefreshUserProfileInBuilding()
        {
            BaseRefreshUserProfileInBuilding();
        }

        public void BaseRefreshUserProfileInBuilding()
        {
            //Initialize User profile
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
                userInfoContainer.userProfileKey = userProfileKey;
            }
        }

        public virtual void CustomCodeUserProfile(Dictionary<string, string> userProfile)
        {

        }


        protected virtual void RefreshRolesInBuilding()
        {
            BaseRefreshRolesInBuilding();
        }

        public void BaseRefreshRolesInBuilding()
        {
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
                        if (HasRole(ADRole.Key))
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
            userInfoContainer.rolesKey = Login;
        }

        public bool HasRole(string role, List<string> adGroupName)
        {
            List<ADGroup> groups = ADHelper.GetADGroupsForRoleOrCreate(role, adGroupName);
            return IsInOneOfThoseGroups(groups);
        }

        private bool HasRole(string role)
        {
            List<ADGroup> groups = ADHelper.GetADGroupsForRole(role);
            return IsInOneOfThoseGroups(groups);
        }

        private bool IsInOneOfThoseGroups(List<ADGroup> groups)
        {
            ADRolesModes? mode = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.ADRolesMode;
            if (mode == null) mode = ADRolesModes.IISGroup;
            foreach (ADGroup adgroup in groups)
            {
                switch (mode)
                {
                    case ADRolesModes.IISGroup:
                        if (IsGroupInList(adgroup.GroupName,UserGroupsFromIIS)) return true;
                        break;
                    case ADRolesModes.ADUserFirst:
                        if (IsGroupInList(adgroup.GroupName,UserGroupsFromAD)) return true;
                        break;
                    case ADRolesModes.ADGroupFirst:
                        if (adgroup.IsUserInGroup(Login)) return true;
                        break;
                    default:
                        return false;
                }

            }
            return false;
        }

        private static bool IsGroupInList(string groupToFind, List<string> groupList)
        {
            if (groupToFind.IndexOfAny(new char[] { '*', '.', '(', ')', '+', '[', ']' }) != -1)
            {
                Regex regex = new Regex("^" + groupToFind + "$");
                foreach (string group in groupList)
                {
                    if (regex.IsMatch(group)) return true;
                }
            }
            else
            {
                return groupList.Contains(groupToFind);
            }
            return false;
        }

        public virtual void CustomCodeRoles(List<string> basicRoles)
        {

        }


        /// <summary>
        /// Refresh the user properties
        /// </summary>
        protected virtual void RefreshPropertiesInBuilding()
        {
            BaseRefreshPropertiesInBuilding();
        }

        /// <summary>
        /// Refresh the user properties (not overidable)
        /// </summary>
        public void BaseRefreshPropertiesInBuilding()
        {
               
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
                    else if (heterogeneousElem is WindowsIdentityElement)
                    {
                        SetFromWindowsIdentityElement(propertiesInBuilding, heterogeneousElem);
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
        }

        public virtual void CustomCodeProperties(TUserProperties userProperties)
        {
        }

        protected virtual void RefreshLanguageInBuilding()
        {
            BaseRefreshLanguageInBuilding();
        }

        public void BaseRefreshLanguageInBuilding()
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
                        if (mappingCollection?.Key != null)
                        {
                            object objectValueToMap = GetObjectValue(this, mappingCollection?.Key);
                            if (objectValueToMap != null)
                            {
                                string sObjectValueToMap = objectValueToMap.ToString();
                                foreach (KeyValueElement mapping in mappingCollection)
                                {
                                    if (mapping.Key == sObjectValueToMap)
                                    {
                                        languageInBuilding = mapping.Value;
                                        break;
                                    }
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
            userInfoContainer.languageKey = Login;
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

    }
}
