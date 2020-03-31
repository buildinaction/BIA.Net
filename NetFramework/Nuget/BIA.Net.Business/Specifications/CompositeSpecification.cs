// <copyright file="CompositeSpecification.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Specifications
{
    /// <summary>
    /// Base class for composite specifications.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification.</typeparam>
    public abstract class CompositeSpecification<TEntity> : Specification<TEntity>
    {
        /// <summary>
        /// Gets the Left side specification for this composite element.
        /// </summary>
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// Gets the Right side specification for this composite element.
        /// </summary>
        public abstract ISpecification<TEntity> RightSideSpecification { get; }
    }
}