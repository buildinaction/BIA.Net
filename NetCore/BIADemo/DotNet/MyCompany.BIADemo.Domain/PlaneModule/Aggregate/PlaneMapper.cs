// <copyright file="PlaneMapper.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.PlaneModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using MyCompany.BIADemo.Domain.Core;
    using MyCompany.BIADemo.Domain.Dto.Plane;

    /// <summary>
    /// The mapper used for plane.
    /// </summary>
    public class PlaneMapper : BaseMapper<PlaneDto, Plane>
    {
        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.ExpressionCollection"/>
        public override ExpressionCollection<Plane> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Plane>
                {
                    { "Id", plane => plane.Id },
                    { "Msn", plane => plane.Msn },
                    { "IsActive", plane => plane.IsActive },
                    { "FirstFlightDate", plane => plane.FirstFlightDate },
                    { "FirstFlightTime", plane => plane.FirstFlightDate },
                    { "LastFlightDate", plane => plane.LastFlightDate },
                    { "Capacity", plane => plane.Capacity },
                };
            }
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToEntity"/>
        public override void DtoToEntity(PlaneDto dto, Plane entity)
        {
            if (entity == null)
            {
                entity = new Plane();
            }

            entity.Id = dto.Id;
            entity.Msn = dto.Msn;
            entity.IsActive = dto.IsActive;
            entity.FirstFlightDate = new DateTime(
                dto.FirstFlightDate.Year,
                dto.FirstFlightDate.Month,
                dto.FirstFlightDate.Day,
                dto.FirstFlightTime.Hour,
                dto.FirstFlightTime.Minute,
                dto.FirstFlightTime.Second);
            entity.LastFlightDate = dto.LastFlightDate;
            entity.Capacity = dto.Capacity;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.EntityToDto"/>
        public override Expression<Func<Plane, PlaneDto>> EntityToDto()
        {
            return entity => new PlaneDto
            {
                Id = entity.Id,
                Msn = entity.Msn,
                IsActive = entity.IsActive,
                FirstFlightDate = entity.FirstFlightDate.Date,
                FirstFlightTime = new DateTime(
                    entity.FirstFlightDate.Year,
                    entity.FirstFlightDate.Month,
                    entity.FirstFlightDate.Day,
                    entity.FirstFlightDate.Hour,
                    entity.FirstFlightDate.Minute,
                    entity.FirstFlightDate.Second),
                LastFlightDate = entity.LastFlightDate,
                Capacity = entity.Capacity,
            };
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Plane, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Plane, object>>[0];
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.DtoToRecord"/>
        public override Func<PlaneDto, object[]> DtoToRecord()
        {
            return x => new object[]
            {
                x.Msn,
                x.IsActive ? "X" : string.Empty,
                x.FirstFlightDate.ToString("yyyy-MM-dd"),
                x.FirstFlightTime.ToString("hh:mm"),
                x.LastFlightDate?.ToString("yyyy-MM-dd hh:mm"),
                x.Capacity.ToString(),
            };
        }
    }
}