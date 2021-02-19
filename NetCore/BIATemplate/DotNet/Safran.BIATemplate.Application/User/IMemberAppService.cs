// <copyright file="IMemberAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using Safran.BIATemplate.Domain.Dto.User;
    using Safran.BIATemplate.Domain.UserModule.Aggregate;

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
    }
}