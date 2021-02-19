// <copyright file="MemberAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Domain.RepoContract;
    using Safran.BIATemplate.Domain.Dto.User;
    using Safran.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for member.
    /// </summary>
    public class MemberAppService : CrudAppServiceBase<MemberDto, Member, MemberFilterDto, MemberMapper>, IMemberAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly BIAClaimsPrincipal principal;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="queryCustomizer">The query customizer.</param>
        public MemberAppService(ITGenericRepository<Member> repository, IPrincipal principal/*, IMemberQueryCustomizer queryCustomizer*/)
            : base(repository)
        {
            this.principal = principal as BIAClaimsPrincipal;

            // Include already add with the mapper MemberMapper
            // this.Repository.QueryCustomizer = queryCustomizer
        }

        /// <inheritdoc cref="IMemberAppService.GetRangeBySiteAsync"/>
        public async Task<(IEnumerable<MemberDto> Members, int Total)> GetRangeBySiteAsync(MemberFilterDto filters)
        {
            return await this.GetRangeAsync(filters: filters, specification: MemberSpecification.SearchGetAll(filters));
        }

        /// <inheritdoc cref="IMemberAppService.SetDefaultSite"/>
        public async Task SetDefaultSiteAsync(int siteId)
        {
            int userId = this.principal.GetUserId();
            if (userId > 0 && siteId > 0)
            {
                IList<Member> members = (await this.Repository.GetAllEntityAsync(filter: x => x.UserId == userId)).ToList();

                if (members?.Any() == true)
                {
                    foreach (Member member in members)
                    {
                        member.IsDefault = member.SiteId == siteId;
                        this.Repository.Update(member);
                    }

                    await this.Repository.UnitOfWork.CommitAsync();
                }
            }
        }
    }
}