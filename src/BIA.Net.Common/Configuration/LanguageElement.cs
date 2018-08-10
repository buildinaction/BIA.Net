using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BIA.Net.Common.Configuration.CommonElement;

namespace BIA.Net.Common.Configuration
{
    public class LanguageElement : ConfigurationElement
    {
        // Declare the LayoutsCollection collection property.
        [ConfigurationProperty("ApplicationLanguages", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ApplicationLanguagesColection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public ApplicationLanguagesColection ApplicationLanguages
        {
            get
            {
                return (ApplicationLanguagesColection)this["ApplicationLanguages"];
            }

            set
            {
                this["ApplicationLanguages"] = value;
            }

        }

        public class ApplicationLanguagesColection : ConfigCollection<ApplicationLanguagesColection.ApplicationLanguageElement>
        {
            public class ApplicationLanguageElement : KeyElement
            {
                [ConfigurationProperty("name", IsRequired = true)]
                public string Name
                {
                    get { return (string)this["name"]; }
                    set { this["name"] = value; }
                }

                [ConfigurationProperty("shortName", IsRequired = true)]
                public string ShortName
                {
                    get { return (string)this["shortName"]; }
                    set { this["shortName"] = value; }
                }
            }
        }
        List<string> _applicationLanguages = null;
        public List<string> GetApplicationLanguages()
        {
            if (_applicationLanguages == null)
            {
                _applicationLanguages = new List<string>();
                foreach (ApplicationLanguagesColection.ApplicationLanguageElement language in ApplicationLanguages)
                {
                    _applicationLanguages.Add(language.Key);
                }
            }
            return _applicationLanguages;
        }

    }
}
