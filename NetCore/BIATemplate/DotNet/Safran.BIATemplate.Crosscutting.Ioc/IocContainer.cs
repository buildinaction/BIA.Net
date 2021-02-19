// <copyright file="IocContainer.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Crosscutting.Ioc
{
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.IocContainer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Safran.BIATemplate.Application.Site;
    using Safran.BIATemplate.Application.User;
    using Safran.BIATemplate.Application.View;
    using Safran.BIATemplate.Domain.RepoContract;
    using Safran.BIATemplate.Domain.UserModule.Aggregate;
    using Safran.BIATemplate.Domain.UserModule.Service;
    using Safran.BIATemplate.Infrastructure.Data;
    using Safran.BIATemplate.Infrastructure.Data.Repositories.QueryCustomizer;
    using Safran.BIATemplate.Infrastructure.Service.Repositories;

    /// <summary>
    /// The IoC Container.
    /// </summary>
    public static class IocContainer
    {
        /// <summary>
        /// The method used to register all instance.
        /// </summary>
        /// <param name="collection">The collection of service.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="isUnitTest">Are we configuring IoC for unit tests? If so, some IoC shall not be performed here but replaced by
        /// specific ones in IocContainerTest.</param>
        public static void ConfigureContainer(IServiceCollection collection, IConfiguration configuration, bool isUnitTest = false)
        {
            BIAIocContainer.ConfigureContainer(collection, configuration, isUnitTest);

            ConfigureInfrastructureServiceContainer(collection);
            ConfigureDomainContainer(collection);
            ConfigureApplicationContainer(collection);

            if (!isUnitTest)
            {
                ConfigureInfrastructureDataContainer(collection, configuration);
                ConfigureCommonContainer(collection, configuration);
            }
        }

        private static void ConfigureApplicationContainer(IServiceCollection collection)
        {
            // Application Layer
            collection.AddTransient<ISiteAppService, SiteAppService>();
            collection.AddTransient<IMemberAppService, MemberAppService>();
            collection.AddTransient<IRoleAppService, RoleAppService>();
            collection.AddTransient<IUserAppService, UserAppService>();
            collection.AddTransient<IViewAppService, ViewAppService>();
        }

        private static void ConfigureDomainContainer(IServiceCollection collection)
        {
            // Domain Layer
            collection.AddTransient<IUserRightDomainService, UserRightDomainService>();
            collection.AddTransient<IUserSynchronizeDomainService, UserSynchronizeDomainService>();
        }

        private static void ConfigureCommonContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Common Layer
        }

        private static void ConfigureInfrastructureDataContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Infrastructure Data Layer
            collection.AddDbContext<IQueryableUnitOfWork, DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BIATemplateDatabase"));
                options.EnableSensitiveDataLogging();
            });
            collection.AddScoped(typeof(ITGenericRepository<>), typeof(TGenericRepositoryEF<>));
            collection.AddTransient<IMemberQueryCustomizer, MemberQueryCustomizer>();
            collection.AddTransient<IViewQueryCustomizer, ViewQueryCustomizer>();
        }

        private static void ConfigureInfrastructureServiceContainer(IServiceCollection collection)
        {
            // Infrastructure Service Layer
            collection.AddSingleton<IUserDirectoryRepository<UserFromDirectory>, LdapRepository>();
            collection.AddTransient<INotification, MailRepository>();
        }
    }
}
