// <copyright file="MemberAppService.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Application.User
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MyCompany.BIATemplate.Application.Bia;
    using MyCompany.BIATemplate.Domain.Core;
    using MyCompany.BIATemplate.Domain.Dto.User;
    using MyCompany.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// The application service used for member.
    /// </summary>
    public class MemberAppService : CrudAppServiceBase<MemberDto, Member, MemberFilterDto, MemberMapper>, IMemberAppService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAppService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public MemberAppService(IGenericRepository repository)
            : base(repository)
        {
        }

        /// <inheritdoc cref="IMemberAppService.GetAllBySiteAsync"/>
        public async Task<(IEnumerable<MemberDto> Members, int Total)> GetAllBySiteAsync(MemberFilterDto filters)
        {
            var mapper = new MemberMapper();

            var specifications = SpecificationHelper.GetLazyLoad(
                MemberSpecification.SearchGetAll(filters),
                mapper.ExpressionCollection,
                filters);

            var queryOrder = this.GetQueryOrder(mapper.ExpressionCollection, filters?.SortField, filters?.SortOrder == 1);

            var results = await this.Repository.GetBySpecAndCountAsync(
                mapper.EntityToDto(),
                specifications,
                queryOrder,
                filters?.First ?? 0,
                filters?.Rows ?? 0);

            return (results.Item1.ToList(), results.Item2);
        }
    }
}