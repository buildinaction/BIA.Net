// <copyright file="IRoleAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Safran.BIATemplate.Domain.Dto.User;

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