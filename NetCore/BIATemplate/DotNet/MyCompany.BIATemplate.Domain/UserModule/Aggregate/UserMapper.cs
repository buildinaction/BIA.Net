// <copyright file="UserMapper.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.UserModule.Aggregate
{
    using System;
    using System.Linq.Expressions;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.Dto.User;

    /// <summary>
    /// The mapper used for user.
    /// </summary>
    public class UserMapper
    {
        /// <summary>
        /// Gets or sets the collection used for expressions to access fields.
        /// </summary>
        public ExpressionCollection<User> ExpressionCollection
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
        public Expression<Func<User, UserDto>> EntityToDto()
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