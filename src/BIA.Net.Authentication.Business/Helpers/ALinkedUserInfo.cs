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
    public abstract class ALinkedUserInfo<TLinkedProperties> : AUserInfoCommon
        where TLinkedProperties : ILinkedProperties, new()
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AUserLinkedInfo"/> class.
        /// </summary>
        public ALinkedUserInfo()
        {
        }
        #endregion Constructors

        #region Properties

        public virtual TLinkedProperties LinkedProperties
        {
            get; set;
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

        /// <summary>
        /// Refresh the user properties
        /// </summary>
        protected virtual void RefreshLinkedProperties()
        {
            BaseRefreshLinkedProperties();
        }

        /// <summary>
        /// Refresh the user properties (not overidable)
        /// </summary>
        public void BaseRefreshLinkedProperties()
        {
            TLinkedProperties linkedPropertiesInBuilding = new TLinkedProperties();
            HeterogeneousCollection linkedPropertiesValues = BIASettingsReader.BIANetSection?.Authentication?.LinkedProperties;
            if (linkedPropertiesValues != null && linkedPropertiesValues.Count > 0)
            {
                foreach (IHeterogeneousConfigurationElement heterogeneousElem in linkedPropertiesValues)
                {
                    if (heterogeneousElem is ValueElement)
                    {
                        SetFromValueElement(linkedPropertiesInBuilding, heterogeneousElem);
                    }
                    else if (heterogeneousElem is ADFieldElement)
                    {
                        SetFromADFieldElement(linkedPropertiesInBuilding, heterogeneousElem);
                    }
                    else if (heterogeneousElem is ObjectFieldElement)
                    {
                        SetFromObjectFieldElement(linkedPropertiesInBuilding, heterogeneousElem);
                    }
                    else if (heterogeneousElem is FunctionElement)
                    {
                        SetFromFunctionElement(linkedPropertiesInBuilding, heterogeneousElem);
                    }
                    else if (heterogeneousElem is WindowsIdentityElement)
                    {
                        SetFromWindowsIdentityElement(linkedPropertiesInBuilding, heterogeneousElem);
                    }
                    else if (heterogeneousElem.TagName == "CustomCode")
                    {
                        if (SetFromCustomCodeElement(linkedPropertiesInBuilding, heterogeneousElem))
                        {
                            CustomCodeLinkedProperties(linkedPropertiesInBuilding);
                        }
                    }
                    else
                    {
                        throw new Exception("Tag " + heterogeneousElem.TagName + " not implemented for Authentication > Properties");
                    }
                }

                LinkedProperties = linkedPropertiesInBuilding;
            }
        }

        public virtual void CustomCodeLinkedProperties(TLinkedProperties linkedProperties)
        {
        }
    }
}
