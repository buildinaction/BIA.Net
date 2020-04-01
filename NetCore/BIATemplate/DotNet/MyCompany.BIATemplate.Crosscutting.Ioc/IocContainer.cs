// <copyright file="IocContainer.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Crosscutting.Ioc
{
    using BIA.Net.ActiveDirectory;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MyCompany.BIATemplate.Application.Site;
    using MyCompany.BIATemplate.Application.User;
    using MyCompany.BIATemplate.Application.View;
    using MyCompany.BIATemplate.Crosscutting.Common.Configuration.BiaNet;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.UserModule.Service;
    using MyCompany.BIATemplate.Infrastructure.Data;
    using MyCompany.BIATemplate.Infrastructure.Data.Repositories;

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
        public static void ConfigureContainer(IServiceCollection collection, IConfiguration configuration)
        {
            // Crosscutting
            collection.AddSingleton<IADHelper, ADHelper>();

            // Infrastructure Data
            collection.AddDbContext<IQueryableUnitOfWork, DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BIATemplateDatabase")));
            collection.AddTransient<IGenericRepository, GenericRepository>();

            // Domain
            collection.AddTransient<IUserRightDomainService, UserRightDomainService>();
            collection.AddTransient<IUserSynchronizeDomainService, UserSynchronizeDomainService>();

            // Application
            collection.AddTransient<ISiteAppService, SiteAppService>();
            collection.AddTransient<IMemberAppService, MemberAppService>();
            collection.AddTransient<IRoleAppService, RoleAppService>();
            collection.AddTransient<IUserAppService, UserAppService>();
            collection.AddTransient<IViewAppService, ViewAppService>();

            // Configuration
            collection.Configure<BiaNetSection>(options => configuration.GetSection("BiaNet").Bind(options));
        }
    }
}
