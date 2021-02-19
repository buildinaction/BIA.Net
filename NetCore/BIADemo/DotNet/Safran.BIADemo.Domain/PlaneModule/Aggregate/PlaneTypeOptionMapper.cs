// BIADemo only
// <copyright file="PlaneTypeOptionMapper.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using Safran.BIADemo.Domain.Dto.Option;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class PlaneTypeOptionMapper : BaseMapper<OptionDto, PlaneType>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<PlaneType, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.Title,
            };
        }
    }
}