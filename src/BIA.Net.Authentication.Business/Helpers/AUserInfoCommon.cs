using BIA.Net.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static BIA.Net.Common.Configuration.CommonElement;

namespace BIA.Net.Authentication.Business.Helpers
{
    public class AUserInfoCommon
    {

        #region TemporaryWorkingValues
        /// <summary>
        /// Gets the current identity.
        /// </summary>
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

        public bool IsInAd()
        {
            return UserPrincipal != null;
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
        protected void SetFromWindowsIdentityElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            WindowsIdentityElement value = (WindowsIdentityElement)heterogeneousElem;
            if (value.IdentityField != null)
            {
                PropertyInfo propertyInfo = target.GetType().GetProperty(value.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(target, Convert.ChangeType(PreparePrincipalUserName(Identity, value.IdentityField, value.RemoveDomain), propertyInfo.PropertyType));
                }
            }
        }

        protected bool SetFromCustomCodeElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
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

        protected void SetFromADFieldElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ADFieldElement value = (ADFieldElement)heterogeneousElem;
            if (UserPrincipal != null)
            {
                string valueToSet = ADHelper.GetProperty(UserPrincipal, value.Adfield, value.MaxLenght, value.Default);
                SetObjectValue(target, value.Key, valueToSet);
            }
        }


        protected void SetFromValueElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ValueElement value = (ValueElement)heterogeneousElem;
            PropertyInfo propertyInfo = target.GetType().GetProperty(value.Key);
            propertyInfo.SetValue(target, Convert.ChangeType(value.Value, propertyInfo.PropertyType));
        }

        protected void SetFromObjectFieldElement(object target, IHeterogeneousConfigurationElement heterogeneousElem)
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
        protected void SetFromKeyElement(List<string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ValueElement value = (ValueElement)heterogeneousElem;
            target.Add(value.Value);
        }
        #endregion
        #region SetToDictionary
        protected void SetFromValueElement(Dictionary<string, string> target, IHeterogeneousConfigurationElement heterogeneousElem)
        {
            ValueElement value = (ValueElement)heterogeneousElem;
            target.Add(value.Key, value.Value);
        }

        protected string SetFromWebService(Dictionary<string, string> target, IHeterogeneousConfigurationElement heterogeneousElem)
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
            HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(address);
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



        protected static void DownToParentObject(ref string key, ref object parentTarget, bool createIfNotExist)
        {
            while (key.Contains(".") && parentTarget != null)
            {
                string parentObjectName = key.Split('.')[0];
                PropertyInfo propertyInfoParent = parentTarget.GetType().GetProperty(parentObjectName);
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
                PropertyInfo propertyInfo = parentTarget.GetType().GetProperty(key);
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
                IDictionary dict = parentSource as IDictionary;
                if (dict != null)
                {
                    try
                    {
                        result = dict[key];
                    }
                    catch(Exception e)
                    {
                        result = null;
                    }
                }
                else
                {
                    PropertyInfo propertyInfoSrc = parentSource.GetType().GetProperty(key);
                    result = propertyInfoSrc.GetValue(parentSource);
                }
            }
            return result;
        }
    }
}
