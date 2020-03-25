namespace $safeprojectname$.Helpers
{
    using BIA.Net.Common.Helpers;
    using BIA.Net.Model.DAL;
    using Model.DAL;

    /// <summary>
    /// Configure unity in Model layer
    /// </summary>
    public static class UnityConfigModel
    {
        /// <summary>Registers the type mappings with the Unity container.</summary>
        public static void RegisterTypes()
        {
            // Singletons per request, durée de vie par requete http
            BIAUnity.RegisterType<TDBContainer<$saferootprojectname$DBContainer>, TDBContainer<$saferootprojectname$DBContainer>>();
        }
    }
}