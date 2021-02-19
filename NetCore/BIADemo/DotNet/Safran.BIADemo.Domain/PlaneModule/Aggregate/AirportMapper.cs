// BIADemo only
// <copyright file="AirportMapper.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using Safran.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class AirportMapper : BaseMapper<AirportDto, Airport>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Airport> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Airport>
                {
                    { "Id", planeType => planeType.Id },
                    { "Name", planeType => planeType.Name },
                    { "City", planeType => planeType.City },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(AirportDto dto, Airport entity)
        {
            if (entity == null)
            {
                entity = new Airport();
            }

            entity.Id = dto.Id;
            entity.Name = dto.Name;
            entity.City = dto.City;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Airport, AirportDto>> EntityToDto()
        {
            return entity => new AirportDto
            {
                Id = entity.Id,
                Name = entity.Name,
                City = entity.City,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<AirportDto, object[]> DtoToRecord()
        {
            return x => new object[]
            {
                x.Name,
                x.City,
            };
        }
    }
}