// <copyright file="IQueryableUnitOfWork.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    using MyCompany.BIATemplate.Domain.Core;

    /// <summary>
    /// The interface base for Data context.
    /// </summary>
    public interface IQueryableUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Attach the item to the current context.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        void Attach<TEntity>(TEntity item)
            where TEntity : class;

        /// <summary>
        /// Get the ObjectSet of the of type TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The set of entity.</returns>
        DbSet<TEntity> RetrieveSet<TEntity>()
            where TEntity : class;

        /// <summary>
        /// Set the item as modified.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <typeparam name="TEntity">The entity type of the item.</typeparam>
        void SetModified<TEntity>(TEntity item)
            where TEntity : class;
    }
}