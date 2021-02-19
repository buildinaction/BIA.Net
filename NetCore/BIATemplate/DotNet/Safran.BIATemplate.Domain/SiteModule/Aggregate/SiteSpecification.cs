// <copyright file="SiteSpecification.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.SiteModule.Aggregate
{
    using System.Linq;
    using BIA.Net.Core.Domain.Specification;
    using Safran.BIATemplate.Domain.Dto.Site;

    /// <summary>
    /// The specifications of the site entity.
    /// </summary>
    public static class SiteSpecification
    {
        /// <summary>
        /// Search site using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="siteId">The site identifier.</param>
        /// <returns>
        /// The specification.
        /// </returns>
        public static Specification<Site> SearchGetAll(SiteFilterDto filter, int siteId)
        {
            Specification<Site> specification = new TrueSpecification<Site>();

            if (filter.UserId > 0)
            {
                specification &= new DirectSpecification<Site>(s =>
                    s.Members.Any(a => a.UserId == filter.UserId));
            }

            if (siteId > 0)
            {
                specification &= new DirectSpecification<Site>(s =>
                    s.Members.Any(a => a.SiteId == siteId));
            }

            return specification;
        }
    }
}