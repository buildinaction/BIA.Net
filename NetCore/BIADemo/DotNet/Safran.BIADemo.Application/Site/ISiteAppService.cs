// <copyright file="ISiteAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Application.Site
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using Safran.BIADemo.Domain.Dto.Site;
    using Safran.BIADemo.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The interface defining the application service for site.
    /// </summary>
    public interface ISiteAppService : ICrudAppServiceBase<SiteDto, Site, SiteFilterDto>
    {
        /// <summary>
        /// Get the list of SiteInfoDto with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of SiteInfoDto.</returns>
        Task<(IEnumerable<SiteInfoDto> Sites, int Total)> GetAllWithMembersAsync(SiteFilterDto filters);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userRights">The user rights.</param>
        /// <returns>all sites.</returns>
        Task<IEnumerable<SiteDto>> GetAllAsync(int userId = 0, IEnumerable<string> userRights = null);
    }
}