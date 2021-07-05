// <copyright file="IMemberAppService.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using TheBIADevCompany.BIATemplate.Domain.Dto.User;
    using TheBIADevCompany.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for member.
    /// </summary>
    public interface IMemberAppService : ICrudAppServiceBase<MemberDto, Member, MemberFilterDto>
    {
        /// <summary>
        /// Get the list of MemberDto with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of MemberDto.</returns>
        Task<(IEnumerable<MemberDto> Members, int Total)> GetRangeBySiteAsync(MemberFilterDto filters);

        /// <summary>
        /// Sets the default site.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultSiteAsync(int siteId);

        /// <summary>
        /// Sets the default role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task SetDefaultRoleAsync(int roleId);
    }
}