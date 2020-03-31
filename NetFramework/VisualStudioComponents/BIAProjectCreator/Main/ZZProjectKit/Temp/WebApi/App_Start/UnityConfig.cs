namespace $companyName$.$saferootprojectname$.MVC
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Authentication.Web;
    using BIA.Net.Common.Helpers;
    using System.Web;
    using Unity.Lifetime;
    using Business.DTO;
    using Business.Helpers;
    using Model.Helpers;
    using WebApi.Helpers;
    using System.Collections.Generic;
    using $safeprojectname$.Jwt;
    using System;

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
            BIAUnity.Init(typeof(PerResolveLifetimeManager));

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
        public static UserInfoWebApi CreateUserInfo()
        {
            UserInfoWebApi userInfo = null;

            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "OPTIONS")
            {
                userInfo = CreateEmptyUserInfo();
                userInfo.Roles = new List<string>() { "AllowVerbOptions" };
            }
            else
            {
                if (HttpContext.Current.User != null)
                {
                    userInfo = BaseAuthorizationFilter<UserInfoWebApi, UserDTO>.PrepareUserInfo();
                    HttpContext.Current.User = userInfo;
                }
            }

            return userInfo;
        }

        /// <summary>
        /// Creates the empty user information.
        /// </summary>
        /// <returns></returns>
        public static UserInfoWebApi CreateEmptyUserInfo()
        {
            UserInfoWebApi userInfo = new UserInfoWebApi();
            userInfo.RefreshUser(false, false, false, false, false);
            userInfo.Properties = new Business.DTO.UserDTO();
            userInfo.Login = string.Empty;
            return userInfo;
        }

        /// <summary>
        /// CreateUserInfo form JWT
        /// </summary>
        /// <returns>the userInfo link to authenticated user</returns>
        public static UserInfoWebApi CreateUserInfoJWT()
        {
            UserInfoWebApi userInfo = null;

            AppJwtManager appJwtManager = new AppJwtManager();

            var headers = HttpContext.Current.Request.Headers;
            string token = appJwtManager.GetToken(HttpContext.Current.Request);

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    userInfo = appJwtManager.RetrieveUserInfo(token);
                }
                catch (Exception ex)
                {
                    BIA.Net.Common.TraceManager.Error(ex.Message, ex);

                    userInfo = CreateEmptyUserInfo();
                    userInfo.Roles = new List<string>() { BiaJwtManager.ALLOW_GET_TOKEN };
                }

                userInfo.RefreshUser(false, false, false, false, false);
            }
            else
            {
                userInfo = CreateEmptyUserInfo();
                userInfo.Roles = new List<string>() { BiaJwtManager.ALLOW_GET_TOKEN };
            }

            return userInfo;
        }
    }
}