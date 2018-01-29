namespace BIA.Net.ImageManager.Common
{
    using System.Configuration;

    public static class AppSettingsReader
    {
        /// <summary>
        /// Gets the path where the uploaded images are located.
        /// </summary>
        public static string ProjectImagesPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ProjectImagesPath"];
            }
        }
    }
}
