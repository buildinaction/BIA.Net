// <copyright file="SiteAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.Site
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Application.Bia;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.Dto.Site;
    using MyCompany.BIATemplate.Domain.SiteModule.Aggregate;

    /// <summary>
    /// The application service used for site.
    /// </summary>
    public class SiteAppService : CrudAppServiceBase<SiteDto, Site, SiteFilterDto, SiteMapper>, ISiteAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public SiteAppService(IGenericRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="ISiteAppService.GetAllWithMembersAsync"/>
        public async Task<(IEnumerable<SiteInfoDto> Sites, int Total)> GetAllWithMembersAsync(SiteFilterDto filters)
        {
            var mapper = new SiteMapper();

            var specifications = SpecificationHelper.GetLazyLoad(
                SiteSpecification.SearchGetAll(filters),
                mapper.ExpressionCollection,
                filters);

            var queryOrder = this.GetQueryOrder(mapper.ExpressionCollection, filters?.SortField, filters?.SortOrder == 1);

            var results = await this.Repository.GetBySpecAndCountAsync(
                mapper.EntityToSiteInfo(),
                specifications,
                queryOrder,
                filters?.First ?? 0,
                filters?.Rows ?? 0);

            return (results.Item1.ToList(), results.Item2);
        }
    }
}