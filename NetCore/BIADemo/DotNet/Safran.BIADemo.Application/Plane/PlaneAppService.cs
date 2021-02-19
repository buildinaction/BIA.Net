// BIADemo only
// <copyright file="PlaneAppService.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Application.Plane
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using BIA.Net.Core.Application;
    using BIA.Net.Core.Application.Authentication;
    using BIA.Net.Core.Domain.Dto.Base;
    using BIA.Net.Core.Domain.Dto.User;
    using BIA.Net.Core.Domain.RepoContract;
    using BIA.Net.Core.Domain.Specification;
    using Safran.BIADemo.Domain.Dto.Plane;
    using Safran.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// The application service used for plane.
    /// </summary>
    public class PlaneAppService : CrudAppServiceListAndItemBase<PlaneDto, PlaneListItemDto, Plane, LazyLoadDto, PlaneMapper, PlaneListMapper>, IPlaneAppService
    {
        /// <summary>
        /// The claims principal.
        /// </summary>
        private readonly int currentSiteId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaneAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="principal">The claims principal.</param>
        /// <param name="planeQueryCustomizer">The plane query customizer for include.</param>
        public PlaneAppService(ITGenericRepository<Plane> repository, IPrincipal principal)
            : base(repository)
        {
            this.currentSiteId = (principal as BIAClaimsPrincipal).GetUserData<UserDataDto>().CurrentSiteId;
            this.filtersContext.Add(AccessMode.Read, new DirectSpecification<Plane>(p => p.SiteId == this.currentSiteId));
        }

        /// <inheritdoc/>
        public override Task<PlaneDto> AddAsync(PlaneDto dto)
        {
            dto.Site = new Domain.Dto.Site.SiteDto { Id = this.currentSiteId };
            return base.AddAsync(dto);
        }

        Task<IEnumerable<PlaneListItemDto>> ICrudAppServiceListAndItemBase<PlaneDto, PlaneListItemDto, Plane, LazyLoadDto>.GetAllAsync<TKey>(Expression<Func<Plane, TKey>> orderByExpression, bool ascending, int id, Specification<Plane> specification, Expression<Func<Plane, bool>> filter, int firstElement, int pageCount, Expression<Func<Plane, object>>[] includes, string accessMode, string queryMode)
        {
            throw new NotImplementedException();
        }
    }
}