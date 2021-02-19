// <copyright file="MemberQueryCustomizer.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>
namespace Safran.BIATemplate.Infrastructure.Data.Repositories.QueryCustomizer
{
    using System.Linq;
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using Microsoft.EntityFrameworkCore;
    using Safran.BIATemplate.Domain.RepoContract;
    using Safran.BIATemplate.Domain.UserModule.Aggregate;

    /// <summary>
    /// Class use to customize the EF request on Member entity.
    /// </summary>
    public class MemberQueryCustomizer : TQueryCustomizer<Member>, IMemberQueryCustomizer
    {
        /// <inheritdoc/>
        public override IQueryable<Member> CustomizeAfter(IQueryable<Member> objectSet, string queryMode)
        {
            if (queryMode == QueryMode.Update)
            {
                return objectSet.Include(member => member.MemberRoles);
            }

            return objectSet;
        }
    }
}
