namespace $safeprojectname$
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Authentication.Web;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Helpers;
    using Model.Helpers;
    using MVC.Helpers;
    using System.Web;
    using Unity.AspNet.Mvc;

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes()
        {
            // Initialize the BIAUnity (provide a static Unity container...)
            BIAUnity.Init(typeof(PerRequestLifetimeManager));

            // Register Types of all other layers
            UnityConfigModel.RegisterTypes();
            UnityConfigBusiness.RegisterTypes();

            // Register Types of MVC layer
            BIAUnity.RegisterTypeContent<IUserInfo>(CreateUserInfo);
        }

        /// <summary>
        /// CreateUserInfo in http context
        /// </summary>
        /// <returns>the userInfo link to authenticated user</returns>
        public static UserInfoMVC CreateUserInfo()
        {
            UserInfoMVC userInfo = null;
            if (HttpContext.Current.User != null)
            {
                userInfo = BaseAuthorizationFilter<UserInfoMVC, UserDTO>.PrepareUserInfo();
                HttpContext.Current.User = userInfo;
            }

            return userInfo;
        }
    }
}