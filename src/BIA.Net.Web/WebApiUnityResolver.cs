namespace Safran.ZZProjectNameZZ.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using Unity;
    using Unity.Exceptions;

    /// <summary>
    /// Use to resolve unity in web api
    /// </summary>
    public class WebApiUnityResolver : IDependencyResolver
    {
        /// <summary>
        /// Containner of configuration
        /// </summary>
        private IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiUnityResolver"/> class.
        /// </summary>
        /// <param name="container">Unity Containner</param>
        public WebApiUnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        /// <summary>
        /// Get Service
        /// </summary>
        /// <param name="serviceType">type of service</param>
        /// <returns>the object resolved</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <param name="serviceType">type of service</param>
        /// <returns>the object resolve</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Begin Scope
        /// </summary>
        /// <returns>a WebApiUnityResolver</returns>
        public IDependencyScope BeginScope()
        {
            var child = container.CreateChildContainer();
            return new WebApiUnityResolver(child);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            container.Dispose();
        }
    }
}