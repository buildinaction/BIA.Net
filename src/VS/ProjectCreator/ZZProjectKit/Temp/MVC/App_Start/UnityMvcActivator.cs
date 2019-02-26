using BIA.Net.Common.Helpers;
using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof($safeprojectname$.UnityMvcActivator), nameof($safeprojectname$.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof($safeprojectname$.UnityMvcActivator), nameof($safeprojectname$.UnityMvcActivator.Shutdown))]

namespace $safeprojectname$
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityMvcActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start()
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            UnityConfig.RegisterTypes();
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(BIAUnity.RootContainer));

            DependencyResolver.SetResolver(new UnityDependencyResolver(BIAUnity.RootContainer));

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            BIAUnity.RootContainer.Dispose();
        }
    }
}