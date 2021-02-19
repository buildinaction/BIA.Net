// BIADemo only
// <copyright file="AirportOptionMapper.cs" company="Safran">
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
    public class AirportOptionMapper : BaseMapper<OptionDto, Airport>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Airport, OptionDto>> EntityToDto()
        {
            return entity => new OptionDto
            {
                Id = entity.Id,
                Display = entity.Name,
            };
        }
    }
}