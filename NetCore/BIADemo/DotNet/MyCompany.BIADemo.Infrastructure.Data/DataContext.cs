// <copyright file="DataContext.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Infrastructure.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    // Begin BIADemo
    using MyCompany.BIADemo.Domain.PlaneModule.Aggregate;

    // End BIADemo
    using MyCompany.BIADemo.Domain.SiteModule.Aggregate;
    using MyCompany.BIADemo.Domain.UserModule.Aggregate;
    using MyCompany.BIADemo.Domain.ViewModule.Aggregate;
    using MyCompany.BIADemo.Infrastructure.Data.ModelBuilders;

    /// <summary>
    /// The database context.
    /// </summary>
    public class DataContext : DbContext, IQueryableUnitOfWork
    {
        /// <summary>
        /// The current logger.
        /// </summary>
        private readonly ILogger<DataContext> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger)
            : base(options)
        {
            this.logger = logger;
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

        // End BIADemo
        /// <summary>
        /// Save Change on DataBase.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="T:System.Threading.CancellationToken"/> to observe while waiting for the
        /// task to complete.
        /// </param>
        /// <returns>Number of Modified Element.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var entities = from e in this.ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                     || e.State == EntityState.Modified
                               select e.Entity;
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext);
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (ValidationException exception)
            {
                this.logger.LogError(exception, "An error occured on entity validation.");
                this.RollbackChanges();
                throw new DataException(exception.Message, exception);
            }
            catch (DbUpdateException exception)
            {
                this.logger.LogError(exception, "An error occured while saving data.");
                this.RollbackChanges();
                throw new DataException("An error occured while saving data.", exception);
            }
        }

        /// <inheritdoc cref="IQueryableUnitOfWork.CommitAsync"/>
        public async Task<int> CommitAsync()
        {
            return await this.SaveChangesAsync();
        }

        /// <summary>
        /// Rollback changes in the current context.
        /// </summary>
        public void RollbackChanges()
        {
            this.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Unchanged);
        }

        /// <summary>
        /// Attach the item to the current context.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        public new void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Attach(item);
        }

        /// <summary>
        /// Get the ObjectSet of the of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The set of entity.</returns>
        public DbSet<TEntity> RetrieveSet<TEntity>()
            where TEntity : class
        {
            return this.Set<TEntity>();
        }

        /// <summary>
        /// Set the item as modified.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            this.Entry(item).State = EntityState.Modified;
        }

        /// <inheritdoc cref="DbContext.OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SiteModelBuilder.CreateSiteModel(modelBuilder);
            UserModelBuilder.CreateModel(modelBuilder);
            // Begin BIADemo
            PlaneModelBuilder.CreatePlaneModel(modelBuilder);
            // End BIADemo
            ViewModelBuilder.CreateModel(modelBuilder);
        }
    }
}