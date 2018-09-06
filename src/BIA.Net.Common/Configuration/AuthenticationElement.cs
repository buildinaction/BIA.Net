

namespace BIA.Net.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using static BIA.Net.Common.Configuration.CommonElement;

    public class AuthenticationElement : ConfigurationElement
    {
        [ConfigurationProperty("Parameters", IsRequired = false)]
        public ParametersElement Parameters
        {
            get { return (ParametersElement)this["Parameters"]; }
            set { this["Parameters"] = value; }
        }

        [ConfigurationProperty("Identities", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HeterogeneousCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public HeterogeneousCollection Identities
        {
            get { return (HeterogeneousCollection)this["Identities"]; }
            set { this["Identities"] = value; }
        }

        [ConfigurationProperty("Properties", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HeterogeneousCollection),
             AddItemName = "add",
             ClearItemsName = "clear",
             RemoveItemName = "remove")]
        public HeterogeneousCollection Properties
        {
            get { return (HeterogeneousCollection)this["Properties"]; }
            set { this["Properties"] = value; }
        }

        [ConfigurationProperty("LinkedProperties", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HeterogeneousCollection),
             AddItemName = "add",
             ClearItemsName = "clear",
             RemoveItemName = "remove")]
        public HeterogeneousCollection LinkedProperties
        {
            get { return (HeterogeneousCollection)this["LinkedProperties"]; }
            set { this["LinkedProperties"] = value; }
        }

        [ConfigurationProperty("Language", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HeterogeneousCollection),
             AddItemName = "add",
             ClearItemsName = "clear",
             RemoveItemName = "remove")]
        public HeterogeneousCollection Language
        {
            get { return (HeterogeneousCollection)this["Language"]; }
            set { this["Language"] = value; }
        }
        [ConfigurationProperty("Roles", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HeterogeneousCollection),
             AddItemName = "add",
             ClearItemsName = "clear",
             RemoveItemName = "remove")]
        public HeterogeneousCollection Roles
        {
            get { return (HeterogeneousCollection)this["Roles"]; }
            set { this["Roles"] = value; }
        }
        [ConfigurationProperty("UserProfile", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(HeterogeneousCollection),
             AddItemName = "add",
             ClearItemsName = "clear",
             RemoveItemName = "remove")]
        public HeterogeneousCollection UserProfile
        {
            get { return (HeterogeneousCollection)this["UserProfile"]; }
            set { this["UserProfile"] = value; }
        }

        public class ParametersElement : ConfigurationElement
        {
            [ConfigurationProperty("Values", IsDefaultCollection = false)]
            [ConfigurationCollection(typeof(KeyValueCollection),
                AddItemName = "add",
                ClearItemsName = "clear",
                RemoveItemName = "remove")]
            public KeyValueCollection Values
            {
                get { return (KeyValueCollection)this["Values"]; }
                set { this["Values"] = value; }
            }

            public enum ADRolesModes
            {
                IISGroup=1,
                ADUserFirst=2,
                ADGroupFirst=3,
            }

            private ADRolesModes _adRolesMode = 0;
            /// <summary>
            /// Return the mode to use to determine if user have a role with tag ADRole
            /// </summary>
            public ADRolesModes ADRolesMode
            {
                get
                {
                    if (_adRolesMode == 0)
                    {
                        KeyValueElement adRolesMode = Values?.GetElemByKey("ADRolesMode");
                        if (adRolesMode != null)
                        {
                            switch (adRolesMode.Value)
                            {
                                case "IISGroup":
                                    _adRolesMode = ADRolesModes.IISGroup;
                                    break;
                                case "ADUserFirst":
                                    _adRolesMode = ADRolesModes.ADUserFirst;
                                    break;
                                case "ADGroupFirst":
                                    _adRolesMode = ADRolesModes.ADGroupFirst;
                                    break;
                                default:
                                    throw new ConfigurationErrorsException("ADRolesMode not managed :" + adRolesMode.Value + ". Authorized values are IISGroup, ADUserFirst or ADGroupFirst.");
                            }
                        }
                        else
                        {
                            _adRolesMode = ADRolesModes.IISGroup;
                        }
                    }
                    return _adRolesMode;
                }
            }
            private List<string> _adDomaines = null;
            public List<string> ADDomains
            {
                get
                {
                    if (_adDomaines == null)
                    {
                        KeyValueElement adDomaines = Values?.GetElemByKey("ADDomains");
                        if (adDomaines != null)
                        {
                            _adDomaines = adDomaines.Value.Split(',').ToList<string>();
                        }
                        else
                        {
                            _adDomaines = new List<string>();
                        }
                    }
                    return _adDomaines;
                }
            }

            private string _caching = null;
            public string Caching
            {
                get
                {
                    if (_caching == null)
                    {
                        KeyValueElement caching = Values?.GetElemByKey("Caching");
                        if (caching != null)
                        {
                            _caching = caching.Value;
                        }
                        else
                        {
                            _caching = "";
                        }
                    }
                    return _caching;
                }
            }
        }

        public class LanguageElement : ConfigurationElement
        {
            [ConfigurationProperty("default", IsRequired = true)]
            public string Default
            {
                get { return (string)this["default"]; }
                set { this["default"] = value; }
            }

            [ConfigurationProperty("Resolvers", IsDefaultCollection = false)]
            [ConfigurationCollection(typeof(ResolversCollection),
                AddItemName = "add",
                ClearItemsName = "clear",
                RemoveItemName = "remove")]
            public ResolversCollection Resolvers
            {
                get { return (ResolversCollection)this["Resolvers"]; }
                set { this["Resolvers"] = value; }
            }

            public class ResolversCollection : ConfigCollection<ResolversCollection.ResolverElement>
            {
                public class ResolverElement : KeyElement
                {
                    [ConfigurationProperty("Mapping", IsDefaultCollection = false)]
                    [ConfigurationCollection(typeof(MappingCollection),
                        AddItemName = "add",
                        ClearItemsName = "clear",
                        RemoveItemName = "remove")]
                    public MappingCollection Mapping
                    {
                        get { return (MappingCollection)this["Mapping"]; }
                        set { this["Mapping"] = value; }
                    }


                }
            }
        }
    }
}
