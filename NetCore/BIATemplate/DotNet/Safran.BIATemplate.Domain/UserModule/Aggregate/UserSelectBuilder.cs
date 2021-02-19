// <copyright file="UserSelectBuilder.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.UserModule.Aggregate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Safran.BIATemplate.Domain.Dto.User;

    /// <summary>
    /// The select builder of the user entity.
    /// </summary>
    public static class UserSelectBuilder
    {
        /// <summary>
        /// Gets the expression used to select user.
        /// </summary>
        /// <returns>The expression.</returns>
        public static Expression<Func<User, UserDto>> EntityToDto()
        {
            return user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Login = user.Login,
                LastName = user.LastName,
                Guid = user.Guid,
                SiteIds = user.Members.Select(s => s.SiteId),
            };
        }

        /// <summary>
        /// Gets the expression used to select user.
        /// </summary>
        /// <returns>The expression.</returns>
        public static Expression<Func<User, UserInfoDto>> SelectUserInfo()
        {
            return user => new UserInfoDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Login = user.Login,
                LastName = user.LastName,
                Country = user.Country,
            };
        }
    }
}