// <copyright file="SiteSpecification.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.SiteModule.Aggregate
{
    using System.Linq;
    using BIA.Net.Specification;
    using MyCompany.BIADemo.Domain.Dto.Site;

    /// <summary>
    /// The specifications of the site entity.
    /// </summary>
    public static class SiteSpecification
    {
        /// <summary>
        /// Search site using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The specification.</returns>
        public static Specification<Site> SearchGetAll(SiteFilterDto filter)
        {
            Specification<Site> specification = new TrueSpecification<Site>();

            if (filter.UserId != 0)
            {
                specification &= new DirectSpecification<Site>(s =>
                    s.Members.Any(a => a.UserId == filter.UserId));
            }

            return specification;
        }
    }
}