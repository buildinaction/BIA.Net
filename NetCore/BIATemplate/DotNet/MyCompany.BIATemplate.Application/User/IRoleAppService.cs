// <copyright file="IRoleAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Domain.Dto.User;

    /// <summary>
    /// The interface defining the application service for role.
    /// </summary>
    public interface IRoleAppService
    {
        /// <summary>
        /// Get all existing roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        Task<IEnumerable<RoleDto>> GetAllAsync();
    }
}