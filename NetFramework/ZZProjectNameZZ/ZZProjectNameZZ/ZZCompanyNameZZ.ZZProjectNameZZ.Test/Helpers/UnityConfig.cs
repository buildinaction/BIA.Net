namespace ZZCompanyNameZZ.ZZProjectNameZZ.Test.Helpers
{
    using BIA.Net.Common.Helpers;
    using BIA.Net.Authentication.Business.Helpers;
    using Business.Helpers;
    using Model.Helpers;
    using System;
    using Unity.Lifetime;

    public static class UnityConfig
    {
        /// <summary>
        /// Register all unity type of all layers.
        /// </summary>
        /// <param name="container"></param>
        public static void RegisterTypes()
        {
            BIAUnity.Init(typeof(ContainerControlledLifetimeManager), true);
            UnityConfigModel.RegisterTypes();
            UnityConfigBusiness.RegisterTypes();
            BIAUnity.RegisterTypeContent<IUserInfo>(UserInfoContainer.GetUserInfo);
        }
    }
}