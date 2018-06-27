namespace BIA.Net.Common.Helpers
{
    using System;
    using Unity;
    using Unity.Lifetime;

    /// <summary>
    /// Configure unity in Model layer
    /// </summary>
    public static class BIAUnity
    {
        /// <summary>
        /// Gets or sets the main container
        /// </summary>
        public static IUnityContainer RootContainer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is moq
        /// </summary>
        public static bool IsMoq { get; set; }

        /// <summary>
        /// Gets or sets the LifetimeManager Type
        /// </summary>
        public static Type LifetimeManagerType { get; set; }


        /// <param name="lifetimeManagerType">The lifetime manager type</param>
        /// <param name="isMoq">Is moq</param>
        public static void Init(Type lifetimeManagerType, bool isMoq = false)
        {
            RootContainer = new UnityContainer(); ;
            IsMoq = isMoq;
            LifetimeManagerType = lifetimeManagerType;
            // Singletons per request, durée de vie par requete http
            //container.RegisterType<IServiceSite, ServiceSite>((LifetimeManager)Activator.CreateInstance(lifetimeManagerType));
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <typeparam name="TFrom">Type from</typeparam>
        /// <typeparam name="TTo">Type to</typeparam>
        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            RootContainer.RegisterType<TFrom, TTo>((LifetimeManager)Activator.CreateInstance(LifetimeManagerType));
        }


        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="from">Type from</param>
        /// <param name="to">Type to</param>
        public static void RegisterType(Type from, Type to)
        {
            RootContainer.RegisterType(from, to, (LifetimeManager)Activator.CreateInstance(LifetimeManagerType));
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <typeparam name="T">Type from</typeparam>
        public static void RegisterType<T>()
        {
            RootContainer.RegisterType<T>((LifetimeManager)Activator.CreateInstance(LifetimeManagerType));
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="t">Type from</param>
        public static void RegisterType(Type t)
        {
            RootContainer.RegisterType(t, (LifetimeManager)Activator.CreateInstance(LifetimeManagerType));
        }

        /// <summary>
        /// Resolve Unity
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <returns>The object resolve</returns>
        public static TFrom Resolve<TFrom>()
        {
            return BIAUnity.RootContainer.Resolve<TFrom>();
        }

        /// <summary>
        /// Resolve Unity
        /// </summary>
        /// <param name="from"></param>
        /// <returns>The object resolve</returns>
        public static object Resolve(Type from)
        {
            return BIAUnity.RootContainer.Resolve(from);
        }



        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <typeparam name="T">Type from</typeparam>
        public static void RegisterTypeContent<Contents>(Func<object> ContentCreator)
        {
            if (BIAContentCreator.ContentsCreator.ContainsKey(typeof(Contents)))
            {
                BIAContentCreator.ContentsCreator[typeof(Contents)] = ContentCreator;
            }
            else
            {
                BIAContentCreator.ContentsCreator.Add(typeof(Contents), ContentCreator);
            }
            RootContainer.RegisterType<BIAContainer<Contents>>((LifetimeManager)Activator.CreateInstance(LifetimeManagerType));
        }

        /// <summary>
        /// Resolve Unity for Content
        /// </summary>
        /// <param name="from"></param>
        /// <returns>The object resolve</returns>
        public static TContentsFrom ResolveContent<TContentsFrom>()
        {
            BIAContainer<TContentsFrom> Container = BIAUnity.Resolve<BIAContainer<TContentsFrom>>();
            return Container.contents;
        }
    }
}