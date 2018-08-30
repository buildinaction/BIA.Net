// <copyright file="MapperBase.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Business.DTO.Infrastructure
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Base class for mapper Entity to DTO
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    public abstract class MapperBase<TEntity, TDto>
    {
        /// <summary>
        /// Gets the selector expression.
        /// </summary>
        /// <value>
        /// The selector expression.
        /// </value>
        public abstract Expression<Func<TEntity, TDto>> SelectorExpression { get; }

        /// <summary>
        /// Maps to model.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="model">The model.</param>
        public abstract void MapToModel(TDto dto, TEntity model);
    }
}