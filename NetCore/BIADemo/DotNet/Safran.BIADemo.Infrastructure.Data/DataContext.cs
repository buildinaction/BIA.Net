// <copyright file="DataContext.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Infrastructure.Data
{
    using BIA.Net.Core.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    // Begin BIADemo
    using Safran.BIADemo.Domain.PlaneModule.Aggregate;

    // End BIADemo
    using Safran.BIADemo.Domain.SiteModule.Aggregate;
    using Safran.BIADemo.Domain.UserModule.Aggregate;
    using Safran.BIADemo.Domain.ViewModule.Aggregate;
    using Safran.BIADemo.Infrastructure.Data.ModelBuilders;

    /// <summary>
    /// The database context.
    /// </summary>
    public class DataContext : BIADataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public DataContext(DbContextOptions<DataContext> options, ILogger<BIADataContext> logger)
            : base(options, logger)
        {
        }

        /// <summary>
        /// Gets or sets the Site DBSet.
        /// </summary>
        public DbSet<Site> Sites { get; set; }

        /// <summary>
        /// Gets or sets the User DBSet.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Role DBSet.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the Member DBSet.
        /// </summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        public DbSet<View> Views { get; set; }

        // Begin BIADemo

        /// <summary>
        /// Gets or sets the Plane DBSet.
        /// </summary>
        public DbSet<Plane> Planes { get; set; }

        /// <summary>
        /// Gets or sets the Plane DBSet.
        /// </summary>
        public DbSet<Airport> Airports { get; set; }

        /// <summary>
        /// Gets or sets the Plane DBSet.
        /// </summary>
        public DbSet<PlaneType> PlanesTypes { get; set; }

        // End BIADemo

        /// <inheritdoc cref="DbContext.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SiteModelBuilder.CreateSiteModel(modelBuilder);
            UserModelBuilder.CreateModel(modelBuilder);
            ViewModelBuilder.CreateModel(modelBuilder);

            // Begin BIADemo
            PlaneModelBuilder.CreateModel(modelBuilder);

            // End BIADemo
            this.OnEndModelCreating(modelBuilder);
        }
    }
}