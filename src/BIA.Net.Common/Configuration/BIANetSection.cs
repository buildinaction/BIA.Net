using BIA.Net.Common.Helpers;
using System;
using System.Configuration;

namespace BIA.Net.Common.Configuration
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
}