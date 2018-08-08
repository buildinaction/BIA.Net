
namespace BIA.Net.Common.Configuration
{
    using System.Configuration;
    using static BIA.Net.Common.Configuration.CommonElement;
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
                return (LayoutsCollection)this["Layouts"];
            }

            set
            {
                this["Layouts"] = value;
            }

        }
    }
}
