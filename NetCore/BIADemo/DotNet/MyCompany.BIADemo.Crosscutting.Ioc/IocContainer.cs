// <copyright file="IocContainer.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Crosscutting.Ioc
{
    using BIA.Net.ActiveDirectory;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    // Begin BIADemo
    using MyCompany.BIADemo.Application.Plane;

    // End BIADemo
    using MyCompany.BIADemo.Application.Site;
    using MyCompany.BIADemo.Application.User;
    using MyCompany.BIADemo.Application.View;
    using MyCompany.BIADemo.Crosscutting.Common.Configuration.BiaNet;
    using MyCompany.BIADemo.Domain.Core;
    using MyCompany.BIADemo.Domain.UserModule.Service;
    using MyCompany.BIADemo.Infrastructure.Data;
    using MyCompany.BIADemo.Infrastructure.Data.Repositories;

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
                options.UseSqlServer(configuration.GetConnectionString("BIADemoDatabase")));
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
            // Begin BIADemo
            collection.AddTransient<IPlaneAppService, PlaneAppService>();
            // End BIADemo

            // Configuration
            collection.Configure<BiaNetSection>(options => configuration.GetSection("BiaNet").Bind(options));
        }
    }
}