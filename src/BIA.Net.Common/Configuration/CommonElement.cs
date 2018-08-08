using BIA.Net.Common.Helpers;
using System;
using System.Configuration;

namespace BIA.Net.Common.Configuration
{
    public class CommonElement
    {

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



        // Define the UrlsConfigElement elements that are contained 
        // by the LayoutsCollection.
        public class MethodFunctionElement : ConfigurationElement
        {

            [ConfigurationProperty("type", IsRequired = true)]
            public string type
            {
                get { return (string)this["type"]; }
                set { this["type"] = value; }
            }

            Type _type = null;

            public Type Type
            {
                get
                {
                    if (_type == null) _type = TypeHelper.GetTypeFromString(type);
                    return _type;
                }
            }

            [ConfigurationProperty("method", IsRequired = false)]
            public string Method
            {
                get { return (string)this["method"]; }
                set { this["method"] = value; }
            }

            [ConfigurationProperty("property", IsRequired = false)]
            public string Property
            {
                get { return (string)this["property"]; }
                set { this["property"] = value; }
            }
        }
    }
}
