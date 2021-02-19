// <copyright file="UserMapper.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.UserModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using BIA.Net.Core.Domain;
    using Safran.BIATemplate.Domain.Dto.User;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class UserMapper : BaseMapper<UserDto, User>
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public override ExpressionCollection<User> ExpressionCollection
        {
            get
            {
                return new ExpressionCollection<User>
                   {
                       { "Id", user => user.Id },
                       { "LastName", user => user.LastName },
                       { "FirstName", user => user.FirstName },
                       { "Login", user => user.Login },
                       { "Guid", user => user.Guid },
                   };
            }
        }

        /// <summary>
        /// Create a user DTO from an entity.
        /// </summary>
        /// <returns>The user DTO.</returns>
        public override Expression<Func<User, UserDto>> EntityToDto()
        {
            return entity => new UserDto
            {
                Id = entity.Id,
                LastName = entity.LastName,
                FirstName = entity.FirstName,
                Login = entity.Login,
                Guid = entity.Guid,
            };
        }
    }
}