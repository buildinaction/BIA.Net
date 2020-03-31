// <copyright file="ViewMapper.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.ViewModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using MyCompany.BIADemo.Domain.Dto.View;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public static class ViewMapper
    {
        /// <summary>
        /// Create a view DTO from an entity.
        /// </summary>
        /// <returns>The view DTO.</returns>
        public static Expression<Func<View, ViewDto>> EntityToDto(int userId)
        {
            return entity => new ViewDto
            {
                Id = entity.Id,
                Name = entity.Name,
                TableId = entity.TableId,
                ViewType = (int)entity.ViewType,
                Description = entity.Description,
                IsSiteDefault = entity.ViewSites.Any(a => a.IsDefault),
                IsUserDefault = entity.ViewUsers.Any(a => a.UserId == userId && a.IsDefault),
                Preference = entity.Preference,
            };
        }
    }
}