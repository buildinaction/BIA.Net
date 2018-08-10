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



        // Define the LayoutsCollection that contains the 
        // UrlsConfigElement elements.
        // This class shows how to use the ConfigurationElementCollection.
        public class ConfigCollection<ConfigElem> : ConfigurationElementCollection
            where ConfigElem : KeyElement, new()
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new ConfigElem();
            }

            protected override Object GetElementKey(ConfigurationElement element)
            {
                return ((ConfigElem)element).Key;
            }

        }

        public class KeyCollection : ConfigCollection<KeyElement> { }

        // Define the UrlsConfigElement elements that are contained 
        // by the LayoutsCollection.
        public class KeyElement : ConfigurationElement
        {
            [ConfigurationProperty("key", IsRequired = true)]
            public string Key
            {
                get { return (string)this["key"]; }
                set { this["key"] = value; }
            }
        }

        public class KeyValueCollection : ConfigCollection<KeyValueElement> { }

        // Define the UrlsConfigElement elements that are contained 
        // by the LayoutsCollection.
        public class KeyValueElement : KeyElement
        {
            [ConfigurationProperty("value", IsRequired = true, IsKey = true)]
            public string Value
            {
                get
                {
                    return (string)this["value"];
                }
                set
                {
                    this["value"] = value;
                }
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

        public class CustomCodeElement : ConfigurationElement
        {

            [ConfigurationProperty("function", IsRequired = true)]
            public string Function
            {
                get { return (string)this["function"]; }
                set { this["function"] = value; }
            }
        }

        /*<WebService URL = "$(UrlDMIndex)/UserProfile/GetUserProfile" >
          < Parameters >
            < add key="Login" object="UserInfo" field="Login" />
          </Parameters>
        </WebService>*/
        public class WebServicesCollection : ConfigCollection<WebServiceElement> { }
        public class WebServiceElement : KeyElement
        {

            [ConfigurationProperty("URL", IsRequired = true)]
            public string URL
            {
                get { return (string)this["URL"]; }
                set { this["URL"] = value; }
            }


            [ConfigurationProperty("Parameters", IsDefaultCollection = false)]
            [ConfigurationCollection(typeof(ObjectFieldsCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
            public ObjectFieldsCollection Parameters
            {
                get { return (ObjectFieldsCollection)this["Parameters"]; }
                set { this["Parameters"] = value; }
            }
        }

        public class ObjectFieldsCollection : ConfigCollection<ObjectFieldElement> {}
        public class ObjectFieldElement : KeyElement
        {
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
}
