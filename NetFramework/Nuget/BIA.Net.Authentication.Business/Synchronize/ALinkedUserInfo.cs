namespace BIA.Net.Authentication.Business.Synchronize
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using System;
    using System.Collections.Generic;
    using static BIA.Net.Common.Configuration.AuthenticationElement.LanguageElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.ParametersElement;
    using static BIA.Net.Common.Configuration.CommonElement;

    /// <summary>
    /// Class to define identity.
    /// </summary>
    public abstract class ALinkedUserInfo<TLinkedProperties> : AUserInfoCommon
        where TLinkedProperties : ILinkedProperties, new()
    {
        protected TLinkedProperties linkedPropertiesInBuilding = default(TLinkedProperties);
        TLinkedProperties linkedProperties = default(TLinkedProperties);

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AUserLinkedInfo"/> class.
        /// </summary>
        public ALinkedUserInfo()
        {
        }
        #endregion Constructors

        #region Properties
        public virtual List<string> Roles
        {
            get; set;
        }

        public virtual TLinkedProperties LinkedProperties
        {
            get
            {
                if (linkedPropertiesInBuilding != null)
                {
                    TraceManager.Debug("Return the linkedPropertiesInBuilding");
                    return linkedPropertiesInBuilding;
                }

                if (linkedProperties != null)
                {
                    return linkedProperties;
                }

                linkedPropertiesInBuilding = new TLinkedProperties();
                RefreshLinkedPropertiesInBuilding();
                LinkedProperties = linkedPropertiesInBuilding;

                return linkedProperties;
            }
            set
            {
                linkedProperties = value;
                linkedPropertiesInBuilding = default(TLinkedProperties);
            }
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
        protected virtual void RefreshLinkedPropertiesInBuilding()
        {
            BaseRefreshLinkedPropertiesInBuilding();
        }

        /// <summary>
        /// Refresh the user properties (not overidable)
        /// </summary>
        public void BaseRefreshLinkedPropertiesInBuilding()
        {
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
            }
        }

        public virtual void CustomCodeLinkedProperties(TLinkedProperties linkedProperties)
        {
        }
    }
}
