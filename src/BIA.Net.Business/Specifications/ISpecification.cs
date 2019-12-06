// <copyright file="ISpecification.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The interface used for the specification pattern.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public interface ISpecification<TEntity>
    {
        /// <summary>
        /// Check if this specification is satisfied by a specific expression lambda.
        /// </summary>
        /// <returns>Lambda expression.</returns>
        Expression<Func<TEntity, bool>> SatisfiedBy();
    }
}