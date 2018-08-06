
namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Principal;
    using System.Text;

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
        }
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current identity.
        /// </summary>
        public IIdentity Identity { get; set; }

        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        /// <value>
        /// The roles of the user.
        /// </value>
        public List<string> Roles { get; set; }

        private Dictionary<string, string> userProfile;

        /// <summary>
        /// User properties based on db format
        /// </summary>
        public TUserDB Properties { get; set; }

        public string Login { get; set; }

        public string SecondaryLogin { get; set; }

        /// <summary>
        /// Additionnal Role added by custom connect
        /// </summary>
        public List<string> AdditionnalRoles { get; set; }

        /// <summary>
        /// Gets or sets the user profil
        /// </summary>
        public Dictionary<string, string> UserProfile
        {
            get
            {
                if (userProfile == null) RefreshUserProfile();
                return userProfile;
            }
            set
            {
                userProfile = value;
            }
         }

        /// <summary>
        /// Gets or sets the user name (used to retrieve profile)
        /// </summary>
        public string UserProfileName { get; set; }

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
        /// <summary>
        /// Set the user properties.
        /// </summary>
        /// <param name="userProperties">The user info in db.</param>
        public void SetProperties(TUserDB userProperties)
        {
            Properties = userProperties;
        }

        /// <summary>
        /// Set the Role and add custum role.
        /// </summary>
        /// <param name="adGroups">groups ad</param>
        /// <param name="newUserProperties">new User Properties</param>
        abstract public void SetRoles(List<string> adGroups, TUserDB newUserProperties);

        /*
        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <returns>
        /// the language
        /// </returns>
        public string GetLanguage()
        {
            
            return Properties?.UserAdInDB?.Language;
        }
        */

        #endregion Methods

        public void RefreshUserProfile()
        {
            if ((UserProfileName != null) && !(string.IsNullOrEmpty(BIASettingsReader.UrlRefreshProfile)))
            {
                //Initialize User profile
                userProfile = new Dictionary<string, string>();
                KeyValueCollection UserProfileValues = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProfile?.Values;
                if (UserProfileValues != null)
                {
                    foreach (KeyValueElement value in UserProfileValues)
                    {
                        userProfile.Add(value.Key, value.Value);
                    }
                }

                //TODO for WebService
                /*

                TraceManager.Info("AUserInfo", "RefreshUserProfile", "Refresh profile for :" + UserProfileName);
                string refreshProfilUrl = BIASettingsReader.UrlRefreshProfile.ToLower();
                String profilURL = refreshProfilUrl + "?login=" + UserProfileName;

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
                }*/
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
    }
}
