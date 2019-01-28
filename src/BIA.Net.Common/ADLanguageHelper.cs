using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Common
{
    public class ADLanguageHelper
    {

        /// <summary>
        /// Retrurn the language to use in application for the user and default en-US
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetADLanguageOrDefault(string userlanguage)
        {
            string languageCode = GetADLanguage(userlanguage);
            if (languageCode == null)
            {
                return "en-US";
            }
            return languageCode;
        }

        /// <summary>
        /// Retrurn the language to use in application for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetADLanguage(string userlanguage)
        {
            string languageCode = null;
            if (!string.IsNullOrEmpty(userlanguage))
            {
                switch (userlanguage)
                {
                    case "FR":
                    case "MA":
                        languageCode = "fr-FR";
                        break;
                    case "ES":
                    case "MX":
                        languageCode = "es-ES";
                        break;
                    case "GB":
                        languageCode = "en-GB";
                        break;
                    case "US":
                        languageCode = "en-US";
                        break;
                    case "DE":
                        languageCode = "de-DE";
                        break;
                }
            }
            return languageCode;
        }
    }
}
