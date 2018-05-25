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
        // Create a "font" element.
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
}