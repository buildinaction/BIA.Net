// <copyright file="WcfServiceFactory.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>
namespace ZZCompanyNameZZ.ZZProjectNameZZ.Connector
{
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using Business.Helpers;
    using Model.Helpers;
    using Unity;
    using Unity.Lifetime;
    using Unity.Wcf;

    /// <summary>
    /// WcfServiceFactory
    /// </summary>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// Configure Unity for Connector layer
        /// </summary>
        /// <param name="container">unity root container</param>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            TraceManager.Configure();
            TraceManager.Debug("WcfServiceFactory", "ConfigureContainer", "Begin");

            ConfigureRootContainer(container);

            // register all your components with the container here
            container.RegisterType<IExampleService, ExampleService>(new PerResolveLifetimeManager());
        }

        private static void ConfigureRootContainer(IUnityContainer container)
        {
            BIAUnity.Init(typeof(HierarchicalLifetimeManager));
            BIAUnity.RootContainer = container;
            UnityConfigModel.RegisterTypes();

            BIAUnity.Init(typeof(PerResolveLifetimeManager));
            BIAUnity.RootContainer = container;
            UnityConfigBusiness.RegisterTypes();
        }
    }
}