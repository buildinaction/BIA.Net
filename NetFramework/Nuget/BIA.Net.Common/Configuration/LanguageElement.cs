namespace BIA.Net.Common.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using static BIA.Net.Common.Configuration.CommonElement;

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

        List<LanguageInfo> _applicationLanguages = null;

        /// <summary>
        /// Get all application language.
        /// </summary>
        /// <returns>The list of language info.</returns>
        public List<LanguageInfo> GetApplicationLanguages()
        {
            if (_applicationLanguages == null)
            {
                _applicationLanguages = new List<LanguageInfo>();
                foreach (ApplicationLanguagesColection.ApplicationLanguageElement language in ApplicationLanguages)
                {
                    _applicationLanguages.Add(new LanguageInfo { Code = language.Key, Name = language.Name, ShortName = language.ShortName });
                }
            }
            return _applicationLanguages;
        }

    }
}
