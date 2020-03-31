// <copyright file="BaseMapper.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Core
{
    using System;
    using System.Linq.Expressions;
    using MyCompany.BIADemo.Domain.Dto.Bia;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseMapper<TDto, TEntity>
        where TDto : BaseDto
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Gets the collection used for expressions to access fields.
        /// </summary>
        public abstract ExpressionCollection<TEntity> ExpressionCollection { get; }

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        public abstract void DtoToEntity(TDto dto, TEntity entity);

        /// <summary>
        /// Create a DTO from an entity.
        /// </summary>
        /// <returns>The created DTO.</returns>
        public abstract Expression<Func<TEntity, TDto>> EntityToDto();

        /// <summary>
        /// Defining the includes to use in the update method when updating related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public abstract Expression<Func<TEntity, object>>[] IncludesForUpdate();

        /// <summary>
        /// Create a record from a DTO.
        /// </summary>
        /// <returns>Func.</returns>
        public virtual Func<TDto, object[]> DtoToRecord()
        {
            throw new NotImplementedException();
        }
    }
}