// <copyright file="RoleAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Application.Bia;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.Dto.User;
    using MyCompany.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for role.
    /// </summary>
    public class RoleAppService : AppServiceBase, IRoleAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public RoleAppService(IGenericRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="IRoleAppService.GetAllAsync"/>
        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return await this.Repository.GetAllAsync<Role, RoleDto>(role => new RoleDto { Id = role.Id, Label = role.Label });
        }
    }
}