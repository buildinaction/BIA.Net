

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
        [ConfigurationProperty("Sources", IsRequired = false)]
        public SourcesElement Sources
        {
            get { return (SourcesElement)this["Sources"]; }
            set { this["Sources"] = value; }
        }
        public class ParametersElement : ConfigurationElement
        {
            [ConfigurationProperty("AD", IsRequired = false)]
            public ADParametersElement AD
            {
                get { return (ADParametersElement)this["AD"]; }
                set { this["AD"] = value; }
            }
            public class ADParametersElement : ConfigurationElement
            {
                [ConfigurationProperty("Domains", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public KeyValueCollection Domains
                {
                    get { return (KeyValueCollection)this["Domains"]; }
                    set { this["Domains"] = value; }
                }
            }
        }
        public class SourcesElement : ConfigurationElement
        {
            [ConfigurationProperty("Identity", IsRequired = false)]
            public IdentityElement Identity
            {
                get { return (IdentityElement)this["Identity"]; }
                set { this["Identity"] = value; }
            }
            [ConfigurationProperty("UserProperties", IsRequired = false)]
            public UserPropertiesElement UserProperties
            {
                get { return (UserPropertiesElement)this["UserProperties"]; }
                set { this["UserProperties"] = value; }
            }
            [ConfigurationProperty("Language", IsRequired = false)]
            public LanguageElement Language
            {
                get { return (LanguageElement)this["Language"]; }
                set { this["Language"] = value; }
            }
            [ConfigurationProperty("Roles", IsRequired = false)]
            public RolesElement Roles
            {
                get { return (RolesElement)this["Roles"]; }
                set { this["Roles"] = value; }
            }
            [ConfigurationProperty("UserProfile", IsRequired = false)]
            public UserProfileElement UserProfile
            {
                get { return (UserProfileElement)this["UserProfile"]; }
                set { this["UserProfile"] = value; }
            }
            public class IdentityElement : ConfigurationElement
            {
                [ConfigurationProperty("Values", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public KeyValueCollection Values
                {
                    get { return (KeyValueCollection)this["Values"]; }
                    set { this["Values"] = value; }
                }
                [ConfigurationProperty("AD", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public ADIdentityValueCollection AD
                {
                    get { return (ADIdentityValueCollection)this["AD"]; }
                    set { this["AD"] = value; }
                }

                public class ADIdentityValueCollection : ConfigurationElementCollection
                {
                    protected override ConfigurationElement CreateNewElement()
                    {
                        return new ADIdentityValueElement();
                    }

                    protected override Object GetElementKey(ConfigurationElement element)
                    {
                        return ((ADIdentityValueElement)element).Key;
                    }
                    public class ADIdentityValueElement : ConfigurationElement
                    {
                        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
                        public string Key
                        {
                            get
                            {
                                return (string)this["key"];
                            }
                            set
                            {
                                this["key"] = value;
                            }
                        }
                        [ConfigurationProperty("identityField", IsRequired = false, IsKey = false)]
                        public string IdentityField
                        {
                            get
                            {
                                return (string)this["identityField"];
                            }
                            set
                            {
                                this["identityField"] = value;
                            }
                        }
                        [ConfigurationProperty("removeDomain", IsRequired = false, IsKey = false)]
                        public bool RemoveDomain
                        {
                            get
                            {
                                return (bool)(this["removeDomain"] == null ? false : this["removeDomain"]);
                            }
                            set
                            {
                                this["removeDomain"] = value;
                            }
                        }
                    }
                }
            }

            public class UserPropertiesElement : ConfigurationElement
            {
                [ConfigurationProperty("Values", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public KeyValueCollection Values
                {
                    get { return (KeyValueCollection)this["Values"]; }
                    set { this["Values"] = value; }
                }

                [ConfigurationProperty("AD", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                AddItemName = "add",
                ClearItemsName = "clear",
                RemoveItemName = "remove")]
                public ADFieldsCollection AD
                {
                    get { return (ADFieldsCollection)this["AD"]; }
                    set { this["AD"] = value; }
                }

                [ConfigurationProperty("Objects", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                AddItemName = "add",
                ClearItemsName = "clear",
                RemoveItemName = "remove")]
                public ObjectFieldsCollection Objects
                {
                    get { return (ObjectFieldsCollection)this["Objects"]; }
                    set { this["Object"] = value; }
                }

                [ConfigurationProperty("Functions", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                AddItemName = "add",
                ClearItemsName = "clear",
                RemoveItemName = "remove")]
                public FunctionsCollection Functions
                {
                    get { return (FunctionsCollection)this["Functions"]; }
                    set { this["Functions"] = value; }
                }

                [ConfigurationProperty("Service", IsRequired = false)]
                public ServiceElement Service
                {
                    get { return (ServiceElement)this["Service"]; }
                    set { this["Service"] = value; }
                }

                [ConfigurationProperty("CustomCodeAD", IsRequired = false)]
                public MethodFunctionElement CustomCodeAD
                {
                    get { return (MethodFunctionElement)this["CustomCodeAD"]; }
                    set { this["CustomCodeAD"] = value; }
                }

                public class ADFieldsCollection : ConfigurationElementCollection
                {
                    protected override ConfigurationElement CreateNewElement()
                    {
                        return new ADFieldElement();
                    }

                    protected override Object GetElementKey(ConfigurationElement element)
                    {
                        return ((ADFieldElement)element).Key;
                    }
                    public class ADFieldElement : ConfigurationElement
                    {
                        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
                        public string Key
                        {
                            get
                            {
                                return (string)this["key"];
                            }
                            set
                            {
                                this["key"] = value;
                            }
                        }
                        [ConfigurationProperty("adfield", IsRequired = false, IsKey = false)]
                        public string Adfield
                        {
                            get
                            {
                                return (string)this["adfield"];
                            }
                            set
                            {
                                this["adfield"] = value;
                            }
                        }
                        [ConfigurationProperty("maxLenght", IsRequired = false, IsKey = false)]
                        public int MaxLenght
                        {
                            get
                            {
                                return (int)this["maxLenght"];
                            }
                            set
                            {
                                this["maxLenght"] = value;
                            }
                        }
                        [ConfigurationProperty("default", IsRequired = false, IsKey = false)]
                        public string Default
                        {
                            get
                            {
                                return (string)this["default"];
                            }
                            set
                            {
                                this["default"] = value;
                            }
                        }
                    }
                }

                public class ObjectFieldsCollection : ConfigurationElementCollection
                {
                    protected override ConfigurationElement CreateNewElement()
                    {
                        return new ObjectFieldElement();
                    }

                    protected override Object GetElementKey(ConfigurationElement element)
                    {
                        return ((ObjectFieldElement)element).Key;
                    }
                    public class ObjectFieldElement : ConfigurationElement
                    {
                        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
                        public string Key
                        {
                            get
                            {
                                return (string)this["key"];
                            }
                            set
                            {
                                this["key"] = value;
                            }
                        }
                        [ConfigurationProperty("object", IsRequired = false, IsKey = false)]
                        public string Object
                        {
                            get
                            {
                                return (string)this["object"];
                            }
                            set
                            {
                                this["object"] = value;
                            }
                        }
                        [ConfigurationProperty("field", IsRequired = false, IsKey = false)]
                        public string Field
                        {
                            get
                            {
                                return (string)this["field"];
                            }
                            set
                            {
                                this["field"] = value;
                            }
                        }
                    }
                }

                public class FunctionsCollection : ConfigurationElementCollection
                {
                    protected override ConfigurationElement CreateNewElement()
                    {
                        return new FunctionElement();
                    }

                    protected override Object GetElementKey(ConfigurationElement element)
                    {
                        return ((FunctionElement)element).Key;
                    }
                    public class FunctionElement : MethodFunctionElement
                    {
                        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
                        public string Key
                        {
                            get
                            {
                                return (string)this["key"];
                            }
                            set
                            {
                                this["key"] = value;
                            }
                        }
                    }
                }

                public class ServiceElement : MethodFunctionElement
                {

                    [ConfigurationProperty("options", IsRequired = false)]
                    public string options
                    {
                        get { return (string)this["options"]; }
                        set { this["options"] = value; }
                    }

                    List<string> _options = null;
                    public List<string> Options
                    {
                        get {
                            if (_options == null)
                            {
                                if (options != null)
                                {
                                    _options = (options).Split('|').ToList();
                                }
                                else
                                {
                                    _options = new List<string>();
                                }
                            }
                             return _options;
                        }
                    }
                }
            }
            public class RolesElement : ConfigurationElement
            {
                [ConfigurationProperty("Values", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public ValueCollection Values
                {
                    get { return (ValueCollection)this["Values"]; }
                    set { this["Values"] = value; }
                }
                [ConfigurationProperty("AD", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                AddItemName = "add",
                ClearItemsName = "clear",
                RemoveItemName = "remove")]
                public KeyValueCollection AD
                {
                    get { return (KeyValueCollection)this["AD"]; }
                    set { this["AD"] = value; }
                }
            }
            public class LanguageElement : ConfigurationElement
            {
                /*
                [ConfigurationProperty("Mapping", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public KeyValueCollection Mapping
                {
                    get { return (KeyValueCollection)this["Mapping"]; }
                    set { this["Mapping"] = value; }
                }*/
                [ConfigurationProperty("default", IsRequired = true)]
                public string Default
                {
                    get { return (string)this["default"]; }
                    set { this["default"] = value; }
                }
            }
            public class UserProfileElement : ConfigurationElement
            {
                [ConfigurationProperty("Values", IsDefaultCollection = false)]
                [ConfigurationCollection(typeof(LayoutsCollection),
                    AddItemName = "add",
                    ClearItemsName = "clear",
                    RemoveItemName = "remove")]
                public KeyValueCollection Values
                {
                    get { return (KeyValueCollection)this["Values"]; }
                    set { this["Values"] = value; }
                }
            }
        }
    }
}
