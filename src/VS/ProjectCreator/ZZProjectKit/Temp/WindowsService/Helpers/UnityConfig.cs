// <copyright file="UnityConfig.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Helpers
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common.Helpers;
    using Business.DTO;
    using Business.Helpers;
    using Model.Helpers;
    using Unity.Lifetime;

    /// <summary>
    /// Configure Unity helper
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Register all unity type of all layers.
        /// </summary>
        public static void RegisterTypes()
        {
            BIAUnity.Init(typeof(PerResolveLifetimeManager));
            UnityConfigModel.RegisterTypes();
            UnityConfigBusiness.RegisterTypes();
            BIAUnity.RegisterTypeContent<IUserInfo>(CreateUserInfo);
        }

        /// <summary>
        /// CreateUserInfo for windows service
        /// </summary>
        /// <returns>UserInfo</returns>
        public static UserInfo CreateUserInfo()
        {
            UserInfo userInfo = new UserInfo
            {
                Properties = new UserDTO()
                {
                    Id = 0,
                    Login = "$saferootprojectname$_WindowsService"
                },
                Roles = new System.Collections.Generic.List<string>() { "Service" }
            };
            return userInfo;
        }
    }
}