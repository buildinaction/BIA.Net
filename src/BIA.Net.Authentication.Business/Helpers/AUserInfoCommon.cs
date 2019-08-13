using BIA.Net.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;
using static BIA.Net.Common.Configuration.CommonElement;

namespace BIA.Net.Authentication.Business.Helpers
{
    public class AUserInfoCommon
    {

        #region TemporaryWorkingValues
        /// <summary>
        /// Gets the current identity.
        /// </summary>
        [JsonIgnore]
        public IIdentity Identity { get; set; }
        protected bool isUserPrincipalInit = false;
        protected UserPrincipal userPrincipal { get; set; }

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

        protected string login = null;

        public virtual string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
            }
        }

        #endregion

        protected static string PreparePrincipalUserName(IIdentity Identity, string fromFieldName, bool removeDomain)
        {
            string userName = null;
            var propertyInfo = Identity.GetType().GetProperty(fromFieldName);
            if (propertyInfo != null)
            {
                userName = (string)propertyInfo.GetValue(Identity);
            }
            /*
            if (string.IsNullOrEmpty(userName))
            {
                //SAML2
                userName = ClaimsPrincipal.Current.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                TraceManager.Debug("BIAAuthorizationFilterApi", "OnAuthorization", "NameID SAML2 : " + userName);
            }
            */
            if (removeDomain)
            {
                userName = ADHelper.RemoveDomain(userName);
            }

            return userName;
        }

        #region SetToObject

        protected void SetFromFunctionElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (FunctionElement)heterogeneousElem;
            var propertyInfo = target.GetType().GetProperty(value.Key);
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
        protected void SetFromWindowsIdentityElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (WindowsIdentityElement)heterogeneousElem;
            if (value.IdentityField != null)
            {
                var propertyInfo = target.GetType().GetProperty(value.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(target, Convert.ChangeType(PreparePrincipalUserName(Identity, value.IdentityField, value.RemoveDomain), propertyInfo.PropertyType));
                }
            }
        }

        protected bool SetFromCustomCodeElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var CustomCode = (CustomCodeElement)heterogeneousElem;
            if (CustomCode != null)
            {
                if (string.IsNullOrEmpty(CustomCode.Function))
                {
                    return true;
                }
                else
                {
                    GetType().GetMethod(CustomCode.Function).Invoke(this, new object[] { target });
                }
            }
            return false;
        }

        protected void SetFromADFieldElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (ADFieldElement)heterogeneousElem;
            if (UserPrincipal != null)
            {
                string valueToSet = ADHelper.GetProperty(UserPrincipal, value.Adfield, value.MaxLenght, value.Default);
                SetObjectValue(target, value.Key, valueToSet);
            }
        }


        protected void SetFromValueElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (ValueElement)heterogeneousElem;
            var propertyInfo = target.GetType().GetProperty(value.Key);
            propertyInfo.SetValue(target, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
        }

        protected void SetFromObjectFieldElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (ObjectFieldElement)heterogeneousElem;
            var propertyInfo = target.GetType().GetProperty(value.Key);
            if (propertyInfo != null)
            {
                object result = ExtractObjectFieldValue(value);
                propertyInfo.SetValue(target, Convert.ChangeType(result, propertyInfo.PropertyType));
            }
        }
        #endregion
        #region SetToList
        protected void SetFromKeyElement(List<string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (ValueElement)heterogeneousElem;
            target.Add(value.Value);
        }
        #endregion
        #region SetToDictionary
        protected void SetFromValueElement(Dictionary<string, string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var value = (ValueElement)heterogeneousElem;
            target.Add(value.Key, value.Value);
        }

        protected string SetFromWebService(Dictionary<string, string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            var webService = (WebServiceElement)heterogeneousElem;
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
                var r = new Regex(pat, RegexOptions.IgnoreCase);

                // Match the regular expression pattern against a text string.
                var m = r.Match(url);
                while (m.Success)
                {
                    string appSetting = ConfigurationManager.AppSettings[m.Groups[1].Value];

                    url = url.Replace(m.Value, appSetting);
                    m = m.NextMatch();
                }
            }

            string profilURL = url + parameters;
            try
            {
                var address = new Uri(profilURL);
                var request = (HttpWebRequest)System.Net.WebRequest.Create(address);
                request.Timeout = 200000;
                request.Credentials = CredentialCache.DefaultCredentials;

                HttpWebResponse response;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    var reader = new StreamReader(response.GetResponseStream());
                    string responseText = reader.ReadToEnd();
                    /*var encoding = ASCIIEncoding.ASCII;
                    string responseText = "";
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        responseText = reader.ReadToEnd();
                    }*/
                    response.Close();
                    var userProfileReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
                    foreach (var item in userProfileReturn)
                        target.Add(item.Key, item.Value);
                }
                catch (WebException e)
                {
                    TraceManager.Error("AUserInfo", "RefreshUserProfile", "Service user profile do not work : " + address.AbsoluteUri + " Error : " + e.Message);
                }
            }
            catch (Exception ex)
            {
                TraceManager.Error("AUserInfo", "RefreshUserProfile", "Impossible to generate address to access to the Service user profile: " + profilURL + " Error : " + ex.Message);
            }

            return userProfileKey;
        }
        #endregion



        protected static void DownToParentObject(ref string key, ref object parentTarget, bool createIfNotExist)
        {
            while (key.Contains(".") && parentTarget != null)
            {
                string parentObjectName = key.Split('.')[0];
                var propertyInfoParent = parentTarget.GetType().GetProperty(parentObjectName);
                object parentObject = null;
                if (propertyInfoParent != null)
                {
                    parentObject = propertyInfoParent.GetValue(parentTarget);
                    if (parentObject == null && createIfNotExist)
                    {
                        parentObject = Activator.CreateInstance(propertyInfoParent.PropertyType);
                        propertyInfoParent.SetValue(parentTarget, parentObject);
                    }
                }
                key = key.Substring(parentObjectName.Length + 1);
                parentTarget = parentObject;
            }
        }

        protected virtual object ExtractObjectFieldValue(ObjectFieldElement value)
        {
            return GetObjectValue(this, value.Source);
        }


        private static void SetObjectValue(object parentTarget, string targetField, string valueToSet)
        {
            string key = String.Copy(targetField);
            DownToParentObject(ref key, ref parentTarget, true);

            if (parentTarget != null)
            {
                var propertyInfo = parentTarget.GetType().GetProperty(key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(parentTarget, Convert.ChangeType(valueToSet, propertyInfo.PropertyType));
                }
            }
        }

        protected virtual object GetObjectValue(object parentSource, string source)
        {
            object result = null;
            string key = String.Copy(source);
            DownToParentObject(ref key, ref parentSource, false);

            if (parentSource != null)
            {
                var dict = parentSource as IDictionary;
                if (dict != null)
                {
                    try
                    {
                        result = dict[key];
                    }
                    catch (Exception)
                    {
                        result = null;
                    }
                }
                else
                {
                    var propertyInfoSrc = parentSource.GetType().GetProperty(key);
                    result = propertyInfoSrc.GetValue(parentSource);
                }
            }
            return result;
        }
    }
}
