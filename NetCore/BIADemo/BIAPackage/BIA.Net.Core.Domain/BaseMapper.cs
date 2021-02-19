// <copyright file="BaseMapper.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.Net.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The class used to define the base mapper.
    /// </summary>
    /// <typeparam name="TDto">The DTO type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class BaseMapper<TDto, TEntity> : BaseEntityMapper<TEntity>
        where TDto : BaseDto
        where TEntity : class, IEntity
    {

        /// <summary>
        /// Create an entity from a DTO.
        /// </summary>
        /// <param name="dto">The DTO to use.</param>
        /// <param name="entity">The entity to update with the DTO values.</param>
        public virtual void DtoToEntity(TDto dto, TEntity entity)
        {
            throw new NotImplementedException("This mapper is not build to manipulate entity, or the implementation of DtoToEntity is missing.");
        }


        /// <summary>
        /// Create a DTO from an entity.
        /// </summary>
        /// <returns>The created DTO.</returns>
        public abstract Expression<Func<TEntity, TDto>> EntityToDto();

        /// <summary>
        /// Defining the includes to use in the update method when updating related entities.
        /// </summary>
        /// <returns>The array of includes.</returns>
        public virtual Expression<Func<TEntity, object>>[] IncludesForUpdate()
        {
            return null;
        }

        /// <summary>
        /// Create a record from a DTO.
        /// </summary>
        /// <returns>Func.</returns>
        public virtual Func<TDto, object[]> DtoToRecord()
        {
            throw new NotImplementedException("This mapper is not build to generate reccords, or the implementation of DtoToRecord is missing.");
        }
    }
}