using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BIA.Net.Common
{
    public class BIANetSection : ConfigurationSection
    {
        [ConfigurationProperty("Authentication", IsRequired = false)]
        public AuthenticationElement Authentication
        {
            get
            {
                return (AuthenticationElement)this["Authentication"];
            }

            set
            {
                this["Authentication"] = value;
            }
        }

        [ConfigurationProperty("Dialog", IsRequired = false)]
        public DialogElement Dialog
        {
            get
            {
                return (DialogElement)this["Dialog"];
            }

            set
            {
                this["Dialog"] = value;
            }
        }
    }

    public class AuthenticationElement : ConfigurationElement
    {
        [ConfigurationProperty("Parameters", IsRequired = false)]
        public ParametersElement Parameters
        {
            get { return (ParametersElement)this["Parameters"];}
            set { this["Parameters"] = value;}
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
                    get { return (KeyValueCollection)this["Domains"];}
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
                    get { return (string)this["default"];}
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


    public class DialogElement : ConfigurationElement
    {
        // Declare the LayoutsCollection collection property.
        [ConfigurationProperty("Layouts", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(LayoutsCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public LayoutsCollection Layouts
        {
            get
            {
                return (LayoutsCollection) this["Layouts"];
            }

            set
            {
                this["Layouts"] = value;
            }

        }
    }
    // Define the LayoutsCollection that contains the 
    // UrlsConfigElement elements.
    // This class shows how to use the ConfigurationElementCollection.
    public class LayoutsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LayoutElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((LayoutElement)element).Name;
        }

    }
    // Define the UrlsConfigElement elements that are contained 
    // by the LayoutsCollection.
    public class LayoutElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }


    // Define the LayoutsCollection that contains the 
    // UrlsConfigElement elements.
    // This class shows how to use the ConfigurationElementCollection.
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

    }
    // Define the UrlsConfigElement elements that are contained 
    // by the LayoutsCollection.
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
                return (bool)(this["removeDomain"]==null?false: this["removeDomain"]) ;
            }
            set
            {
                this["removeDomain"] = value;
            }
        }
        /*
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
        public int Default
        {
            get
            {
                return (int)this["default"];
            }
            set
            {
                this["default"] = value;
            }
        }*/
    }

    // Define the LayoutsCollection that contains the 
    // UrlsConfigElement elements.
    // This class shows how to use the ConfigurationElementCollection.
    public class KeyValueCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValueElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((KeyValueElement)element).Key;
        }

    }
    // Define the UrlsConfigElement elements that are contained 
    // by the LayoutsCollection.
    public class KeyValueElement : ValueElement
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
    // Define the LayoutsCollection that contains the 
    // UrlsConfigElement elements.
    // This class shows how to use the ConfigurationElementCollection.
    public class ValueCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ValueElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ValueElement)element).Value;
        }

    }
    // Define the UrlsConfigElement elements that are contained 
    // by the LayoutsCollection.
    public class ValueElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}