// <copyright file="IocContainer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIATemplate.Crosscutting.Ioc
{
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Infrastructure.Data;
    using BIA.Net.Core.Infrastructure.Data.Repositories;
    using BIA.Net.Core.Infrastructure.Service.Repositories;
    using BIA.Net.Core.IocContainer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using TheBIADevCompany.BIATemplate.Application.Site;
    using TheBIADevCompany.BIATemplate.Application.User;
    using TheBIADevCompany.BIATemplate.Application.View;
    using TheBIADevCompany.BIATemplate.Domain.RepoContract;
    using TheBIADevCompany.BIATemplate.Domain.UserModule.Aggregate;
    using TheBIADevCompany.BIATemplate.Domain.UserModule.Service;
    using TheBIADevCompany.BIATemplate.Infrastructure.Data;
    using TheBIADevCompany.BIATemplate.Infrastructure.Data.Repositories.QueryCustomizer;
    using TheBIADevCompany.BIATemplate.Infrastructure.Service.Repositories;

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
