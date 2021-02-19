// <copyright file="RoleAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Domain.RepoContract;
    using Safran.BIATemplate.Domain.Dto.User;
    using Safran.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for role.
    /// </summary>
    public class RoleAppService : AppServiceBase<Role>, IRoleAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public RoleAppService(ITGenericRepository<Role> repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="IRoleAppService.GetAllAsync"/>
        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return await this.Repository.GetAllResultAsync<RoleDto>(role => new RoleDto { Id = role.Id, Label = role.Label });
        }
    }
}