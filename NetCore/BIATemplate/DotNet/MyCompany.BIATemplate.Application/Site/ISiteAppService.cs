// <copyright file="ISiteAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.Site
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Application.Bia;
    using MyCompany.BIATemplate.Domain.Dto.Site;

    /// <summary>
    /// The interface defining the application service for site.
    /// </summary>
    public interface ISiteAppService : ICrudAppServiceBase<SiteDto, SiteFilterDto>
    {
        /// <summary>
        /// Get the list of SiteInfoDto with paging and sorting.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>The list of SiteInfoDto.</returns>
        Task<(IEnumerable<SiteInfoDto> Sites, int Total)> GetAllWithMembersAsync(SiteFilterDto filters);
    }
}