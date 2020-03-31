﻿// <copyright file="SiteMapper.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using MyCompany.BIADemo.Crosscutting.Common.Enum;
    using MyCompany.BIADemo.Domain.Core;
    using MyCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The mapper used for site.
    /// </summary>
    public class SiteMapper : BaseMapper<SiteDto, Site>
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<Site> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<Site> { { "Id", site => site.Id }, { "Title", site => site.Title }, };
            }
        }

        /// <summary>
        /// Create a site DTO from a entity.
        /// </summary>
        /// <returns>The site DTO.</returns>
        public override Expression<Func<Site, SiteDto>> EntityToDto()
        {
            return entity => new SiteDto { Id = entity.Id, Title = entity.Title };
        }

        /// <summary>
        /// Create a site entity from a DTO.
        /// </summary>
        /// <param name="dto">The site DTO.</param>
        /// <param name="entity">The entity to update.</param>
        public override void DtoToEntity(SiteDto dto, Site entity)
        {
            if (entity == null)
            {
                entity = new Site();
            }

            entity.Id = dto.Id;
            entity.Title = dto.Title;
        }

        /// <inheritdoc cref="BaseMapper{TDto,TEntity}.IncludesForUpdate"/>
        public override Expression<Func<Site, object>>[] IncludesForUpdate()
        {
            return new Expression<Func<Site, object>>[0];
        }

        /// <summary>
        /// Create a site info DTO from a entity.
        /// </summary>
        /// <returns>The site info DTO.</returns>
        public Expression<Func<Site, SiteInfoDto>> EntityToSiteInfo()
        {
            return entity => new SiteInfoDto
            {
                Id = entity.Id,
                Title = entity.Title,
                SiteAdmins = entity.Members
                    .Where(w => w.MemberRoles.Any(a => a.RoleId == (int)RoleEnum.SiteAdmin))
                    .Select(s => new SiteMemberDto { UserFirstName = s.User.FirstName, UserLastName = s.User.LastName, UserLogin = s.User.Login }),
            };
        }
    }
}