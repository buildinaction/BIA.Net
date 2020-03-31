using BIA.Net.Common.Helpers;
using System;
using System.Configuration;
using System.Xml;

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

            public ConfigElem GetElemByKey(string key)
            {
                if (Count > 0)
                {
                    foreach (ConfigElem value in this)
                    {
                        if (value.Key == key)
                        {
                            return value;
                        }
                    }
                }
                return null;
            }
        }

        // Define the LayoutsCollection that contains the 
        // UrlsConfigElement elements.
        // This class shows how to use the ConfigurationElementCollection.
        public class HeterogeneousCollection : ConfigurationElementCollection
        {
            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((IHeterogeneousConfigurationElement)element).Key;
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return null;
            }

            protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
            {
                //if (elementName.("Test1") || elementName.Equals("Test2"))
                {
                    var element = (IHeterogeneousConfigurationElement)CreateNewElement(elementName);
                    element.Deserialize(reader);
                    BaseAdd((ConfigurationElement)element);

                    return true;
                }

                //return base.OnDeserializeUnrecognizedElement(elementName, reader);
            }

            protected override ConfigurationElement CreateNewElement(string elementName)
            {
                switch (elementName)
                {
                    case "WindowsIdentity":
                        return new WindowsIdentityElement(elementName);
                    case "ObjectField":
                        return new ObjectFieldElement(elementName);
                    case "Parameter":
                        return new ObjectFieldElement(elementName);
                    case "Function":
                        return new FunctionElement(elementName);
                    case "ADField":
                        return new ADFieldElement(elementName);
                    case "Value":
                        return new ValueElement(elementName);
                    case "CustomCode":
                        return new CustomCodeElement(elementName);
                    case "WebService":
                        return new WebServiceElement(elementName);
                    case "ADRole":
                        return new ValueElement(elementName);
                    case "Mapping":
                        return new MappingCollection(elementName);
                    case "ClientCertificateInHeader":
                        return new ClientCertificateInHeaderCollection(elementName);
                }

                throw new ConfigurationErrorsException("Unsupported element: " + elementName);
            }
        }

        public class ClientCertificateInHeaderCollection : HeterogeneousCollectionBase, IHeterogeneousConfigurationElement
        {
            public ClientCertificateInHeaderCollection(string elementName) : base(elementName) { }
            [ConfigurationProperty("windowsIdentity", IsRequired = true, IsKey = true)]
            public string WindowsIdentity
            {
                get { return (string)this["windowsIdentity"]; }
                set { this["windowsIdentity"] = value; }
            }
            protected override ConfigurationElement CreateNewElement(string elementName)
            {
                switch (elementName)
                {
                    case "Validation":
                        return new ValidationCollection(elementName);
                    case "CertField":
                        return new CertFieldElement(elementName);
                }

                throw new ConfigurationErrorsException("Unsupported element in ClientCertificate: " + elementName);
            }
        }

        public interface IHeterogeneousConfigurationElement
        {
            void Deserialize(XmlReader reader);
            string Key { get; set; }
            string TagName { get; set; }
        }

        public abstract class HeterogeneousConfigurationElementBase : ConfigurationElement, IHeterogeneousConfigurationElement
        {
            public HeterogeneousConfigurationElementBase(string elementName) : base()
            {
                TagName = elementName;
            }
            public string TagName { get; set; }
            [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
            public string Key
            {
                get { return (string)this["key"]; }
                set { this["key"] = value; }
            }
            public void Deserialize(XmlReader reader)
            {
                base.DeserializeElement(reader, false);
            }
        }
        public abstract class HeterogeneousCollectionBase : HeterogeneousCollection, IHeterogeneousConfigurationElement
        {
            public HeterogeneousCollectionBase(string elementName) : base()
            {
                TagName = elementName;
            }
            public string TagName { get; set; }
            [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
            public string Key
            {
                get { return (string)this["key"]; }
                set { this["key"] = value; }
            }
            public void Deserialize(XmlReader reader)
            {
                base.DeserializeElement(reader, false);
            }
        }


        public class MappingCollection : KeyValueCollection, IHeterogeneousConfigurationElement
        {
            public MappingCollection(string elementName) : base()
            {
                TagName = elementName;
            }
            public string TagName { get; set; }
            [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
            public string Key
            {
                get { return (string)this["key"]; }
                set { this["key"] = value; }
            }
            public void Deserialize(XmlReader reader)
            {
                base.DeserializeElement(reader, false);
            }
        }

        // Define the UrlsConfigElement elements that are contained 
        // by the LayoutsCollection.
        public class ValueElement : HeterogeneousConfigurationElementBase
        {
            public ValueElement(string elementName) : base(elementName) { }
            [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
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


        public class KeyCollection : ConfigCollection<KeyElement> { }

        // Define the UrlsConfigElement elements that are contained 
        // by the LayoutsCollection.
        public class KeyElement : ConfigurationElement
        {
            [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
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
            [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
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
        public class MethodFunctionElement : HeterogeneousConfigurationElementBase
        {
            public MethodFunctionElement(string elementName) : base(elementName) { }
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

        public class CustomCodeElement : HeterogeneousConfigurationElementBase
        {
            public CustomCodeElement(string elementName) : base(elementName) { }
            [ConfigurationProperty("function", IsRequired = false)]
            public string Function
            {
                get { return (string)this["function"]; }
                set { this["function"] = value; }
            }
        }

        public class WebServiceElement : HeterogeneousCollectionBase
        {
            public WebServiceElement(string elementName) : base(elementName) { }
            [ConfigurationProperty("URL", IsRequired = true)]
            public string URL
            {
                get { return (string)this["URL"]; }
                set { this["URL"] = value; }
            }
        }

        public class ObjectFieldElement : HeterogeneousConfigurationElementBase
        {
            public ObjectFieldElement(string elementName) : base(elementName) { }

            [ConfigurationProperty("source", IsRequired = true, IsKey = false)]
            public string Source
            {
                get
                {
                    return (string)this["source"];
                }
                set
                {
                    this["source"] = value;
                }
            }
        }

        public class ValidationCollection : KeyValueCollection, IHeterogeneousConfigurationElement
        {
            public ValidationCollection(string elementName) : base()
            {
                TagName = elementName;
            }
            public string TagName { get; set; }
            [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
            public string Key
            {
                get { return (string)this["key"]; }
                set { this["key"] = value; }
            }
            public void Deserialize(XmlReader reader)
            {
                base.DeserializeElement(reader, false);
            }
        }



        public class WindowsIdentityElement : HeterogeneousConfigurationElementBase
        {
            public WindowsIdentityElement(string elementName) : base(elementName) { }
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
        public class CertFieldElement : HeterogeneousConfigurationElementBase
        {

            public CertFieldElement(string elementName) : base(elementName) { }
            [ConfigurationProperty("certfield", IsRequired = false, IsKey = false)]
            public string CertField
            {
                get
                {
                    return (string)this["certfield"];
                }
                set
                {
                    this["certfield"] = value;
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
        public class ADFieldElement : HeterogeneousConfigurationElementBase
        {

            public ADFieldElement(string elementName) : base(elementName) { }
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


        public class FunctionElement : MethodFunctionElement
        {
            public FunctionElement(string elementName) : base(elementName) { }
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
}
